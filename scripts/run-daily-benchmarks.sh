#!/bin/bash

# Daily Benchmark Runner for GameVariable
# This script runs all benchmarks and generates a markdown report

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"
BENCHMARK_PROJECT="$PROJECT_ROOT/GameVariable.Benchmarks"
REPORT_FILE="$PROJECT_ROOT/scripts/BENCHMARK_REPORT.md"
HISTORY_DIR="$PROJECT_ROOT/scripts/benchmark_history/latest"
TIMESTAMP=$(date -u +"%Y-%m-%dT%H:%M:%SZ")

mkdir -p "$HISTORY_DIR"

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}  GameVariable Benchmark Runner${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""
echo -e "Timestamp: ${GREEN}$TIMESTAMP${NC}"
echo ""

# Navigate to project root
cd "$PROJECT_ROOT"

# Step 1: Clean previous results
echo -e "${YELLOW}[1/5] Cleaning previous benchmark results...${NC}"
rm -rf "$BENCHMARK_PROJECT/BenchmarkDotNet.Artifacts"
echo -e "${GREEN}✓ Cleaned${NC}"
echo ""

# Step 2: Restore dependencies
echo -e "${YELLOW}[2/5] Restoring dependencies...${NC}"
dotnet restore "$BENCHMARK_PROJECT/GameVariable.Benchmarks.csproj"
echo -e "${GREEN}✓ Restored${NC}"
echo ""

# Step 3: Build benchmark project
echo -e "${YELLOW}[3/5] Building benchmark project...${NC}"
dotnet build "$BENCHMARK_PROJECT/GameVariable.Benchmarks.csproj" --configuration Release --no-restore
echo -e "${GREEN}✓ Built${NC}"
echo ""

# Step 4: Run benchmarks
echo -e "${YELLOW}[4/5] Running benchmarks...${NC}"
echo -e "${BLUE}This may take several minutes...${NC}"
echo ""

cd "$BENCHMARK_PROJECT"

# Run with GitHub exporter for nice markdown output and Json for data processing
dotnet run -c Release -- --filter "*" --exporters GitHub Json --iterationTime 500

echo ""
echo -e "${GREEN}✓ Benchmarks complete${NC}"
echo ""

# Step 5: Generate report
echo -e "${YELLOW}[5/5] Generating report...${NC}"

# Create the report header
cat > "$REPORT_FILE" << EOF
# GameVariable Benchmark Report

**Generated:** $TIMESTAMP
**Commit:** $(git rev-parse --short HEAD)
**Branch:** $(git rev-parse --abbrev-ref HEAD)

---

EOF

# Section: Comparison
echo "## Benchmark Comparisons" >> "$REPORT_FILE"
echo "" >> "$REPORT_FILE"

CHANGES_DETECTED=false

if [ -d "BenchmarkDotNet.Artifacts/results" ]; then
    # Process JSON files for comparison
    find BenchmarkDotNet.Artifacts/results -name "*.json" -type f | while read -r current_file; do
        filename=$(basename "$current_file")
        previous_file="$HISTORY_DIR/$filename"

        if [ -f "$previous_file" ]; then
            CHANGES_DETECTED=true
            echo "Processing comparison for $filename..."
            python3 "$PROJECT_ROOT/scripts/compare_benchmarks.py" "$previous_file" "$current_file" "temp_diff.md"
            cat "temp_diff.md" >> "$REPORT_FILE"
            echo "" >> "$REPORT_FILE"
            rm "temp_diff.md"
        else
            echo "No previous history for $filename"
        fi

        # Update history
        cp "$current_file" "$previous_file"
    done
fi

if [ "$CHANGES_DETECTED" = false ]; then
    echo "No previous benchmark data found for comparison." >> "$REPORT_FILE"
fi

echo "" >> "$REPORT_FILE"
echo "---" >> "$REPORT_FILE"

# Section: Full Results
echo "## Full Benchmark Results" >> "$REPORT_FILE"
echo "" >> "$REPORT_FILE"

# Append benchmark results if available
if [ -d "BenchmarkDotNet.Artifacts/results" ]; then
    # Find and append all markdown files
    find BenchmarkDotNet.Artifacts/results -name "*.md" -type f | while read -r file; do
        echo "" >> "$REPORT_FILE"
        cat "$file" >> "$REPORT_FILE"
    done
    echo -e "${GREEN}✓ Report generated: $REPORT_FILE${NC}"
else
    echo "No benchmark results found." >> "$REPORT_FILE"
    echo -e "${RED}✗ No results found${NC}"
fi

echo ""
echo -e "${BLUE}========================================${NC}"
echo -e "${GREEN}  Benchmark Run Complete!${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""
echo -e "Report saved to: ${GREEN}$REPORT_FILE${NC}"
echo -e "Artifacts: ${BLUE}$BENCHMARK_PROJECT/BenchmarkDotNet.Artifacts${NC}"
echo ""

# Optional: Display summary stats
if [ -f "$REPORT_FILE" ]; then
    echo -e "${YELLOW}Quick Summary:${NC}"
    grep -E "^\|" "$REPORT_FILE" | head -20 || true
fi
