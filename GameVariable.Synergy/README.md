# 🎼 GameVariable.Synergy

**The Conductor.** 🎻

**GameVariable.Synergy** (codenamed "Sergey") is the integration layer that proves all other **GameVariable** projects play perfectly together. It provides a reference architecture for building complex entities using the "Data-Logic-Extension" pattern.

---

## 🧠 The Mental Model: "The Conductor"

Imagine an orchestra.
*   **Variable.Bounded** is the Drum (Health).
*   **Variable.Regen** is the Violin (Mana).
*   **Variable.Timer** is the Metronome (Cooldowns).
*   **Variable.RPG** is the Sheet Music (Stats).
*   **GameVariable.Synergy** is the **Conductor**. It doesn't make sound itself; it waves the baton to ensure the Violin plays when the Metronome ticks, and the Sheet Music dictates how loud the Drums are.

## 👶 ELI5

You have a box of Lego bricks (Bounded, Timer, Regen).
**Synergy** is the instruction booklet that shows you how to build a cool Robot (The Ultimate Character) using all of them at once.

---

## 📦 Installation

```bash
dotnet add package GameVariable.Synergy
```

*(Note: Since this is an integration package, it pulls in almost everything else!)*

---

## 🎮 Usage Guide

### The "Ultimate Character" Structure

Here is how we compose a character using **Layer A (Data)**.

```csharp
using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;
using Variable.RPG;

// 1. Define the State (The Struct)
// Everything is POD (Plain Old Data). Zero allocation.
using var state = new SynergyState();

// Initialize Components
state.Health = new BoundedFloat(100f);
state.Mana = new RegenFloat(100f, 50f, 5f); // 50/100 mana, +5 per sec
state.SkillCooldown = new Cooldown(3f);     // 3s cooldown
state.Experience = new ExperienceInt(1000); // Level 1, need 1000 XP
state.Stats = new RpgStatSheet(10);         // 10 Stats slots

// Set some stats (Strength = Index 0)
state.Stats.SetBase(SynergyExtensions.STAT_STR, 10f); // 10 Strength
```

### The Game Loop (Layer C - Extensions)

We use extension methods to "conduct" the logic.

```csharp
void Update(float deltaTime)
{
    // Ticks Mana Regen and Cooldowns
    state.Tick(deltaTime);
}

void TryCastFireball()
{
    // Checks Mana AND Cooldown.
    // If successful: Consumes Mana, Resets Cooldown, Triggers AI Event.
    if (state.Cast(manaCost: 20f))
    {
        Console.WriteLine("Fireball! 🔥");
    }
    else
    {
        Console.WriteLine("Not enough mana or on cooldown!");
    }
}

void LootChest()
{
    // Checks Inventory Capacity based on Strength Stat!
    // 10 Str => 100 Capacity (Example Logic)
    state.Loot(50f);
}

void KillMonster()
{
    // Adds XP.
    // If Level Up: Restores Health & Mana to Full!
    state.GainXp(500);
}
```

---

## 🔧 Architecture

**GameVariable.Synergy** follows the strict **Data-Logic-Extension** triad:

1.  **SynergyState (Layer A):** A composite `struct` holding all other state structs.
2.  **SynergyLogic (Layer B):** Pure static methods for cross-cutting rules (e.g., "Can I cast?").
3.  **SynergyExtensions (Layer C):** The fluent API that orchestrates the calls.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
