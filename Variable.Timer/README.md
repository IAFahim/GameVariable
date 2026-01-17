# ⏱️ Variable.Timer

**Tick Tock.** Countdowns, Cooldowns, and Stopwatches.

`Variable.Timer` provides two essential structs: `Timer` (counts UP from 0) and `Cooldown` (counts DOWN from Duration). They replace messy float variables (`float timer = 0f;`) with a robust, semantic API that handles completion states, looping, and resetting.

[![NuGet](https://img.shields.io/nuget/v/Variable.Timer?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Timer)

## 📦 Installation

```bash
dotnet add package Variable.Timer
```

## 🔥 Features

* **⏳ Semantic**: `timer.IsFull()` is clearer than `timer >= duration`.
* **🔄 Cooldowns**: Specialized struct for abilities that need to recharge.
* **🛡️ Safe**: Auto-clamps values. No negative time.
* **⚡ Zero Allocation**: Lightweight structs.

## 🛠️ Usage Guide

### 1. `Timer` (Counts UP 📈)
Use this for duration-based events, like "Cast time" or "Fuse length".

```csharp
using Variable.Timer;

// Create a 5-second timer
var bombFuse = new Timer(5f);

// In Update:
bombFuse.Tick(Time.deltaTime);

// Check status
if (bombFuse.IsFull())
{
    Explode();
}

// Get progress (0.0 to 1.0)
uiSlider.value = bombFuse.GetRatio();
```

### 2. `Cooldown` (Counts DOWN 📉)
Use this for abilities, dashes, or weapon fire rates. It starts "Empty" (0) meaning "Ready".

```csharp
// Create a 2-second cooldown
// Note: Cooldown logic is inverted.
// 0 = Ready. Duration = On Cooldown.
var dashCd = new Cooldown(2f);

// Try to dash
if (dashCd.IsReady()) // Checks if Current <= 0
{
    Dash();
    dashCd.Reset(); // Sets Current = Duration (2s)
}

// In Update:
dashCd.Tick(Time.deltaTime); // Reduces Current by deltaTime
```

### 3. Looping Timers
Great for spawners.

```csharp
var spawner = new Timer(3f);

spawner.Tick(dt);

if (spawner.IsFull())
{
    SpawnEnemy();
    spawner.Reset(); // Back to 0
}
```

### 4. Advanced: Modifying Time
You can add or subtract time directly.

```csharp
// "Time Extension" powerup
timer += 5f;

// "Cooldown Reduction" buff
cooldown -= 1f;
```

## ❓ Timer vs Cooldown?

| Feature | `Timer` | `Cooldown` |
|---------|---------|------------|
| Direction | Counts **UP** (0 → Max) | Counts **DOWN** (Max → 0) |
| Ready Condition | `IsFull()` | `IsReady()` / `IsEmpty()` |
| Start Value | Usually 0 | Usually 0 (Ready) |
| Reset Action | Sets to 0 | Sets to Max |
| Use Case | Cast bars, Fuses, Durations | Spells, Dashes, Reloads |

## 🤝 Contributing
Found a bug? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
