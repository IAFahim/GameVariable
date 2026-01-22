# ⚔️ Variable.RPG

**Diamond Architecture for Stats.** 💎

**Variable.RPG** is a high-performance system for handling complex RPG attributes (Strength, Agility, Armor) and the damage pipeline (Mitigation, Resistance, Vulnerability).

---

## 🧠 Mental Model

### The Diamond Pattern 🔷
Stats are not just a number. They are a calculation:
1.  **Base:** The raw value (Strength: 10).
2.  **Add:** Bonuses (Ring: +5).
3.  **Mult:** Multipliers (Potion: x1.5).
4.  **Result:** `(Base + Add) * Mult` = `(10 + 5) * 1.5` = **22.5**.

### The Damage Pipeline 🛡️
Damage flows like water through filters:
1.  **Incoming:** 100 Fire Damage.
2.  **Lookup:** "Do I have Fire Resistance?"
3.  **Mitigation:** Apply Armor (Flat) or Resistance (Percent).
4.  **Aggregation:** Sum up all damages (Fire + Phys + Ice).
5.  **Result:** Final HP loss.

---

## 👶 ELI5 (Explain Like I'm 5)

Without Variable.RPG:
> **You:** "Attack for 10 damage!"
> **You:** "Wait, he has armor. Subtract 2. So 8."
> **You:** "Wait, he is weak to fire! Multiply by 2. So 16."
> **You:** *Writes 50 nested if-statements.*

With Variable.RPG:
> **You:** `DamageLogic.ResolveDamage(stats, damage, config);`
> **Computer:** "I calculated Armor, Fire Resistance, Buffs, and Debuffs. He takes 14.5 damage."

---

## 📦 Installation

```bash
dotnet add package Variable.RPG
```

---

## 🎮 Usage Guide

### 1. Defining Stats
You define which numbers mean what.

```csharp
public static class StatIds
{
    public const int Health = 0;
    public const int Armor = 1;
    public const int FireResist = 2;
    public const int Strength = 3;
}
```

### 2. The Stat Sheet
Create a sheet of stats.

```csharp
using Variable.RPG;

// Create a sheet with 10 slots
var sheet = new RpgStatSheet(10);

// Setup initial values
sheet.SetBase(StatIds.Health, 100f);      // 100 HP
sheet.SetBase(StatIds.Armor, 10f);        // 10 Armor
sheet.SetBase(StatIds.FireResist, 0.5f);  // 50% Resist
sheet.SetBase(StatIds.Strength, 10f);     // 10 Str
```

### 3. Modifiers (Buffs/Gear)
Equip a "Ring of Strength".

```csharp
// Get reference to Strength
ref var str = ref sheet.GetStat(StatIds.Strength);

// Add Modifier: +5 Flat, +0% Mult
AttributeLogic.AddModifier(ref str, 5f, 0f);

// Result: (10 + 5) * 1.0 = 15 Strength
```

### 4. Taking Damage
Calculate damage using the pipeline.

```csharp
// 1. Define the attack
var damages = new[] {
    new DamagePacket { ElementId = ElementIds.Physical, Amount = 50f },
    new DamagePacket { ElementId = ElementIds.Fire, Amount = 100f }
};

// 2. Resolve it
// (You need to implement IDamageConfig to tell it which stat resists which element)
float finalDmg = DamageLogic.ResolveDamage(
    sheet.AsSpan(),
    damages,
    MyGameConfig.Instance
);

// 3. Apply it
sheet.GetStat(StatIds.Health).Base -= finalDmg;
```

---

## 🚀 Advanced: The Condition System

Want to check requirements like "Strength > 50" or "Health < 20%" without allocating memory?

```csharp
// Create a reusable condition
var lowHealth = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

// Check it
if (sheet.GetStat(StatIds.Health).Satisfies(lowHealth))
{
    ActivateBerserkMode();
}
```

---

## 🔧 API Reference

### `RpgStat` (The Struct)
- `Base`: Raw value.
- `ModAdd`: Sum of flat bonuses.
- `ModMult`: Sum of multipliers (starts at 1.0).
- `Value`: The calculated result.

### `DamageLogic`
- `ResolveDamage(...)`: The heavy lifter. Takes stats, damage packets, and config. Returns total damage.

### `IDamageConfig`
- `TryGetMitigationStat(...)`: Maps `Fire` -> `FireResist`. You implement this!

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
