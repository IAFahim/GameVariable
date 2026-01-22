# 🎲 Variable.Random

**Zero-Allocation, Deterministic Random Number Generator.**

Part of the **[GameVariable](https://github.com/iafahim/GameVariable)** ecosystem.

## ✨ Features
*   **Zero Allocation:** `struct`-based state. No `new System.Random()` garbage.
*   **Deterministic:** Same seed = Same results. Perfect for replays and procedural generation.
*   **Fast:** Uses **Xoshiro128\*\*** algorithm (standard for high-performance games).
*   **Unified API:** `NextInt`, `NextFloat`, `NextBool`.

## 🚀 Quick Start

```csharp
using Variable.Random;

// 1. Initialize
var rng = new RandomState(123456); // Seed

// 2. Use it!
rng.NextInt(0, 10);    // 0 to 9
rng.NextFloat();       // 0.0 to 1.0
rng.NextBool();        // true/false
```
