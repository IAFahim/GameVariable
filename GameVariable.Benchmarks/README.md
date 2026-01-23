# 🏎️ GameVariable.Benchmarks

**Proving the speed.** ⚡

This project contains the **BenchmarkDotNet** performance tests for the entire GameVariable ecosystem. We don't just say "zero allocation" — we prove it.

---

## 🏃 Running Benchmarks

### Prerequisites
*   .NET SDK 9.0+

### Run All Benchmarks
```bash
dotnet run -c Release --project GameVariable.Benchmarks
```

> **Note:** This takes a long time! (200+ benchmarks)

### Run Specific Benchmarks
Use the `--filter` argument to run specific tests.

```bash
# Run only RPG benchmarks
dotnet run -c Release --project GameVariable.Benchmarks -- --filter "*RPG*"

# Run only Math benchmarks
dotnet run -c Release --project GameVariable.Benchmarks -- --filter "*Math*"
```

### Fast Run (Dry Run)
If you just want to verify the benchmarks compile and run without waiting for statistical precision:

```bash
dotnet run -c Release --project GameVariable.Benchmarks -- --job short
```

---

## 📊 Interpreting Results

Look for the **Allocated** column.
*   `0 B` = **Perfect.** No Garbage Collection. ✅
*   `> 0 B` = Memory was allocated. (We try to avoid this!)

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
