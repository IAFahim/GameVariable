# RpgStatOperation & RpgStatModifier - Oldschool RPG Modifier System

## Overview

A complete, production-grade RPG stat modification system inspired by classic RPGs (Diablo 2, Path of Exile, Baldur's Gate). Provides 11 different operation types for equipment, buffs, debuffs, passives, and consumables.

## Architecture

### The Three Layers

```
┌────────────────────────────────────────────────────────────┐
│ Data Layer                                                  │
│ • RpgStatOperation (enum) - What to do                     │
│ • RpgStatModifier (struct) - The complete instruction     │
└────────────────────────────────────────────────────────────┘
┌────────────────────────────────────────────────────────────┐
│ Logic Layer                                                 │
│ • ApplyOperation() - Pure primitive logic                  │
│ • Takes floats, returns float, no mutation                 │
└────────────────────────────────────────────────────────────┘
┌────────────────────────────────────────────────────────────┐
│ Extension Layer                                             │
│ • ApplyModifier() - Apply single modifier                  │
│ • ApplyModifiers() - Batch apply (zero GC)                │
└────────────────────────────────────────────────────────────┘
```

## Operation Types

### 1. **Set** - Absolute Assignment
```csharp
// "Set Health to 100"
var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Set, 100f);
stat.ApplyModifier(mod);
```
**Use Cases:** Armor Break, Silence (set to 0), Full Heal (set to max)

### 2. **Add** - Flat Addition
```csharp
// "+5 Strength from Ring"
var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);
stat.ApplyModifier(mod);
```
**Use Cases:** Equipment bonuses, consumables, level-up stats

### 3. **Subtract** - Flat Subtraction
```csharp
// "-10 Max Health debuff"
var mod = new RpgStatModifier(RpgStatField.Max, RpgStatOperation.Subtract, 10f);
stat.ApplyModifier(mod);
```
**Use Cases:** Debuffs, curses, equipment requirements

### 4. **Multiply** - Direct Multiplication
```csharp
// "×2 damage from Berserk"
var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Multiply, 2f);
stat.ApplyModifier(mod);
```
**Use Cases:** Berserk, critical hits, damage amplification

### 5. **Divide** - Direct Division
```csharp
// "÷2 speed from Slow"
var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Divide, 2f);
stat.ApplyModifier(mod);
```
**Use Cases:** Slow effects, stun penalties, weapon weight

### 6. **AddPercent** - Percentage of Base
```csharp
// "+20% of Base Health" (if base is 100, adds 20)
var mod = RpgStatModifier.AddPercent(RpgStatField.Max, 0.2f);
stat.ApplyModifier(mod);
```
**Use Cases:** Endurance scaling (each point adds % max health), percentage bonuses

### 7. **SubtractPercent** - Subtract Percentage of Base
```csharp
// "-30% Armor" (if base is 100, subtracts 30)
var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.SubtractPercent, 0.3f);
stat.ApplyModifier(mod);
```
**Use Cases:** Armor shred, percentage penalties

### 8. **AddPercentOfCurrent** - Percentage of Current Value
```csharp
// "Heal 50% of current health"
var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.AddPercentOfCurrent, 0.5f);
stat.ApplyModifier(mod);
```
**Use Cases:** Healing potions, regeneration, percentage-based heal spells

### 9. **SubtractPercentOfCurrent** - Subtract Percentage of Current
```csharp
// "-25% current mana cost"
var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.SubtractPercentOfCurrent, 0.25f);
stat.ApplyModifier(mod);
```
**Use Cases:** Mana cost reduction, percentage damage spells

### 10. **Min** - Cap Maximum Value
```csharp
// "Cap damage at 100"
var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Min, 100f);
stat.ApplyModifier(mod);
```
**Use Cases:** Damage caps, max resistance limits

### 11. **Max** - Ensure Minimum Value
```csharp
// "At least 10 armor"
var mod = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Max, 10f);
stat.ApplyModifier(mod);
```
**Use Cases:** Min damage guarantees, resistance floors

## RpgStatModifier Struct

```csharp
public struct RpgStatModifier
{
    public RpgStatField Field;      // Which field (Base, Max, etc.)
    public RpgStatOperation Operation; // What to do
    public float Value;             // The operand
}
```

### Factory Methods

```csharp
// Most common: Flat addition
RpgStatModifier.AddFlat(RpgStatField.Base, 5f, sourceId: 1001);

// Percentage bonus
RpgStatModifier.AddPercent(RpgStatField.Max, 0.2f, sourceId: 2001);

// Set absolute value
RpgStatModifier.SetValue(RpgStatField.Base, 0f, sourceId: 3001);
```

## Real-World Examples

### Equipment System
```csharp
// Legendary Sword: +15 damage, +20% attack speed
var weapon = new[]
{
    RpgStatModifier.AddFlat(RpgStatField.Base, 15f, sourceId: ItemId.LegendarySword),
    RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.2f, sourceId: ItemId.LegendarySword)
};

damage.ApplyModifiers(weapon);
attackSpeed.ApplyModifiers(weapon);
```

### Buff System
```csharp
// Strength Potion: +30% strength for 60 seconds
var buff = RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.3f, sourceId: BuffId.StrengthPotion);
strength.ApplyModifier(buff);

// After 60 seconds, remove:
var removeBuff = RpgStatModifier.AddFlat(RpgStatField.ModMult, -0.3f, sourceId: BuffId.StrengthPotion);
strength.ApplyModifier(removeBuff);
```

### Level-Up System
```csharp
// Each level: +20 max health
var levelUp = RpgStatModifier.AddFlat(RpgStatField.Max, 20f);
health.ApplyModifier(levelUp);
```

### Passive Abilities
```csharp
// Passive: +15% all damage
var passive = RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.15f, sourceId: PassiveId.DamageMastery);

physicalDamage.ApplyModifier(passive);
magicDamage.ApplyModifier(passive);
fireDamage.ApplyModifier(passive);
```

### Consumables
```csharp
// Health Potion: Heal 50% of current health
var potion = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.AddPercentOfCurrent, 0.5f);
health.ApplyModifier(potion);
```

### Debuffs
```csharp
// Curse: -50% armor
var curse = new RpgStatModifier(RpgStatField.Base, RpgStatOperation.SubtractPercentOfCurrent, 0.5f, sourceId: DebuffId.ArmorBreak);
armor.ApplyModifier(curse);
```

### Endurance Scaling
```csharp
// Each point of Endurance adds 2% max health
var endurance = 50f;
var healthBonus = RpgStatModifier.AddPercent(RpgStatField.Max, endurance * 0.02f);
health.ApplyModifier(healthBonus);
// If base health is 100: 100 + (100 * (50 * 0.02)) = 200
```

## Batch Operations

### Apply Multiple Modifiers (Zero Allocation)
```csharp
var modifiers = new[]
{
    RpgStatModifier.AddFlat(RpgStatField.Base, 10f),
    RpgStatModifier.AddFlat(RpgStatField.ModAdd, 5f),
    RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.2f)
};

// Applies all modifiers, recalculates once at the end
var count = stat.ApplyModifiers(modifiers);
Console.WriteLine($"Applied {count} modifiers");
```

### Disable Auto-Recalculation for Performance
```csharp
// When applying many mods to different stats
var weaponMods = GetWeaponModifiers();

foreach (var mod in weaponMods)
{
    stats[mod.Field].ApplyModifier(mod, autoRecalculate: false);
}

// Recalculate all at once
foreach (var stat in stats)
{
    stat.Recalculate();
}
```

## Performance Characteristics

- ✅ **Zero Allocation** - Struct-based, no GC
- ✅ **Burst Compatible** - Logic layer uses primitives only
- ✅ **Cache Friendly** - Sequential layout
- ✅ **Inline-Friendly** - AggressiveInlining attributes
- ✅ **Batch Optimized** - Single recalculation for multiple mods

## Formula Summary

| Operation | Formula | Example |
|-----------|---------|---------|
| Set | `result = operand` | `Set to 100` |
| Add | `result = current + operand` | `100 + 5 = 105` |
| Subtract | `result = current - operand` | `100 - 5 = 95` |
| Multiply | `result = current * operand` | `100 * 2 = 200` |
| Divide | `result = current / operand` | `100 / 2 = 50` |
| AddPercent | `result = current + (base * operand)` | `100 + (50 * 0.2) = 110` |
| SubtractPercent | `result = current - (base * operand)` | `100 - (50 * 0.2) = 90` |
| AddPercentOfCurrent | `result = current + (current * operand)` | `100 + (100 * 0.5) = 150` |
| SubtractPercentOfCurrent | `result = current - (current * operand)` | `100 - (100 * 0.25) = 75` |
| Min | `result = min(current, operand)` | `min(100, 50) = 50` |
| Max | `result = max(current, operand)` | `max(50, 100) = 100` |

## Best Practices

### 1. Use Factory Methods
```csharp
// ✅ GOOD - Clear intent
RpgStatModifier.AddFlat(RpgStatField.Base, 5f);

// ❌ BAD - Verbose
new RpgStatModifier(RpgStatField.Base, RpgStatOperation.Add, 5f);
```

### 3. Batch Modifications
```csharp
// ✅ GOOD - One recalculation
stat.ApplyModifiers(multipleModifiers);

// ❌ BAD - Multiple recalculations
foreach (var mod in multipleModifiers)
{
    stat.ApplyModifier(mod); // Recalculates each time
}
```

### 4. Use Percentages Correctly
```csharp
// ✅ GOOD - 20% is 0.2f
RpgStatModifier.AddPercent(RpgStatField.Max, 0.2f);

// ❌ BAD - This is 2000%
RpgStatModifier.AddPercent(RpgStatField.Max, 20f);
```

## Testing

**62 tests covering:**
- ✅ All 11 operation types
- ✅ Batch modifier application
- ✅ Real RPG scenarios (equipment, buffs, passives)
- ✅ Edge cases (division by zero, clamping)
- ✅ Factory methods

Run tests:
```bash
dotnet test Variable.RPG.Tests/Variable.RPG.Tests.csproj
```

---

**Status:** ✅ 62/62 tests passing  
**Architecture:** Data-Logic-Extension Triad  
**Inspired By:** Diablo 2, Path of Exile, Baldur's Gate  
**Zero Allocation:** Yes  
**Burst Compatible:** Yes
