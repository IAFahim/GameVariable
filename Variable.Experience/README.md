# ⭐ Variable.Experience

**Level Up Your Leveling System.** 🆙

**Variable.Experience** handles the core loop of RPG progression: Gaining XP, checking for level-ups, and handling the "overflow" (e.g., getting enough XP to jump 2 levels at once).

---

## 📦 Installation

```bash
dotnet add package Variable.Experience
```

---

## 🧠 The Mental Model

**The Bucket Chain.** 🪣➡️🪣

XP is just filling a bucket.
*   **IsFull():** The bucket is full.
*   **Level Up:** You empty the bucket, get a BIGGER bucket (new Max XP), and pour any leftover water (Overflow) into the new one.

## 👶 ELI5

**"The Sticker Book."** 📒

*   You need **10 stickers** to finish Page 1.
*   When you finish it, you turn the page (**Level Up!**).
*   Now you need **20 stickers** for Page 2.
*   If someone gives you **50 stickers** at once, you might finish Page 1, Page 2, *and* Page 3 in one go!

---

## 🚀 Features

* **📈 Smart Overflow:** If you gain 1,000,000 XP at level 1, it correctly calculates how many levels you gain.
* **🔧 Flexible Curves:** It doesn't force a formula on you. You decide what "Max XP" is for each level.
* **⚡ Zero Allocation:** Pure structs.

---

## 🎮 Usage Guide

### 1. Basic Setup

```csharp
using Variable.Experience;

// Level 1, 0 XP, Need 100 to level up
var xp = new ExperienceInt(100, 0, 1);

public void OnKillSlime()
{
    // Add 50 XP
    // The operator+ creates a new struct with updated values
    xp += 50;

    // Check if we leveled up
    if (xp.IsFull())
    {
        LevelUp();
    }
}
```

### 2. Handling Level Ups (The Smart Way)

Real games have overflow. If I need 10 XP and I gain 500 XP, I should level up multiple times!

```csharp
public void AddExperience(int amount)
{
    // Add the XP
    xp += amount;

    // Loop until we stop leveling up
    while (xp.IsFull())
    {
        // 1. Calculate surplus XP (Current - Max)
        int overflow = xp.Current - xp.Max;

        // 2. Increase Level
        int newLevel = xp.Level + 1;

        // 3. Calculate new Max XP (Your custom formula!)
        // Example: Level * 1000 (1000, 2000, 3000...)
        int newMax = newLevel * 1000;

        // 4. Create new state
        xp = new ExperienceInt(newMax, overflow, newLevel);

        PlayLevelUpSound();
    }
}
```

### 3. Long XP (MMOs)

Building an MMO where players have billions of XP? Use `ExperienceLong`.

```csharp
var xp = new ExperienceLong(1_000_000_000, 0, 1);
```

---

## 🔧 API Reference

### `ExperienceInt` / `ExperienceLong`
- `Current`: Current XP.
- `Max`: XP required for *next* level.
- `Level`: Current level.

### Logic
- `IsFull()`: Ready to level up?
- `GetRatio()`: Progress bar (0.0 to 1.0).
- `GetRemaining()`: XP needed for next level.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
