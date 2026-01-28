# 🌬️ GameVariable.Benchmarks

**The Wind Tunnel.** 🏎️💨

This is where we prove our claims. **GameVariable.Benchmarks** is the dedicated testing ground for measuring the performance, memory allocation, and instruction count of every struct in the ecosystem.

---

## 🧠 The Mental Model

Think of this project as a **Wind Tunnel** for code.

Before we put a car on the track (your game), we put it in the wind tunnel to see if it's aerodynamic.
Before we ship a struct (like `BoundedFloat`), we put it here to ensure it causes **zero allocations** and runs as fast as raw math.

---

## 👶 ELI5

**"How fast is it?"**
This project answers that question. It runs the code millions of times and counts how long it takes (down to the nanosecond!).

If `Variable.Bounded` says it's "Zero Allocation", this is the lie detector test that proves it.

---

## 🏃 Usage Guide

### 1. Run All Benchmarks

**⚠️ Important:** Always run in `Release` configuration to get accurate results! Debug builds include overhead that skews the data.

```bash
dotnet run -c Release
```

### 2. Run Specific Benchmarks (Filters)

You can filter by name to save time (the full suite takes a while!).

```bash
# Run only Bounded benchmarks
dotnet run -c Release -- --filter *Bounded*

# Run only Timer benchmarks
dotnet run -c Release -- --filter *Timer*
```

### 3. Fast Mode (Short Run)

If you just want a quick sanity check and don't need publication-quality data:

```bash
dotnet run -c Release -- --job short
```

---

## 📊 Understanding Results

### "Why is the result 0.00 ns?" 🤯

If you see `Mean: 0.00 ns`, it usually means the code is **too fast**.
The JIT (Just-In-Time) compiler is so smart that it realized the method calculation wasn't being used, so it deleted the code entirely (Dead Code Elimination).

This is actually a **good sign**! It means the struct overhead is effectively zero. To measure the actual math cost, we often have to trick the compiler by consuming the result.

### "Allocated: - "

This means **Zero Allocation**. No garbage. No GC spikes. Pure speed.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
