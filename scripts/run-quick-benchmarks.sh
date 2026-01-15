#!/bin/bash

# Quick Benchmark Smoke Test
# Runs a minimal set of benchmarks to verify everything works

set -e

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}  GameVariable Quick Benchmark Test${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""

# Navigate to benchmark project
cd "$(dirname "$0")/.."
PROJECT_ROOT="$(pwd)"
BENCHMARK_PROJECT="$PROJECT_ROOT/GameVariable.Benchmarks"

echo -e "${YELLOW}Building benchmarks...${NC}"
dotnet build "$BENCHMARK_PROJECT/GameVariable.Benchmarks.csproj" --configuration Release --no-restore

echo ""
echo -e "${YELLOW}Running quick benchmarks (Dry job)...${NC}"
echo ""

cd "$BENCHMARK_PROJECT"

# Run one benchmark from each package as a smoke test
echo "Testing Core package..."
dotnet run -c Release -- -j Dry -i --filter "*CoreMath*Construction*"

echo ""
echo "Testing Timer package..."
dotnet run -c Release -- -j Dry -i --filter "*Timer*Construction*"

echo ""
echo "Testing Input package..."
dotnet run -c Release -- -j Dry -i --filter "*InputRingBuffer*Construction*"

echo ""
echo -e "${GREEN}✓ All benchmarks passed!${NC}"
echo ""
echo -e "${BLUE}To run full benchmarks, use:${NC}"
echo "  ./scripts/run-daily-benchmarks.sh"
