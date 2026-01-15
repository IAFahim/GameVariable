#!/bin/bash
set -e

# Self-preservation: Copy script to /tmp and run from there if we are inside the repo
SCRIPT_PATH=$(readlink -f "$0")
SCRIPT_NAME=$(basename "$0")
TMP_SCRIPT="/tmp/$SCRIPT_NAME"

if [ "$0" != "$TMP_SCRIPT" ]; then
    cp "$SCRIPT_PATH" "$TMP_SCRIPT"
    chmod +x "$TMP_SCRIPT"
    exec "$TMP_SCRIPT" "$@"
fi

# Configuration
REPORT_FILE="BENCHMARK_REPORT.md"
REPO_ROOT=$(pwd)

# Ensure we are in the root of the repo
if [[ "$REPO_ROOT" == "/tmp" ]]; then
    echo "Error: Please run this script from the repository root."
    exit 1
fi

# Trap for cleanup
ORIGINAL_BRANCH=$(git branch --show-current)
STASHED=0

cleanup() {
    echo "Cleaning up..."
    # If we are in detached head, try to return to original branch
    if [ -n "$ORIGINAL_BRANCH" ]; then
        git checkout "$ORIGINAL_BRANCH" 2>/dev/null || true
    else
        # If we were already detached, we can't easily go back, but we can try to checkout the tip of HEAD we started with?
        # For now, just ensuring we don't leave stashed changes is the main thing.
        true
    fi

    if [ "$STASHED" -eq 1 ]; then
        echo "Restoring stashed changes..."
        git stash pop >/dev/null 2>&1 || echo "Warning: Failed to pop stash or stash was already popped."
    fi

    rm -rf bench_current bench_previous
}

trap cleanup EXIT

# 1. Identify Commits
CURRENT_COMMIT=$(git rev-parse HEAD)
# Find the last commit before 24 hours ago
PREVIOUS_COMMIT=$(git rev-list -n 1 --before="24 hours ago" HEAD 2>/dev/null || echo "")

if [ -z "$PREVIOUS_COMMIT" ]; then
    # Fallback if history is too short (less than 24 hours old repo?)
    # Use the first commit
    echo "Warning: Could not find commit from 24 hours ago. Using first commit."
    PREVIOUS_COMMIT=$(git rev-list --max-parents=0 HEAD | head -n 1)
fi

echo "Current Commit: $CURRENT_COMMIT"
echo "Previous Commit (approx 24h ago): $PREVIOUS_COMMIT"

if [ "$CURRENT_COMMIT" == "$PREVIOUS_COMMIT" ]; then
    echo "No changes in the last 24 hours. Exiting."
    exit 0
fi

echo "# Benchmark Report" > "$REPORT_FILE"
echo "Generated on $(date)" >> "$REPORT_FILE"
echo "Comparison: $PREVIOUS_COMMIT (Old) vs $CURRENT_COMMIT (New)" >> "$REPORT_FILE"
echo "" >> "$REPORT_FILE"

# Function to run benchmarks and save output to a specific directory
run_benchmarks_to_dir() {
    local output_dir=$1
    local label=$2

    mkdir -p "$output_dir"

    echo "Running benchmarks for: $label"

    # Clean build environment
    # Use find to delete bin/obj to be safer/compatible than globstar
    find . -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} + 2>/dev/null || true

    # 1. IntentBenchmarks
    local intent_project="GameVariable.Intent.Tests/GameVariable.Intent.Tests.csproj"
    if [ -f "$intent_project" ]; then
        echo "  Running IntentBenchmarks..."
        rm -rf BenchmarkDotNet.Artifacts
        if dotnet run -c Release --project "$intent_project" -- --filter "*" > "$output_dir/intent_run.log" 2>&1; then
            local report_md=$(find BenchmarkDotNet.Artifacts/results -name "*-report-github.md" | head -n 1)
            if [ -f "$report_md" ]; then
                cp "$report_md" "$output_dir/intent_report.md"
            else
                echo "Report not found" > "$output_dir/intent_report.md"
            fi
        else
            echo "Build/Run Failed" > "$output_dir/intent_error.log"
            cat "$output_dir/intent_run.log" >> "$output_dir/intent_error.log"
        fi
    else
        echo "Project not found" > "$output_dir/intent_missing.log"
    fi

    # 2. RpgStatBenchmark
    local rpg_project="Variable.RPG.Tests/Variable.RPG.Tests.csproj"
    if [ -f "$rpg_project" ]; then
        echo "  Running RpgStatBenchmark..."
        if dotnet test -c Release "$rpg_project" --filter "FullyQualifiedName=Variable.RPG.Tests.Performance.RpgStatBenchmark.BenchmarkToStringCompact" --logger "console;verbosity=detailed" > "$output_dir/rpg_run.log" 2>&1; then
             if grep -q "Standard Output Messages:" "$output_dir/rpg_run.log"; then
                 grep -A 10 "Standard Output Messages:" "$output_dir/rpg_run.log" | sed 's/  Standard Output Messages://' | grep -v "Test Run Successful" | grep -v "Total tests" > "$output_dir/rpg_stats.txt"
             else
                 tail -n 20 "$output_dir/rpg_run.log" > "$output_dir/rpg_stats.txt"
             fi
        else
            echo "Build/Run Failed" > "$output_dir/rpg_error.log"
            cat "$output_dir/rpg_run.log" >> "$output_dir/rpg_error.log"
        fi
    else
        echo "Project not found" > "$output_dir/rpg_missing.log"
    fi
}

