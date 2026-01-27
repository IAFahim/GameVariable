# 💨 GameVariable.Benchmarks

**The Wind Tunnel.** 🌪️

**GameVariable.Benchmarks** is where we prove it.
We don't just *say* "Zero Allocation" — we prove it with millions of iterations per second.

---

## 🧠 The Mental Model: The Wind Tunnel

Imagine you're building a race car (your game code). You think it's fast, but is it *aerodynamic*?

You put it in a **Wind Tunnel** (this project) and blast it with air (data) at 500mph.
- If pieces fly off (Exceptions/Allocations) -> **Failed.** ❌
- If it drags (Slow execution) -> **Failed.** ❌
- If it cuts through the air like a knife -> **Passed.** ✅

This project is that wind tunnel. It verifies that `Variable.Core`, `Variable.Bounded`, and others are actually as fast as we claim.

---

## 🚀 How to Run

This is a Console Application. You run it from the terminal.

### 1. Basic Run (Run Everything)

**⚠️ IMPORTANT:** Always run in `Release` mode! Debug mode ruins performance tests.

```bash
cd GameVariable.Benchmarks
dotnet run -c Release
```

### 2. Filter Specific Benchmarks

Only want to test the Math?

```bash
dotnet run -c Release -- --filter *CoreMath*
```

---

## 📊 What We Measure

We use [BenchmarkDotNet](https://benchmarkdotnet.org/) to track:

1.  **Mean Time:** How fast is it? (ns/op)
2.  **Allocated Memory:** Did we trigger the Garbage Collector? (Should be **-** or **0 B**)
3.  **Gen 0/1/2:** Did we stress the GC generations?

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
