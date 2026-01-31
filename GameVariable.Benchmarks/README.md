# 🏎️ GameVariable.Benchmarks

**The Wind Tunnel for your Code.** 🌬️

**GameVariable.Benchmarks** is the proving ground where we ensure every nanosecond counts. It uses `BenchmarkDotNet` to verify that our zero-allocation promises are actually kept.

---

## 🧠 The Mental Model: The Wind Tunnel

Imagine building a race car. You don't just guess if it's aerodynamic; you put it in a **Wind Tunnel**.
- **GameVariable:** The Race Car.
- **Benchmarks:** The Wind Tunnel.
- **You:** The Engineer checking the data.

If a method allocates memory when it shouldn't, the alarm goes off here.

---

## 🏃 Running Benchmarks

### 1. Run Everything (Grab a coffee ☕)

```bash
# Must run in Release mode!
dotnet run -c Release
```

### 2. Run Specific Benchmarks (Quick Check ⚡)

Use the filter (`-f`) argument:

```bash
# Run only BoundedFloat benchmarks
dotnet run -c Release -- -f *BoundedFloat*
```

### 3. Fast Run (Dev Mode)

Use the short run job (`-j short`) for quick iterations (less precise):

```bash
dotnet run -c Release -- -f *Timer* --job short
```

---

## 📊 What We Measure

| Metric | Why it matters | Goal |
|--------|----------------|------|
| **Mean Time** | How long it takes to run. | < 5ns for simple ops |
| **Allocated** | Garbage created. | **0 B (Zero!)** |

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
