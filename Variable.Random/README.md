# 🎲 Variable.Random

**Deterministic, Zero-Allocation RNG for Games.** 🎰

**Variable.Random** provides high-performance, struct-based random number generation. It implements the standard **PCG-XSH-RR** algorithm, which is statistically superior to standard System.Random and perfectly suited for procedural generation, simulation, and ECS/Burst environments.

---

## 📦 Installation

```bash
dotnet add package Variable.Random
```

---

## 🚀 Features

* **⚡ Zero Allocation:** Pure `struct` design. No GC pressure.
* **🔒 Deterministic:** Same seed + same sequence = same output, every time.
* **🕹️ PCG Algorithm:** Fast, high-quality randomness (passes statistical tests better than LCG).
* **🧮 Burst Compatible:** Safe to use in Unity Jobs and Burst-compiled code.

---

## 🎮 Usage Guide

### 1. Basic Usage

```csharp
using Variable.Random;

// Create a generator with a specific seed
var rng = new PcgRandom(12345);

// Generate numbers
int damage = rng.NextInt(10, 20); // 10 to 19
float chance = rng.NextFloat();   // 0.0 to 1.0
bool crit = rng.NextBool();       // True or False
```

### 2. Time-Based Seed (For non-deterministic gameplay)

```csharp
// Seeds automatically from DateTime.Now
var rng = new PcgRandom();
```

### 3. Independent Streams (Sequences)

You can have multiple generators with the *same seed* but different *sequences* that produce completely different streams of numbers. This is great for having a "World Gen" stream and a "Loot Drop" stream that don't interfere with each other.

```csharp
ulong mapSeed = 9999;

// Stream 1: Terrain
var terrainRng = new PcgRandom(mapSeed, 1);

// Stream 2: Loot
var lootRng = new PcgRandom(mapSeed, 2);

// Even though they share the seed, their output is unique and independent!
```

---

## 🔧 API Reference

### `PcgRandom` Struct

#### Constructors
- `PcgRandom(ulong seed, ulong sequence = 0)`: Initialize with explicit seed and sequence.
- `PcgRandom()`: Initialize with time-based seed.

#### Methods
- `Next()`: Returns random `uint`.
- `Next(uint max)`: Returns random `uint` in `[0, max)`.
- `NextInt(int min, int max)`: Returns random `int` in `[min, max)`.
- `NextFloat()`: Returns random `float` in `[0.0, 1.0)`.
- `NextFloat(float min, float max)`: Returns random `float` in `[min, max)`.
- `NextBool()`: Returns random `bool`.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
