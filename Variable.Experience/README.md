# ⭐ Variable.Experience

**Ding! Level Up!** The definitive system for XP, Levels, and Progression.

`Variable.Experience` handles the tricky math of adding experience, detecting level-ups, and carrying over excess XP to the next level. It supports both `int` (for standard games) and `long` (for idle games with massive numbers).

[![NuGet](https://img.shields.io/nuget/v/Variable.Experience?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Experience)

## 📦 Installation

```bash
dotnet add package Variable.Experience
```

## 🔥 Features

* **🆙 Automatic Leveling**: Detects when XP > Max and helps you process level-ups.
* **🌊 Overflow Handling**: Excess XP automatically spills over to the next level. No wasted grind.
* **🔢 Int & Long Support**: Whether you need 1,000 XP or 1,000,000,000,000 XP.
* **📐 Custom Formulas**: Plug in your own math for how XP requirements scale.

## 🛠️ Usage Guide

### 1. The Basics

```csharp
using Variable.Experience;

// Create XP: Max=1000, Current=0, Level=1
var xp = new ExperienceInt(1000, 0, 1);

// Add XP
xp += 500; // 500/1000
xp += 600; // 1100/1000 -> Level Up ready!

// Check and process level up
if (xp.IsFull())
{
    // Calculate overflow (100 xp extra)
    int overflow = xp.Current - xp.Max;

    // Create next level state (Next level needs 2000 XP)
    xp = new ExperienceInt(2000, overflow, xp.Level + 1);

    Console.WriteLine($"Ding! Level {xp.Level}!");
}
```

### 2. Handling Multiple Level Ups
If the player gets a huge XP drop, they might level up multiple times at once.

```csharp
void AddXp(int amount)
{
    xp += amount;

    while (xp.IsFull())
    {
        LevelUp();
    }
}

void LevelUp()
{
    int overflow = xp.GetOverflow();
    int nextLevel = xp.Level + 1;
    int nextMax = GetXpRequiredForLevel(nextLevel);

    xp = new ExperienceInt(nextMax, overflow, nextLevel);
}
```

### 3. Idle Games (Big Numbers)
Use `ExperienceLong` for massive values.

```csharp
var idleXp = new ExperienceLong(1_000_000_000L);
idleXp += 500_000L;
```

### 4. Custom Scaling Formulas
You can implement `INextMaxFormula` to define your curve, or just calculate it yourself like in the examples above.

```csharp
public struct LinearGrowth : INextMaxFormula
{
    public int BaseXp;

    public int GetFor(int level) => BaseXp * level;
}
```

## 🧩 Integration Tips

* **Save Systems**: Just save `Current`, `Max`, and `Level`.
* **UI**: `xp.GetRatio()` works perfectly for XP bars!

## 🤝 Contributing
Found a bug? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
