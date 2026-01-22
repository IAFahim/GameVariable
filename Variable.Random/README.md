# 🎲 Variable.Random

**Deterministic, Zero-Allocation RNG.** 🎰

**Variable.Random** gives you a fast, reliable random number generator that doesn't create garbage.
- **Pure Data:** Just a `struct`. No classes.
- **Deterministic:** Same seed = same sequence, guaranteed.
- **PCG Algorithm:** Better statistical quality than `System.Random`.

---

## 📦 Installation

```bash
dotnet add package Variable.Random
```

---

## 🚀 Features

* **⚡ Zero Allocation:** No `new` keywords in your loop.
* **🔒 Deterministic:** Perfect for procedurally generated worlds, replays, and networking.
* **🃏 Utilities:** `Shuffle`, `PickRandom`, `Chance` built-in.
* **🔢 Type Safe:** Distinct `Next`, `NextFloat`, `Range` methods.

---

## 🎮 Usage Guide

### 1. Basic Usage

```csharp
using Variable.Random;

// Initialize with a seed (e.g., Level 1 seed)
var rng = new PcgRandom(12345);

void Update()
{
    // Get a random float [0.0, 1.0)
    float val = rng.NextFloat();

    // Get a random int [1, 10)
    int damage = rng.Range(1, 10);

    // 50% chance
    if (rng.Chance(0.5f))
    {
        CriticalHit();
    }
}
```

### 2. Procedural Generation (Deterministic)

Because `PcgRandom` is a struct, you can pass it by reference to ensure the state updates.

```csharp
public void GenerateLevel(ulong levelSeed)
{
    var rng = new PcgRandom(levelSeed);

    // Always generates the same map for this seed
    var width = rng.Range(10, 20);
    var height = rng.Range(10, 20);

    GenerateTerrain(ref rng, width, height);
}

// Pass by 'ref' to update the RNG state!
private void GenerateTerrain(ref PcgRandom rng, int w, int h)
{
    if (rng.Chance(0.1f)) SpawnLake();
}
```

### 3. Shuffling Lists

```csharp
var cards = new List<string> { "Ace", "King", "Queen", "Jack" };
var rng = PcgRandom.New(); // Random seed

// Zero-allocation Fisher-Yates shuffle
rng.Shuffle(cards);

// Pick one
string chosen = rng.PickRandom(cards);
```

---

## 🔧 API Reference

### `PcgRandom`
- `State`: The internal 64-bit state.
- `Increment`: The internal 64-bit stream selector.

### Extensions
- `Next()`: Returns `uint`.
- `NextFloat()`: Returns `float` [0, 1).
- `Range(min, max)`: Returns `int` [min, max) or `float` [min, max].
- `Chance(probability)`: Returns `bool`.
- `Shuffle(list)`: Randomizes a list in-place.
- `PickRandom(list)`: Returns a random item.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
