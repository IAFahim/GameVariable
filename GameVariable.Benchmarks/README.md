# 💨 GameVariable.Benchmarks

**The Wind Tunnel for your Code.** 🏎️

**GameVariable.Benchmarks** is the proving ground where we ensure every struct is zero-allocation and blazingly fast. It uses [BenchmarkDotNet](https://benchmarkdotnet.org/) to measure performance down to the nanosecond.

---

## 🏃 How to Run

### 1. The Standard Way (Release Mode!)

**Crucial:** Always run benchmarks in `Release` mode to avoid debug overhead and allocation warnings.

```bash
dotnet run -c Release
```

### 2. Running Specific Benchmarks

You can filter which benchmarks to run using arguments.

```bash
# Run only Bounded benchmarks
dotnet run -c Release -- --filter *Bounded*

# Run only CoreMath benchmarks
dotnet run -c Release -- --filter *CoreMath*
```

### 3. Fast Mode (Short Run)

The full suite can take a while. Use the `short` job to get quick feedback (warning: less precise).

```bash
dotnet run -c Release -- --job short
```

*Note: Some micro-benchmarks might report `0.00 ns` in Short Run mode due to JIT optimizations removing "dead code" in low-iteration loops. The full run handles this better.*

---

## 📜 Automation Scripts

We use scripts to track performance over time.

### `scripts/run-daily-benchmarks.sh`

This script is used by our CI/CD to:
1. Check if relevant code changed.
2. Run benchmarks.
3. Compare results against the previous run.
4. Generate a report in `scripts/latest_comparison.md`.

---

## 📊 interpreting Results

| Method | Mean | Error | StdDev | Gen0 | Allocation |
|--------|------|-------|--------|------|------------|
| Tick   | 0.1ns| 0.01 | 0.01 | -    | -          |

- **Mean**: Average time taken.
- **Allocation**: Should be **"-"** (Zero). If you see bytes here, we have a problem!

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
