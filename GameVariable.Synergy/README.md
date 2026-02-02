# GameVariable.Synergy 🎻

**The Orchestra that makes your variables play together in harmony.**

## 🧠 Mental Model: The Conductor

Imagine your game code is an orchestra.
*   **Health** is the drums.
*   **Mana** is the violins.
*   **Cooldowns** are the trumpets.

If they all play at different tempos, it's noise. **GameVariable.Synergy** is the **Conductor**. It ensures that when you cast a spell, Mana goes down, Cooldowns start ticking, and XP might go up—all in perfect synchronization.

## 👶 ELI5 (Explain Like I'm 5)

You have a toy robot.
*   One hand holds a battery (**Mana**).
*   One hand holds a shield (**Health**).
*   It has a button to shoot lasers (**Cooldown**).

**Synergy** is the brain chip that says: "You can only shoot lasers if you have enough battery and the button isn't stuck!"

## 📦 Installation

```bash
dotnet add package GameVariable.Synergy
```

## 🚀 Usage

### The `SynergyCharacter`
A pre-built struct that combines Health, Mana, Cooldowns, and XP.

```csharp
using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;

// 1. Create your character
var hero = new SynergyCharacter
{
    Health = new BoundedFloat(100f),
    Mana = new RegenFloat(100f, 100f, 5f), // 5 mana/sec regen
    AbilityCooldown = new Cooldown(3f),    // 3s cooldown
    XP = new ExperienceInt(1000)
};

// 2. Update loop
public void Update(float dt)
{
    // Ticks Mana regen and Cooldowns automatically!
    hero.Tick(dt);

    // Try to cast fireball (Costs 20 Mana)
    if (hero.TryCastAbility(20f))
    {
        Console.WriteLine("Fireball! 🔥");
    }
    else
    {
        Console.WriteLine("Not ready yet... ⏳");
    }
}
```

## ✨ Features

*   **Zero Allocation:** `SynergyCharacter` is a struct.
*   **Integration:** Combines `Variable.Bounded`, `Variable.Regen`, `Variable.Timer`, and `Variable.Experience`.
*   **Safe Logic:** Prevents casting if Mana is too low or Cooldown is active.
