# 🏎️ GameVariable.Benchmarks

**The Wind Tunnel.** 💨

**GameVariable.Benchmarks** is the proving ground. It uses `BenchmarkDotNet` to prove that `GameVariable` structs are faster than standard OOP classes and allocated memory.

---

## 🧠 Mental Model: The Wind Tunnel

You don't just *guess* that a Ferrari is aerodynamic. You put it in a wind tunnel and measure it.

This project is our wind tunnel. It runs thousands of operations per second to measure:
1.  **Speed:** How fast is it? (ns)
2.  **Memory:** How much garbage does it create? (Bytes)

Spoiler: The memory is usually **0 Bytes**.

---

## 📦 Running Benchmarks

1.  **Install .NET SDK 9.0+**
2.  **Run the project:**

```bash
# Run everything (Takes a while!)
dotnet run -c Release

# Run specific tests (Filter)
dotnet run -c Release -- --filter *Bounded*
```

---

## 📊 What We Measure

*   **BoundedFloat vs float:** Is the wrapper overhead negligible? (Yes).
*   **Grid2D vs Array[,]**: Is flat indexing faster? (Yes).
*   **Intent State Machine:** Is it zero allocation? (Yes).
*   **RPG Stats:** How fast is the "Diamond Architecture"?

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
