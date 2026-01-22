# ⭐ Variable.Experience

**The "Level Up" Logic.** 🆙

**Variable.Experience** handles the RPG progression loop: **Gain XP ➔ Check Limit ➔ Level Up ➔ Overflow**.

---

## 🧠 Mental Model

Think of a **Stack of Buckets**. 🪣
- You pour water (XP) into the current bucket (Level 1).
- When it overflows, you don't lose the water!
- You pour the excess into the *next* bucket (Level 2).
- But... the next bucket might be *bigger* (Harder to level up).

**Variable.Experience** handles the pouring and the bucket swapping logic.

---

## 👶 ELI5 (Explain Like I'm 5)

Without Variable.Experience:
> **You:** "Add 10,000 XP!"
> **Bug:** "Okay! Level 1 is now 10,000/100 XP."
> **You:** "Wait, he should be Level 50!"
> **You:** *Writes a complex while-loop that crashes the game.*

With Variable.Experience:
> **You:** `xp.Add(10000, MyLevelCurve);`
> **Computer:** "Okay! Leveled up 15 times. Current Level: 16. XP: 45/2000."

---

## 📦 Installation

```bash
dotnet add package Variable.Experience
```

---

## 🎮 Usage Guide

### 1. Basic Setup

```csharp
using Variable.Experience;

// Level 1.
// 0 XP.
// Need 1000 XP for next level.
var xp = new ExperienceInt(1000, 0, 1);
```

### 2. The Smart Level Up Loop

This is the standard way to handle XP in any RPG.

```csharp
public void OnKillBoss()
{
    // 1. Add XP
    xp.Current += 5000;

    // 2. Handle Overflow (Leveling Up)
    // We pass a lambda/function to determine the NEW Max XP for each level
    bool leveledUp = xp.TryLevelUp(level =>
    {
        // Example Formula: Next Level * 1000
        // Lvl 1->2: Need 1000
        // Lvl 2->3: Need 2000
        return level * 1000;
    });

    if (leveledUp)
    {
        SpawnParticles();
        ShowMessage($"Welcome to Level {xp.Level}!");
    }
}
```

### 3. Massive Numbers (MMOs)

Building a Disgaea clone or an Idle Game? Use `ExperienceLong` for 64-bit integers.

```csharp
var xp = new ExperienceLong(1_000_000_000, 0, 1); // 1 Billion XP cap
```

---

## 🔧 API Reference

### Types
- `ExperienceInt`: Standard 32-bit (Up to 2 Billion XP).
- `ExperienceLong`: Massive 64-bit (Up to 9 Quintillion XP).

### Properties
- `Current`: Current XP in this level.
- `Max`: XP required to complete this level.
- `Level`: The current level number.

### Extensions
- `Add(amount)`: Adds XP.
- `TryLevelUp(formula)`: Checks for overflow, increments level, calculates new Max using the formula, and applies overflow. **Safe for multi-level jumps!**

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
