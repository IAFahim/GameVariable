# 📊 Variable.Bounded

**The "Cup" that never spills.** ☕

**Variable.Bounded** gives you specialized numbers (floats, ints, bytes) that *cannot* go outside their limits. It is the bedrock of robust game state.

---

## 🧠 Mental Model

Think of a **Cup**. 🥤
- It has a bottom (`Min`).
- It has a rim (`Max`).
- It has liquid inside (`Current`).

If you pour too much water (add value), it just spills over the rim—the cup stays full.
If you try to drink more than is there (subtract value), you just suck air—the cup stays empty.
**The cup never explodes.**

Use `Variable.Bounded` for anything that behaves like a cup: Health, Mana, Stamina, Ammo, Batteries.

---

## 👶 ELI5 (Explain Like I'm 5)

In normal coding:
> **You:** "Subtract 1000 health!"
> **Computer:** "Okay! Health is now -900. Your character is zombie." 🧟‍♂️

In Variable.Bounded:
> **You:** "Subtract 1000 health!"
> **Computer:** "I can only subtract 100. Health is now 0. You are dead." 💀

It handles the math so you don't have to write `if` statements everywhere.

---

## 📦 Installation

```bash
dotnet add package Variable.Bounded
```

---

## 📚 The Types

| Type | Size | Best For... |
|------|------|-------------|
| **`BoundedFloat`** | 12 bytes | **Health, Mana, Stamina.** Anything smooth or regenerating. |
| **`BoundedInt`** | 12 bytes | **Ammo, Inventory Slots, Skill Points.** Things you count 1-by-1. |
| **`BoundedByte`** | 2 bytes | **Grid Coordinates, Small Stacks.** Tiny numbers (0-255). |

---

## 🎮 Usage Guide

### 1. Health & Mana (The Classics)
Most stats go from 0 to Max.

```csharp
// Max HP: 100. Starts full.
var health = new BoundedFloat(100f);

// Take damage (clamped to 0)
health -= 45f;

// Heal (clamped to 100)
health += 2000f; // Won't exceed 100!

if (health.IsEmpty())
    Debug.Log("Wasted.");
```

### 2. Temperature (Negative Ranges) ❄️🔥
Not everything starts at 0!

```csharp
// Range: -50 to +50. Starts at 20.
var temp = new BoundedFloat(50f, -50f, 20f);

// Getting colder...
temp -= 100f; // Clamps to -50f (Absolute minimum)
```

### 3. Ammo Clips (Integer Logic) 🔫
Integers work exactly the same way.

```csharp
var ammo = new BoundedInt(30); // 30 rounds

if (!ammo.IsEmpty()) {
    ammo--;
    FireGun();
}
```

---

## ⚡ Performance Secrets

### Zero Allocation
These are `structs`. Creating them allocates **0 bytes** of garbage. You can use millions of them in Unity ECS or tight loops without triggering the Garbage Collector.

### Mathematical Operators
You can use `+`, `-`, `*`, `/`, `++`, `--` directly.
```csharp
health += 10f; // Clean syntax, safe logic.
```

---

## ⚠️ Important: The "Public Field" Trap

We expose fields (`.Current`, `.Min`, `.Max`) for **maximum performance** (Unity Burst Compiler loves this). But with great power comes great responsibility.

### ❌ The "Ooops" Way
```csharp
// DIRECT MODIFICATION BYPASSES LOGIC!
health.Current = -9999f;
// Now your health is -9999. The struct didn't know you touched it.
```

### ✅ The Safe Way (Extensions)
```csharp
// Use methods to ensure safety
health.Set(9999f); // Correct! Sets to Max.
```

### ✅ The "Pro" Way (Manual Normalization)
If you *must* edit fields directly (e.g., inside a Burst Job), you must fix it yourself.
```csharp
health.Current += heavyCalculation;
health.Normalize(); // Snaps value back within Min/Max
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
