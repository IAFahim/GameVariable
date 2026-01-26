#!/bin/bash
set -e

# Setup directories
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
REPO_ROOT="$SCRIPT_DIR/.."
HISTORY_DIR="$SCRIPT_DIR/benchmark_history"

# Artifacts are generated in the current working directory by default.
# We will change to REPO_ROOT to ensure consistency.
cd "$REPO_ROOT"
ARTIFACTS_DIR="$REPO_ROOT/BenchmarkDotNet.Artifacts/results"

mkdir -p "$HISTORY_DIR"

# Check for changes
COMMIT_FILE="$SCRIPT_DIR/last_run_commit"

if [ -f "$COMMIT_FILE" ]; then
    LAST_COMMIT=$(cat "$COMMIT_FILE")
    echo "Last run commit: $LAST_COMMIT"

    # Check if there are changes between LAST_COMMIT and HEAD
    if git diff --quiet "$LAST_COMMIT" HEAD; then
        echo "No changes detected since last run. Exiting."
        exit 0
    else
        echo "Changes detected since $LAST_COMMIT."
    fi
else
    echo "No last run commit found. Running for the first time."
fi

# Ensure .NET is in PATH (in case it wasn't added permanently or we are in a new shell without source)
export PATH="$HOME/.dotnet:$PATH"

# Build
echo "Building benchmarks..."
dotnet build -c Release "$REPO_ROOT/GameVariable.Benchmarks/GameVariable.Benchmarks.csproj"

# Run
echo "Running benchmarks..."
# Using --join to get a single combined report.
# Note: --join might create a report named "BenchmarkRun-report-full.json" or similar.
# Allow passing a filter as the first argument, default to "*"
FILTER="${1:-*}"
dotnet run -c Release --project "$REPO_ROOT/GameVariable.Benchmarks/GameVariable.Benchmarks.csproj" -- --filter "$FILTER" --join

# Find the generated JSON report
# We look for the most recently modified json file in the artifacts directory
LATEST_REPORT=$(ls -t "$ARTIFACTS_DIR"/*-report-full.json 2>/dev/null | head -n 1)

if [ -z "$LATEST_REPORT" ]; then
    echo "No benchmark report found in $ARTIFACTS_DIR!"
    echo "Contents of artifacts dir:"
    ls -F "$ARTIFACTS_DIR" || echo "Directory not found"
    exit 1
fi

echo "Found report: $LATEST_REPORT"

# Timestamp for history
TIMESTAMP=$(date +"%Y-%m-%d_%H-%M-%S")
HISTORY_FILE="$HISTORY_DIR/benchmark_$TIMESTAMP.json"

# Copy to history
cp "$LATEST_REPORT" "$HISTORY_FILE"
echo "Saved to history: $HISTORY_FILE"

# Find previous history file (excluding the one we just made)
PREV_HISTORY_FILE=$(ls -t "$HISTORY_DIR"/*.json | grep -v "$HISTORY_FILE" | head -n 1)

if [ -n "$PREV_HISTORY_FILE" ]; then
    echo "Comparing with previous run: $PREV_HISTORY_FILE"
    python3 "$SCRIPT_DIR/compare_benchmarks.py" "$PREV_HISTORY_FILE" "$HISTORY_FILE" > "$SCRIPT_DIR/latest_comparison.md"
    echo "Comparison report generated at $SCRIPT_DIR/latest_comparison.md"
    echo "---------------------------------------------------"
    cat "$SCRIPT_DIR/latest_comparison.md"
    echo "---------------------------------------------------"
else
    echo "No previous history to compare with. This is likely the first run."
fi

# Update last run commit
git rev-parse HEAD > "$COMMIT_FILE"
echo "Updated last run commit to $(cat "$COMMIT_FILE")"
