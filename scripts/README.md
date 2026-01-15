# GameVariable Scripts

This directory contains automation scripts for GameVariable development and benchmarking.

## Benchmark Scripts

### run-daily-benchmarks.sh

Runs all GameVariable benchmarks and generates a markdown report.

**Note:** Despite the name, this script is typically run manually or via GitHub Actions on code changes, not on a daily schedule.

**Usage:**

```bash
# From project root
./scripts/run-daily-benchmarks.sh

# Or from scripts directory
cd scripts
./run-daily-benchmarks.sh
```

**What it does:**

1. Cleans previous benchmark results
2. Restores NuGet dependencies
3. builds the benchmark project in Release configuration
4. Runs all benchmarks with GitHub exporter
5. Generates a markdown report at `scripts/BENCHMARK_REPORT.md`

**Requirements:**

- .NET 9.0 SDK
- Bash shell (Linux/macOS/WSL)

**Output:**

- Benchmark report: `scripts/BENCHMARK_REPORT.md`
- Detailed artifacts: `GameVariable.Benchmarks/BenchmarkDotNet.Artifacts/`

### Running Individual Benchmarks

You can also run benchmarks directly:

```bash
# Run all benchmarks
cd GameVariable.Benchmarks
dotnet run -c Release -- -j short -i -m

# Run specific package benchmarks
dotnet run -c Release -- -j short -i -m --filter "*Timer*"
dotnet run -c Release -- -j short -i -m --filter "*Input*"

# Run with specific configuration
dotnet run -c Release -- -j short -i -m --exporters GitHub

# Run single iteration (smoke test)
dotnet run -c Release -- -j Dry -i
```

### Benchmark Options

- `-j <job>`: Job configuration (Short/Medium/Long/Dry)
- `-i`: Run in-process (faster)
- `-m`: Enable memory diagnostics
- `--filter <pattern>`: Filter benchmarks by name
- `--exporters <type>`: Export results (GitHub/CSV/JSON/HTML)

## GitHub Actions

### Benchmarks Workflow (`.github/workflows/benchmarks.yml`)

Automatically runs benchmarks on:
- Push to `master`/`main` branch
- Pull requests (compares against base branch)
- Manual trigger via workflow_dispatch (click "Run workflow" button in Actions tab)

**Artifacts:**

- `BenchmarkResults-<run-number>`: Complete benchmark results (30-day retention)
- `BenchmarkReport`: Formatted markdown report (90-day retention, manual runs only)

### CI Workflow (`.github/workflows/ci.yml`)

Runs on every push and PR:
- Build and test all projects
- Quick benchmark smoke test
- Code quality checks

## Local Development

### Quick Benchmark Test

```bash
# Run a quick smoke test (single iteration)
cd GameVariable.Benchmarks
dotnet run -c Release -- -j Dry -i --filter "*CoreMath*"
```

### Full Benchmark Run

```bash
# Run with default configuration (faster)
./scripts/run-daily-benchmarks.sh

# Or run manually with custom options
cd GameVariable.Benchmarks
dotnet run -c Release -- -j short -i -m --exporters JSON
```

### Comparing Benchmarks

To compare performance between commits:

```bash
# Run on base commit
git checkout main
./scripts/run-daily-benchmarks.sh
cp scripts/BENCHMARK_REPORT.md /tmp/before.md

# Run on your branch
git checkout feature-branch
./scripts/run-daily-benchmarks.sh
cp scripts/BENCHMARK_REPORT.md /tmp/after.md

# Compare
diff /tmp/before.md /tmp/after.md
```

## Interpreting Results

### Key Metrics

- **Mean**: Average execution time
- **StdDev**: Standard deviation (lower = more consistent)
- **Allocated**: Memory allocated per operation
- **Gen 0/1/2**: Garbage collections

### What to Look For

1. **Allocations**: Should be zero for most struct operations
2. **Consistency**: Low standard deviation indicates stable performance
3. **Scaling**: Linear performance for batch operations

## Troubleshooting

### Benchmarks Fail to Run

```bash
# Clean and rebuild
cd GameVariable.Benchmarks
dotnet clean
dotnet restore
dotnet build -c Release
```

### Out of Memory

Run fewer benchmarks at once:

```bash
dotnet run -c Release -- --filter "*Timer*" -j short
```

### Inconsistent Results

1. Close other applications
2. Use in-process mode (`-i`)
3. Increase iteration count for stability:

```bash
dotnet run -c Release -- --iterationCount 20 --warmupCount 5
```

## Contributing

When adding new benchmarks:

1. Add them to the appropriate package directory in `GameVariable.Benchmarks/`
2. Follow naming convention: `<TypeName>Benchmarks.cs`
3. Use `[MemoryDiagnoser]` and `[ShortRunJob]` attributes
4. Update this README if adding new script functionality
