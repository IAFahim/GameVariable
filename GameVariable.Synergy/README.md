# 🎻 GameVariable.Synergy

**The Orchestra**: The integration layer that makes everything play together in harmony.

## 🧠 Mental Model

Think of `GameVariable.Synergy` as the **Conductor**. While `Variable.Bounded` is a cup, `Variable.Regen` is a faucet, and `Variable.Timer` is an egg timer, `Synergy` is the sheet music that orchestrates them all. It proves that these atomic, isolated primitives can be composed into complex, high-performance game entities without tight coupling.

## 👶 ELI5

Imagine you are building a video game character.
- Their Health is a **Cup** (`BoundedFloat`).
- Their Mana is a **Faucet** (`RegenFloat`) that fills up over time.
- Their Super Move has an **Egg Timer** (`Cooldown`) so they can't spam it.

`GameVariable.Synergy` shows you how to glue these parts together into a single `Character` struct that is fast, clean, and easy to use.

## 🛠️ Installation

Add the project reference to your solution:

```bash
dotnet add reference ../GameVariable.Synergy/GameVariable.Synergy.csproj
```

## 🚀 Usage

### The Synergy Character

The `SynergyCharacter` struct combines Health, Mana, Cooldowns, and XP into one memory-efficient block.

```csharp
// Create a character: 100 HP, 50 Mana (10 regen/sec), 3s Cooldown, 1000 XP to level
var hero = new SynergyCharacter(100f, 50f, 10f, 3f, 1000);

// Game Loop
void Update(float dt) {
    // 1. Tick the character (Regens Mana, ticks Cooldown)
    hero.Tick(dt);

    // 2. Try to cast a spell (Costs 20 Mana)
    if (Input.GetButtonDown("Fire") && hero.TryCastAbility(20f)) {
        Console.WriteLine("Fireball cast!");
    }
}
```

### The Architecture

This project strictly follows the **Data-Logic-Extension Triad**:

1.  **Layer A (Data):** `SynergyCharacter` struct. Pure data. No methods.
2.  **Layer B (Logic):** `SynergyLogic` static class. Primitives only. No structs.
3.  **Layer C (Extensions):** `SynergyExtensions`. The bridge that makes it easy to use.

This ensures that your game logic is burst-compatible, allocation-free, and easy to test.
