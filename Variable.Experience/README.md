# ⭐ Variable.Experience

**Level up your game without the math mess.** 📈

**Variable.Experience** handles XP, Levels, and the "Next Level Requirement" curve. It supports custom formulas so you can define exactly how hard it is to reach level 99.

---

## 🧠 Mental Model: "The Bucket Chain" 🪣

Think of leveling like a series of buckets, each bigger than the last.
- **Current XP:** Water in the current bucket.
- **Max XP:** Size of the current bucket.
- **Level Up:** When the bucket overflows, you pour the *extra* water into the next (bigger) bucket and call it "Level 2".

## 👶 ELI5: "Filling the Bar"

You have a bar. When you fill it up, *Ding!* You are now Level 2, and you get a new, empty, bigger bar to fill.

---

## 📦 Installation

```bash
dotnet add package Variable.Experience
```

---

## 🛠️ Usage Guide

### 1. The Basic Setup (ExperienceInt)

```csharp
using Variable.Experience;

public struct Player
{
    // Max XP for Lvl 1 is 1000.
    public ExperienceInt XP;

    public Player()
    {
        XP = new ExperienceInt(1000);
    }
}
```

### 2. Adding XP & Leveling Up

You can manually handle it, or use the automated formula system.

#### Method A: Manual (The Hard Way)
```csharp
hero.XP += 500; // Add 500 XP

if (hero.XP.IsFull()) // Did we ding?
{
    hero.XP.Level++;
    hero.XP.Current -= hero.XP.Max; // Carry over overflow
    hero.XP.Max = (int)(hero.XP.Max * 1.5f); // Next level is 50% harder
    Console.WriteLine($"DING! Level {hero.XP.Level}");
}
```

#### Method B: Automated Formula (The Easy Way) ✨

Define a formula struct once, and reuse it everywhere.

```csharp
// Define your curve: Next Level = Current Level * 1000
public struct LinearRPGFormula : INextMaxFormula<int>
{
    public int Calculate(int level) => level * 1000;
}

// ... usage ...
var formula = new LinearRPGFormula();

// Adds XP and handles MULTIPLE level ups automatically!
// Returns number of levels gained.
int levelsGained = hero.XP.Add(5000, formula);

if (levelsGained > 0)
{
    PlayLevelUpSound();
}
```

### 3. High Numbers (MMORPG Style)

Need trillions of XP? Use `ExperienceLong`.

```csharp
var paragonXP = new ExperienceLong(1_000_000_000_000);
```

---

## 🔧 API Reference

### Fields
- **`Current`**: XP earned towards next level.
- **`Max`**: XP required for next level.
- **`Level`**: Current Level (starts at 1).

### Extensions
- **`Add(amount, formula)`**: Adds XP, handles overflow, updates Max based on formula. Returns levels gained.
- **`IsFull()`**: Is Current >= Max?
- **`GetRatio()`**: Progress bar value (0.0 to 1.0).

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
