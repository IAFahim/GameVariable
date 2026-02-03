# 💨 GameVariable.Benchmarks

**The Wind Tunnel.** 🏎️

**GameVariable.Benchmarks** is where we prove our claims. We don't just *say* it's fast; we measure it down to the nanosecond.

---

## 🧠 Mental Model: The Wind Tunnel 🌪️

You don't put a car on the race track until you've tested it in the **Wind Tunnel**.
*   Does it drag? (Allocations)
*   Is it fast? (Throughput)
*   Does it overheat? (GC Pressure)

This project is the proving ground for every struct in the ecosystem.

---

## 🚀 How to Run

We use [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). It takes a while, but it's thorough.

### 1. Run Everything (The "I have time" mode)

```bash
cd GameVariable.Benchmarks
dotnet run -c Release
```

### 2. Run Specific Tests

You can filter by name.

```bash
# Only run BoundedFloat tests
dotnet run -c Release --filter *BoundedFloat*

# Only run Timer tests
dotnet run -c Release --filter *Timer*
```

---

## 📊 What We Measure

1.  **Mean Time:** How fast is it? (Usually in **nanoseconds**).
2.  **Allocated:** How much memory did it create? (Target: **0 B**).
3.  **Gen 0/1/2:** Did we trigger the Garbage Collector? (Target: **-**).

---

## 🏆 Current Results (Preview)

*Actual results depend on your machine.*

| Method | Mean | Allocated |
|--------|------|-----------|
| `BoundedFloat.Op_Add` | ~0.5 ns | **0 B** |
| `Timer.Tick` | ~1.2 ns | **0 B** |
| `Grid2D.GetRow` | ~0.1 ns | **0 B** |

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
