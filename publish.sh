#!/bin/bash

# ==============================================================================
# Interactive NuGet Publisher with Auto-Versioning
# ==============================================================================

set -e

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m'

# Security: Read API Key from environment variable instead of command-line argument
if [ -z "$NUGET_API_KEY" ]; then
  API_KEY=$1
  if [ -z "$API_KEY" ]; then
    echo -e "${RED}Error: NuGet API Key is missing.${NC}"
    echo -e ""
    echo -e "Please provide the API key in one of two ways:"
    echo -e "  1. Set environment variable: export NUGET_API_KEY=your-key-here"
    echo -e "  2. Pass as argument: ./publish.sh your-key-here"
    echo -e ""
    echo -e "${YELLOW}WARNING: Using environment variable is more secure!${NC}"
    exit 1
  fi
else
  API_KEY=$NUGET_API_KEY
fi

SOURCE="https://api.nuget.org/v3/index.json"
OUTPUT_DIR="./nupkgs"

# Find all non-test projects
# We use a while loop to read lines into an array safely
PROJECTS=()
while IFS= read -r line; do
    PROJECTS+=("$line")
done < <(find . -maxdepth 2 -name "*.csproj" | grep -v ".Tests")

if [ ${#PROJECTS[@]} -eq 0 ]; then
    echo -e "${RED}No projects found!${NC}"
    exit 1
fi

echo -e "${CYAN}=========================================${NC}"
echo -e "${CYAN}   Interactive NuGet Publisher           ${NC}"
echo -e "${CYAN}=========================================${NC}"
echo -e "Found ${#PROJECTS[@]} projects:"

# Display Menu
i=0
for proj in "${PROJECTS[@]}"; do
    echo -e "  [$i] $proj"
    i=$((i+1))
done
echo -e "  [a] Publish ALL"
echo -e "  [q] Quit"

echo -e ""
read -p "Select an option: " CHOICE

SELECTED_PROJECTS=()

if [ "$CHOICE" == "q" ]; then
    echo "Exiting."
    exit 0
elif [ "$CHOICE" == "a" ]; then
    SELECTED_PROJECTS=("${PROJECTS[@]}")
elif [[ "$CHOICE" =~ ^[0-9]+$ ]] && [ "$CHOICE" -ge 0 ] && [ "$CHOICE" -lt ${#PROJECTS[@]} ]; then
    SELECTED_PROJECTS=("${PROJECTS[$CHOICE]}")
else
    echo -e "${RED}Invalid selection.${NC}"
    exit 1
fi

# Ensure output dir exists
mkdir -p "$OUTPUT_DIR"

# Process selected projects
for PROJ_PATH in "${SELECTED_PROJECTS[@]}"; do
    echo -e "\n--------------------------------------------------"
    echo -e "🚀 Processing: ${GREEN}$PROJ_PATH${NC}"

    # 1. Get Current Version
    # Try to find <Version> tag
    CURRENT_VERSION=$(grep -oP '(?<=<Version>).*?(?=</Version>)' "$PROJ_PATH" || true)
    
    if [ -z "$CURRENT_VERSION" ]; then
        echo -e "${YELLOW}No <Version> tag found. Defaulting to 1.0.0${NC}"
        CURRENT_VERSION="1.0.0"
        # Insert Version tag if missing (simple insertion before </PropertyGroup>)
        # This is a bit hacky, assuming standard csproj structure
        if grep -q "</PropertyGroup>" "$PROJ_PATH"; then
             sed -i 's|</PropertyGroup>|  <Version>1.0.0</Version>\n  </PropertyGroup>|' "$PROJ_PATH"
        else
             echo -e "${RED}Could not automatically add Version tag. Please add <Version>1.0.0</Version> to $PROJ_PATH manually.${NC}"
             exit 1
        fi
    fi

    echo -e "   Current Version: $CURRENT_VERSION"

    # 2. Bump Version (Patch)
    IFS='.' read -r -a PARTS <<< "$CURRENT_VERSION"
    MAJOR=${PARTS[0]}
    MINOR=${PARTS[1]}
    PATCH=${PARTS[2]}
    
    # Handle cases where version might be 1.0 (missing patch)
    if [ -z "$MINOR" ]; then MINOR=0; fi
    if [ -z "$PATCH" ]; then PATCH=0; fi

    NEW_PATCH=$((PATCH + 1))
    NEW_VERSION="$MAJOR.$MINOR.$NEW_PATCH"
    
    echo -e "   Bumping to:      ${GREEN}$NEW_VERSION${NC}"

    # 3. Update .csproj
    # Use sed to replace the version. 
    # We use a temporary file to avoid issues on some systems, though -i works on most modern linux sed.
    sed -i "s|<Version>$CURRENT_VERSION</Version>|<Version>$NEW_VERSION</Version>|g" "$PROJ_PATH"

    # 4. Pack
    echo -e "   📦 Packing..."
    dotnet pack "$PROJ_PATH" --configuration Release --output "$OUTPUT_DIR" /p:ContinuousIntegrationBuild=true > /dev/null

    # Find the package we just created (read PackageId from csproj, fallback to filename)
    PACKAGE_ID=$(grep -oP '(?<=<PackageId>).*?(?=</PackageId>)' "$PROJ_PATH" || true)
    if [ -z "$PACKAGE_ID" ]; then
        PACKAGE_NAME=$(basename "$PROJ_PATH" .csproj)
    else
        PACKAGE_NAME="$PACKAGE_ID"
    fi
    NUPKG="$OUTPUT_DIR/$PACKAGE_NAME.$NEW_VERSION.nupkg"

    if [ ! -f "$NUPKG" ]; then
        echo -e "${RED}Error: Package file not found at $NUPKG${NC}"
        exit 1
    fi

    # 5. Push
    echo -e "   ⬆️  Pushing to NuGet..."
    OUTPUT=$(dotnet nuget push "$NUPKG" --api-key "$API_KEY" --source "$SOURCE" --skip-duplicate 2>&1)
    EXIT_CODE=$?

    if [ $EXIT_CODE -eq 0 ]; then
        echo -e "   ✅  ${GREEN}Published Successfully${NC}"
    elif [[ $OUTPUT == *"already exists"* ]]; then
        echo -e "   ⚠️  Package already exists (Skipped)"
    else
        echo -e "${RED}   ❌  Failed to publish${NC}"
        echo -e "$OUTPUT"
        exit 1
    fi
done

echo -e "\n${CYAN}Done!${NC}"
