# 🏎️ GameVariable.Benchmarks

**The Wind Tunnel.** 💨

**GameVariable.Benchmarks** verifies the zero-allocation and high-performance claims of the entire GameVariable ecosystem.

---

## 🚀 How to Run

To run the full suite of benchmarks:

```bash
dotnet run -c Release
```

> **Note:** Always run in `Release` mode to avoid allocation warnings and ensure accurate timing!

### 🎯 Run Specific Benchmarks

You can filter which benchmarks to run using the `--filter` argument (or just passing arguments directly):

```bash
# Run only Intent benchmarks
dotnet run -c Release -- --filter "*Intent*"

# Run only Core Math benchmarks
dotnet run -c Release -- --filter "*CoreMath*"
```

### ⚡ Short Run (Fast Check)

For a quick sanity check (not for publishing results):

```bash
dotnet run -c Release -- --job short
```

---

## 📊 Reports

After running, results are generated in:
- `BenchmarkDotNet.Artifacts/results/` (HTML, Markdown, CSV)
- `scripts/latest_comparison.md` (if using the daily script)

---

## 🧪 What We Measure

We benchmark everything to ensure **0 Bytes Allocated** per frame:
- **Bounded**: Float/Int clamping and arithmetic.
- **Timer**: Tick logic and state checks.
- **Experience**: Level-up loops and overflow handling.
- **Intent**: State machine transitions.
- **RPG**: Damage pipeline resolution (Diamond Architecture).

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
