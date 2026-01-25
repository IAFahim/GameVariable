# 🏁 GameVariable.Benchmarks

**The Proving Grounds.** 🏎️💨

**GameVariable.Benchmarks** is the performance lab for the entire ecosystem. It ensures that "Zero Allocation" isn't just a marketing buzzword—it's a measurable fact.

---

## 🧠 The Mental Model

Think of this project as a **Wind Tunnel**.
We put our structs (`BoundedFloat`, `Timer`, `IntentState`) inside, blast them with millions of operations per second, and measure:
1.  **Speed:** How fast is it compared to raw C#?
2.  **Drag:** How much memory does it allocate? (Goal: 0 Bytes)

---

## 🚀 How to Run

Benchmarks must be run in **Release** mode to be valid.

### 1. Run Everything (Go get a coffee ☕)
This will take a long time as it runs every benchmark in the suite.

```bash
dotnet run -c Release
```

### 2. Run Specific Benchmarks (Recommended)
Use filters to focus on specific systems.

```bash
# Test only the Math core
dotnet run -c Release -- --filter *CoreMath*

# Test Intent State Machine performance
dotnet run -c Release -- --filter *Intent*

# Test Bounded types (Float/Int)
dotnet run -c Release -- --filter *Bounded*
```

### 3. Quick Dry Run
If you just want to verify the code works without waiting for statistical accuracy:

```bash
dotnet run -c Release -- --job short
```

---

## 📊 What We Measure

We use **BenchmarkDotNet** to track:

*   **Mean Execution Time:** How long the operation takes (in nanoseconds).
*   **Allocated Memory:** Heap allocations per operation (Should be `-` or `0 B`).
*   **Gen 0/1/2:** Garbage Collection frequency.

### Example Output

```
| Method      | Mean     | Error    | StdDev   | Gen0 | Allocated |
|-------------|---------:|---------:|---------:|-----:|----------:|
| RawFloatOps | 0.045 ns | 0.003 ns | 0.003 ns |    - |         - |
| BoundedOps  | 0.048 ns | 0.005 ns | 0.004 ns |    - |         - |
```

*If `BoundedOps` is close to `RawFloatOps` and `Allocated` is empty, we are winning.* 🎉

---

## 📁 Key Benchmark Categories

| Class | What it tests |
|-------|---------------|
| `CoreMathBenchmarks` | Low-level math (`Clamp`, `Lerp`) vs `Math.Clamp`. |
| `BoundedFloatBenchmarks` | The cost of safety checks in `BoundedFloat`. |
| `TimerBenchmarks` | `Timer` and `Cooldown` tick logic. |
| `IntentBenchmarks` | State Machine transition overhead. |
| `RpgStatBenchmarks` | Diamond architecture and modifier calculations. |

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
