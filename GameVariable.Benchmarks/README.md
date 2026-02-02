# 💨 GameVariable.Benchmarks

**The Wind Tunnel for your Code.** 🏎️

**GameVariable.Benchmarks** is the proving ground. It ensures that "Zero Allocation" isn't just a marketing slogan—it's a measured fact.

---

## 🧠 Mental Model: "The Wind Tunnel"

Before a Formula 1 car hits the track, it goes into a wind tunnel to test aerodynamics.
This project is our wind tunnel. We blast the code with millions of operations to see if it allocates memory (drag) or runs slow (friction).

## 👶 ELI5: "The Stopwatch"

We use a super-precise stopwatch to see how fast the code runs. We also check if the code leaves any trash behind (Garbage Collection). If it leaves trash, we fail it.

---

## 🏃 How to Run

**⚠️ CRITICAL:** Always run benchmarks in **Release** mode! Debug mode is slow and misleading.

```bash
# Run all benchmarks (Warning: Takes a long time!)
dotnet run -c Release --project GameVariable.Benchmarks

# Run specific benchmarks (Recommended)
dotnet run -c Release --project GameVariable.Benchmarks --filter "*Bounded*"
```

### Common Filters
- `*Bounded*` - Test BoundedFloat/Int/Byte
- `*Timer*` - Test Timer/Cooldown
- `*Experience*` - Test Leveling logic
- `*Grid*` - Test Grid2D performance

---

## 📊 Interpreting Results

You will see a table like this:

| Method | Mean | Error | StdDev | Gen0 | Allocated |
|------- |-----:|------:|-------:|-----:|----------:|
| Tick   | 2 ns | 0.1 ns| 0.05 ns|    - |         - |

- **Mean:** Average time taken (lower is better). `ns` = nanoseconds (1 billionth of a second).
- **Allocated:** Memory created per run. **MUST BE "- (0 B)"** for most operations. If you see bytes here, we have a problem.

---

## 🤖 Automation

We have scripts that run these automatically!
- **`scripts/run-daily-benchmarks.sh`**: Runs the suite and compares against the last run.
- **`scripts/compare_benchmarks.py`**: Generates a markdown report of the differences.

Artifacts are saved in `scripts/benchmark_history/`.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
