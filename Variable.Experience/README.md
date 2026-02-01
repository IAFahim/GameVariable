# ⭐ Variable.Experience

**The Bucket Chain.** 🪣

**Variable.Experience** handles the math of RPG leveling. It manages the loop of filling a bar, spilling over to the next level, and repeating.

---

## 🧠 Mental Model: The Bucket Chain

Imagine a line of buckets, each bigger than the last.
1.  You pour water (XP) into the current bucket.
2.  If it overflows, you pour the *excess* into the next bucket.
3.  You keep doing this until a bucket isn't full.

**Variable.Experience** manages the pouring and the buckets.

---

## 👶 ELI5: "I killed a Dragon at Level 1!"

If you need 100 XP to reach Level 2, and you kill a Dragon that gives 5,000 XP...
You shouldn't just hit Level 2 and stop.
You should overflow to Level 3, then Level 4, then Level 5...

**Variable.Experience** handles this "multi-level jump" math for you.

---

## 📦 Installation

```bash
dotnet add package Variable.Experience
```

---

## 🚀 Usage Guide

### 1. The Basics

```csharp
using Variable.Experience;

// Level 1. Need 1000 XP for Level 2.
var xp = new ExperienceInt(1000);

public void AddXp(int amount)
{
    // Add XP to the bucket
    xp += amount;

    // Check if bucket is full
    // NOTE: In a real game, use a while loop (see below)
    if (xp.IsFull())
    {
        // Calculate next level's requirement (e.g. Linear)
        int nextLevelMax = (xp.Level + 1) * 1000;

        // Apply level up logic (resets Current, adds Level)
        // You provide the new Max
        xp.ApplyLevelUp(nextLevelMax);

        PlaySound("LevelUp");
    }
}
```

### 2. The "Dragon Slayer" Loop (Multi-Level)

Use the extension `Add` with a formula struct to handle massive XP gains automatically.

```csharp
// Define your curve (Linear, Exponential, Custom)
public struct RpgCurve : INextMaxFormula<int>
{
    public int GetMaxForLevel(int level) => level * 1000;
}

// In your game code:
var curve = new RpgCurve();

// Gain 50,000 XP at once!
// Automatically loops through levels 1 -> 50
xp.Add(50000, curve);
```

### 3. UI Implementation

Since it implements `IBoundedInfo`, it works with your standard bars.

```csharp
// "Level 5"
levelText.text = $"Level {xp.Level}";

// "500 / 1000"
xpText.text = $"{xp.Current} / {xp.Max}";

// Progress Bar (0.5)
fillImage.fillAmount = (float)xp.GetRatio();
```

---

## 🔧 API Reference

### `ExperienceInt` / `ExperienceLong`
- `Current`: XP in current bar.
- `Max`: XP needed for next level.
- `Level`: Current Level.

### Methods
- `Add(amount, formula)`: Adds XP and handles all leveling logic.
- `ApplyLevelUp(newMax)`: Manually triggers one level up, carrying over overflow.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
