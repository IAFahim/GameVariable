@echo off
setlocal EnableDelayedExpansion

:: ==============================================================================
:: Interactive NuGet Publisher - Clean ASCII Version
:: ==============================================================================

:: Force UTF-8 Encoding to prevent filename issues
chcp 65001 >nul

:: --- Configuration ---
:: Security: Read API Key from environment variable instead of command-line argument
if "%NUGET_API_KEY%"=="" (
    set "API_KEY=%~1"
    if "!API_KEY!"=="" (
        echo [ERROR] NuGet API Key is missing.
        echo.
        echo Please provide the API key in one of two ways:
        echo   1. Set environment variable: set NUGET_API_KEY=your-key-here
        echo   2. Pass as argument: publish.bat your-key-here
        echo.
        echo WARNING: Using environment variable is more secure!
        exit /b 1
    )
) else (
    set "API_KEY=%NUGET_API_KEY%"
)

set "SOURCE=https://api.nuget.org/v3/index.json"
set "OUTPUT_DIR=.\nupkgs"

:: --- Safe ANSI Color Setup ---
:: This method creates a real ESC character (ASCII 27)
for /F %%a in ('echo prompt $E ^| cmd') do set "ESC=%%a"

set "GREEN=!ESC![32m"
set "RED=!ESC![31m"
set "YELLOW=!ESC![33m"
set "CYAN=!ESC![36m"
set "NC=!ESC![0m"

:: --- Validation ---
:: Validation is now handled above in the configuration section

:: --- Find Projects ---
echo.
echo !CYAN!Scanning for projects...!NC!

set count=0
for /f "delims=" %%F in ('dir /s /b *.csproj ^| findstr /v /i ".Tests"') do (
    set "projects[!count!]=%%F"
    set /a count+=1
)

if %count%==0 (
    echo !RED!No projects found!!NC!
    exit /b 1
)

:: --- Menu ---
echo !CYAN!=========================================!NC!
echo !CYAN!   Interactive NuGet Publisher           !NC!
echo !CYAN!=========================================!NC!
echo Found %count% projects:

set /a last_index=%count%-1
for /L %%i in (0,1,%last_index%) do (
    echo   [%%i] !projects[%%i]!
)
echo   [a] Publish ALL
echo   [q] Quit
echo.

set /p CHOICE="Select an option: "

:: --- Selection Logic ---
if /i "%CHOICE%"=="q" goto :EOF

set "TARGET_INDICES="

if /i "%CHOICE%"=="a" (
    for /L %%i in (0,1,%last_index%) do (
        set "TARGET_INDICES=!TARGET_INDICES! %%i"
    )
) else (
    echo %CHOICE%| findstr /r "^[0-9]*$" >nul
    if errorlevel 1 (
        echo !RED!Invalid selection.!NC!
        exit /b 1
    )
    if %CHOICE% GTR %last_index% (
        echo !RED!Selection out of range.!NC!
        exit /b 1
    )
    set "TARGET_INDICES= %CHOICE%"
)

if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"

:: --- Processing Loop ---
for %%i in (%TARGET_INDICES%) do (
    call :ProcessProject "!projects[%%i]!"
)

echo.
echo !CYAN!Done!!NC!
goto :EOF

:: ==============================================================================
:: Helper Function
:: ==============================================================================
:ProcessProject
set "PROJ_PATH=%~1"
echo.
echo --------------------------------------------------
echo !CYAN!Processing:!NC! %PROJ_PATH%

:: 1. Get Version & Bump Patch (PowerShell)
for /f "usebackq tokens=*" %%V in (`powershell -NoProfile -Command ^
    "$path = '%PROJ_PATH%'; " ^
    "$content = Get-Content $path -Raw; " ^
    "$verMatch = [regex]::Match($content, '(?<=<Version>).*?(?=</Version>)'); " ^
    "if (-not $verMatch.Success) { Write-Host 'DEFAULT'; exit 0; } " ^
    "$ver = $verMatch.Value; " ^
    "$parts = $ver.Split('.'); " ^
    "if ($parts.Length -lt 3) { $parts += '0' * (3 - $parts.Length) }; " ^
    "$patch = [int]$parts[2] + 1; " ^
    "$newVer = $parts[0] + '.' + $parts[1] + '.' + $patch; " ^
    "$content = $content.Replace('<Version>' + $ver + '</Version>', '<Version>' + $newVer + '</Version>'); " ^
    "Set-Content $path $content -NoNewline; " ^
    "Write-Host $newVer;"`) do (
    set "NEW_VERSION=%%V"
)

:: 2. Handle Missing Version Tag
if "%NEW_VERSION%"=="DEFAULT" (
    echo    !YELLOW![WARN] No Version tag found. Defaulting to 1.0.0!NC!
    set "NEW_VERSION=1.0.0"
    
    powershell -NoProfile -Command ^
        "$path = '%PROJ_PATH%'; " ^
        "$content = Get-Content $path -Raw; " ^
        "if ($content -match '</PropertyGroup>') { " ^
        "   $content = $content -replace '</PropertyGroup>', \"  <Version>1.0.0</Version>`n  </PropertyGroup>\"; " ^
        "   Set-Content $path $content -NoNewline; " ^
        "} else { exit 1 }"
        
    if !errorlevel! NEQ 0 (
        echo !RED![FAIL] Could not automatically add Version tag.!NC!
        exit /b 1
    )
) else (
    echo    Bumping to:      !GREEN!!NEW_VERSION!!NC!
)

:: 3. Pack
echo    Packing...
dotnet pack "%PROJ_PATH%" --configuration Release --output "%OUTPUT_DIR%" /p:ContinuousIntegrationBuild=true
if !errorlevel! NEQ 0 (
    echo    !RED![FAIL] Pack Failed!NC!
    exit /b 1
)

:: 4. Calculate Nupkg Name (Read PackageId from csproj, fallback to filename)
for /f "usebackq tokens=*" %%P in (`powershell -NoProfile -Command ^
    "$content = Get-Content '%PROJ_PATH%' -Raw; " ^
    "$idMatch = [regex]::Match($content, '(?<=<PackageId>).*?(?=</PackageId>)'); " ^
    "if ($idMatch.Success) { Write-Output $idMatch.Value; exit 0 } " ^
    "$filename = Split-Path '%PROJ_PATH%' -Leaf; " ^
    "Write-Output ($filename -replace '\.csproj$', ''); "`) do (
    set "PACKAGE_ID=%%P"
)

set "NUPKG=%OUTPUT_DIR%\!PACKAGE_ID!.%NEW_VERSION%.nupkg"

if not exist "%NUPKG%" (
    echo !RED![FAIL] Package file not found at %NUPKG%!NC!
    exit /b 1
)

:: 5. Push
echo    Pushing to NuGet...
dotnet nuget push "%NUPKG%" --api-key "%API_KEY%" --source "%SOURCE%" --skip-duplicate >nul 2>&1

:: We use GOTO logic to avoid If-Block syntax errors with colors
if !errorlevel! NEQ 0 goto :PushFail

:: Success
echo    !GREEN![OK] Published Successfully!NC!
exit /b 0

:PushFail
echo    !YELLOW![WARN] Failed or Already Exists (Skipped)!NC!
exit /b 0