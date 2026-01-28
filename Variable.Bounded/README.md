# 📊 Variable.Bounded

**Health bars, Ammo, Stamina, Mana... Solved.** ✅

**Variable.Bounded** gives you zero-allocation structs that *know their limits*. No more manually checking `if (health < 0) health = 0;` spread across 50 different scripts.

---

## 📦 Installation

```bash
dotnet add package Variable.Bounded
```

---

## 🧠 The Mental Model

| Type | Metaphor | Explanation |
|------|----------|-------------|
| **BoundedInt** | **The Cup** ☕ | It has a rim. You can pour coffee in, but once it hits the rim (Max), any extra just spills on the floor. It never gets "fuller" than full. |
| **BoundedFloat** | **The Gauge** 🌡️ | Like a thermometer or speedometer. There is a physical stopper at the top and bottom. The needle literally *cannot* go past the red line. |

## 👶 ELI5

**"The Magic Bucket."** 🪣

Imagine a bucket that can hold exactly 10 liters.
*   If you try to pour **100 liters** into it, the bucket doesn't explode. It just fills to 10 and stops.
*   If you try to take **20 liters** out, it just becomes empty (0). It never goes into "negative water".

You don't have to be careful when pouring. The bucket handles the mess for you.

---

## 🚀 Features

* **🛡️ Bulletproof Clamping:** Values *cannot* escape their bounds.
* **⚡ Zero Allocation:** Pure `struct` design. No GC pressure. Burst compatible.
* **➕ Natural Math:** Use `+`, `-`, `++`, `--` just like normal numbers.
* **📏 Standardized:** Implements `IBoundedInfo` for easy UI integration.

---

## 📚 Types Available

| Type | Use Case | Size |
|------|----------|------|
| `BoundedFloat` | Health, Mana, Stamina, Temperature | 12 bytes |
| `BoundedInt` | Ammo count, Inventory slots, Skill points | 12 bytes |
| `BoundedByte` | Small counts (0-255), Grid coordinates | 2 bytes |

---

## 🎮 Usage Guide

### 1. The Basics (Health System)

```csharp
using Variable.Bounded;

// Create health: 0 to 100
var health = new BoundedFloat(100f);

// Take damage
health -= 25f;  // Automatically clamps! No if-checks needed.

// Check status
if (health.IsEmpty())
{
    Die();
}

// Heal
health += 50f;  // Clamps to 100. Can't over-heal.

// UI
float fillAmount = (float)health.GetRatio(); // 0.0 to 1.0
```

### 2. Advanced Ranges (Temperature)

Not everything starts at 0!

```csharp
// Range: -50 to +50. Starting at 20.
var temperature = new BoundedFloat(50f, -50f, 20f);

temperature -= 100f; // Clamps to -50f (Min)
```

### 3. Ammo Clips (Integers)

```csharp
var ammo = new BoundedInt(12); // Max 12, Current 12

// Fire!
if (!ammo.IsEmpty())
{
    ammo--;
    FireBullet();
}
```

### 4. Tiny Values (Bytes)

Perfect for tile grids or small inventory stacks.

```csharp
// Max 10 items
var stack = new BoundedByte(10);
stack += 5; // Clamped to 10
```

---

## ⚠️ Important: The "Public Field" Trap

For maximum performance (Unity ECS/Burst), we expose public fields. This is great for speed, but requires care.

### ❌ The Wrong Way
```csharp
health.Current = 9999f; // 😱 OH NO!
// You just bypassed the limits! The struct doesn't know you changed this.
```

### ✅ The Right Way (Extension Methods)
```csharp
health.Set(9999f); // Correct! Will clamp to Max.
```

### ✅ The "I Know What I'm Doing" Way
If you *must* modify fields directly (e.g. inside a Job), you **must** normalize afterwards:
```csharp
health.Current += calculation;
health.Normalize(); // Snaps value back to bounds
```

---

## 🔧 API Reference

### Properties
- `Current`: The raw value.
- `Min`: The floor (0 for Byte).
- `Max`: The ceiling.

### Operations
- `+`, `-`, `*`, `/`: Standard math (result is always clamped).
- `++`, `--`: Increment/Decrement by 1.
- `implicit operator`: Treat `BoundedFloat` as `float` for comparisons (`if (hp > 50)`).

### Extensions
- `IsFull()`: Is `Current == Max`?
- `IsEmpty()`: Is `Current == Min`?
- `GetRatio()`: Percentage (0.0 to 1.0).
- `GetRange()`: Size of the range (`Max - Min`).
- `TryConsume(amount)`: Tries to subtract. Returns `false` if not enough.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
