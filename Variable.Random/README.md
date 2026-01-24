# 🎲 Variable.Random

**Deterministic, Zero-Allocation RNG for Games.**

`Variable.Random` provides a lightweight `RngState` struct that implements a fast Xorshift algorithm. It is fully deterministic, thread-safe (stack-allocated), and garbage-collection free.

## 🚀 Quick Start

```csharp
using Variable.Random;

// 1. Create a seeded state
var rng = new RngState(12345);

// 2. Generate numbers
int value = rng.Next();         // Raw uint cast to int
float chance = rng.NextFloat(); // 0.0 to 1.0
int damage = rng.Range(10, 20); // 10 to 20

// 3. Determinism
var rng2 = new RngState(12345);
// rng.Next() == rng2.Next() is ALWAYS true.
```

## 📦 Features

*   **Zero Allocation:** It's a `struct`. No class overhead.
*   **Deterministic:** Same seed always produces the same sequence.
*   **Fast:** Uses Xorshift32 (very low CPU cost).
*   **Safe:** Impossible to accidentally share state across threads (it's a value type).
