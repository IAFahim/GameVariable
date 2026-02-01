# 📈 Variable.RPG

**The Diamond Architecture.** 💎

**Variable.RPG** is a high-performance system for complex RPG stats (Strength, Agility, Armor) and damage calculation. It handles the "Base Value + Equipment + Buffs" math so you don't have to.

---

## 🧠 Mental Model: The Diamond Architecture

How do you calculate "Attack Power"?
1.  **Base:** You have 10 Strength.
2.  **Add:** You wear a Ring of Strength (+5).
3.  **Multiply:** You cast a Rage spell (+50%).

**Variable.RPG** treats every stat as a structured calculation:
`Value = (Base + Add) * Multiplier`

It also handles **Damage Pipelines**:
`Incoming Damage -> Mitigation (Armor/Resist) -> Final Damage`

---

## 👶 ELI5: "Why did I only deal 5 damage?"

You hit the Goblin for 100 Fire Damage.
The Goblin has 50 Fire Resistance.
The Goblin is wearing Fire Armor (-10).

The math gets messy fast. **Variable.RPG** acts like a calculator that takes all the inputs (Attack, Defense, Buffs) and spits out the final number: "You deal 45 Damage."

---

## 📦 Installation

```bash
dotnet add package Variable.RPG
```

---

## 🚀 Usage Guide

### 1. Defining Stats

First, define your stats as simple IDs (integers).

```csharp
public static class StatIds
{
    public const int Health = 0;
    public const int Strength = 1;
    public const int Armor = 2;
}
```

### 2. Creating a Character Sheet

Use `RpgStatSheet` to hold all the stats. It manages the memory for you.

```csharp
using Variable.RPG;

// Create a sheet with 10 stats
var stats = new RpgStatSheet(10);

// Set Base Values
stats.SetBase(StatIds.Health, 100f);
stats.SetBase(StatIds.Strength, 10f);
stats.SetBase(StatIds.Armor, 5f);
```

### 3. Applying Modifiers (Buffs/Items)

Don't change `Base` directly. Add modifiers!

```csharp
// Get reference to Strength
ref var str = ref stats.GetRef(StatIds.Strength);

// Equip a Ring: +5 Flat Strength
// Arguments: (Ref Stat, Flat, Percent)
RpgStatLogic.AddModifier(ref str, 5f, 0f);

// Cast Rage: +50% Strength
RpgStatLogic.AddModifier(ref str, 0f, 0.5f);

// Calculate Final Value: (10 + 5) * 1.5 = 22.5
float currentStr = str.GetValue();
```

### 4. Resolving Damage

The "Killer Feature". Calculate damage across multiple types (Fire + Physical) against multiple defenses (Armor + Resist).

```csharp
// 1. Define incoming damage (Fireball + Sword)
var incoming = new[]
{
    new DamagePacket { ElementId = Element.Fire, Amount = 50f },
    new DamagePacket { ElementId = Element.Physical, Amount = 20f }
};

// 2. Define how your game handles defenses
var config = new MyGameDamageConfig(); // Implements IDamageConfig

// 3. Calculate!
float finalDamage = DamageLogic.ResolveDamage(stats.AsSpan(), incoming, config);

// 4. Apply to Health
stats.GetRef(StatIds.Health).Base -= finalDamage;
```

---

## 🔧 API Reference

### `RpgStat` (The Atom)
- `Base`: The raw value (naked character).
- `ModAdd`: Sum of all flat modifiers.
- `ModMult`: Sum of all multipliers (starts at 1.0).
- `GetValue()`: Returns `(Base + ModAdd) * ModMult`.

### `RpgStatSheet` (The Container)
- `SetBase(id, val)`: Sets base value.
- `Get(id)`: Gets final calculated value.
- `GetRef(id)`: Gets `ref` to the struct for modification.

### `DamageLogic` (The Calculator)
- `ResolveDamage(...)`: The magic function that runs the mitigation pipeline.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