# Function to generate report section from a run directory
generate_report_section() {
    local dir=$1
    local title=$2

    echo "## $title" >> "$REPORT_FILE"

    echo "### GameVariable.Intent.Tests" >> "$REPORT_FILE"
    if [ -f "$dir/intent_report.md" ]; then
        cat "$dir/intent_report.md" >> "$REPORT_FILE"
    elif [ -f "$dir/intent_error.log" ]; then
        echo "FAILED" >> "$REPORT_FILE"
        echo '```' >> "$REPORT_FILE"
        head -n 20 "$dir/intent_error.log" >> "$REPORT_FILE"
        echo "..." >> "$REPORT_FILE"
        echo '```' >> "$REPORT_FILE"
    elif [ -f "$dir/intent_missing.log" ]; then
        echo "Project not found (Not implemented or deleted)" >> "$REPORT_FILE"
    else
        echo "No data" >> "$REPORT_FILE"
    fi

    echo "### Variable.RPG.Tests" >> "$REPORT_FILE"
    if [ -f "$dir/rpg_stats.txt" ]; then
        echo '```' >> "$REPORT_FILE"
        cat "$dir/rpg_stats.txt" >> "$REPORT_FILE"
        echo '```' >> "$REPORT_FILE"
    elif [ -f "$dir/rpg_error.log" ]; then
        echo "FAILED" >> "$REPORT_FILE"
        echo '```' >> "$REPORT_FILE"
        head -n 20 "$dir/rpg_error.log" >> "$REPORT_FILE"
        echo "..." >> "$REPORT_FILE"
        echo '```' >> "$REPORT_FILE"
    elif [ -f "$dir/rpg_missing.log" ]; then
        echo "Project not found (Not implemented or deleted)" >> "$REPORT_FILE"
    else
        echo "No data" >> "$REPORT_FILE"
    fi
    echo "" >> "$REPORT_FILE"
}

# --- Main Execution ---

# 1. Run on Current Commit
echo "Step 1: Benchmarking Current Commit..."
run_benchmarks_to_dir "bench_current" "Current Commit"

# 2. Run on Previous Commit
echo "Step 2: Benchmarking Previous Commit..."
if [ -n "$PREVIOUS_COMMIT" ]; then
    if [ -n "$(git status --porcelain)" ]; then
        echo "Stashing changes..."
        git stash push -m "benchmark_temp_stash"
        STASHED=1
    fi

    echo "Checking out $PREVIOUS_COMMIT..."
    if git checkout "$PREVIOUS_COMMIT" 2>/dev/null; then
        run_benchmarks_to_dir "bench_previous" "Previous Commit"
    else
        echo "Failed to checkout previous commit."
    fi
    # Cleanup trap will handle restoring branch and stash
else
    echo "No previous commit found."
fi

# 3. Generate Comparison Report
echo "Step 3: Generating Report..."
generate_report_section "bench_previous" "Previous Day ($PREVIOUS_COMMIT)"
generate_report_section "bench_current" "Current Day ($CURRENT_COMMIT)"

echo "Detailed report saved to $REPORT_FILE"
cat "$REPORT_FILE"

# Cleanup happens via trap
