# 🎻 GameVariable.Synergy

### The Integration Layer ("The Orchestra")

**Ensures all your game systems play in harmony.** 🎶
This package provides composite structures and validation logic to demonstrate how `Variable.Bounded`, `Variable.Timer`, `Variable.Regen`, and others work together seamlessly.

---

## 🧠 Mental Model: The Orchestra

Think of **GameVariable.Synergy** as the **Conductor**.

*   `Variable.Bounded` is the **Drummer** (keeping the beat/health).
*   `Variable.Timer` is the **Metronome** (tracking cooldowns).
*   `Variable.Regen` is the **Violin** (smoothly rising mana).

Individually, they are talented musicians. But without a conductor (Synergy), they might play at different tempos. **Synergy** brings them together into a coherent symphony (a Playable Character).

---

## 👶 ELI5 (Explain Like I'm 5)

Imagine you have a toy box.
*   One toy counts your health.
*   One toy acts like a stopwatch.
*   One toy refills your water bottle.

**GameVariable.Synergy** is the **Instruction Manual** that shows you how to tape them all together to build a **Super Robot** that can run, shoot, and recharge all at once!

---

## 🚀 Usage

### The Synergy Character

Use `SynergyCharacter` to combine Health, Mana, Cooldowns, and XP into one efficient struct.

```csharp
using GameVariable.Synergy;
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;

// 1. Create the character
var hero = new SynergyCharacter
{
    Health = new BoundedFloat(100f),
    Mana = new RegenFloat(50f, 50f, 5f), // Max 50, starts full, +5/sec
    AbilityCooldown = new Cooldown(2f),   // 2 second cooldown
    XP = new ExperienceInt(1000)
};

// 2. Update Loop (Game Frame)
void Update(float dt)
{
    // Ticks Mana regen and Cooldowns automatically
    hero.Update(dt);
}

// 3. Cast Ability
void TryAttack()
{
    // Checks:
    // 1. Is Cooldown ready?
    // 2. Do we have 10 Mana?
    // If yes -> Consumes Mana, Resets Cooldown, Returns true.
    hero.TryCastAbility(10f, out bool castSuccess);

    if (castSuccess)
    {
        Console.WriteLine("Fireball! 🔥");
    }
    else
    {
        Console.WriteLine("Not enough mana or on cooldown! ❄️");
    }
}
```

### Why use this?

While you can manually combine `BoundedFloat` and `Cooldown` in your own code, **GameVariable.Synergy** provides:
1.  **Standardized Patterns:** The "official" way to integrate these primitives.
2.  **Interoperability Logic:** Pre-written logic to check multiple state types efficiently.
3.  **Zero Allocation:** As always, everything is a `struct`.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Suite**

</div>
