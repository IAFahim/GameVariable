# Variable.RPG

**Diamond Architecture for RPG Attributes and Damage Pipelines** — AAA-grade, zero-allocation, framework-agnostic.

---

## 🎯 What Is This?

A complete RPG stat system implementing the **Diamond Architecture** pattern:

- **Aggregation**: Multiple damage sources → single result
- **Pipeline**: Damage → Mitigation → Final Value
- **Span-Based**: Works with arrays, NativeArray, BlobArray
- **Pure Logic**: No framework dependencies

---

## ⚡ Quick Example

```csharp
// 1. Define your game's stats
public static class Stats {
    public const int Health = 0;
    public const int Armor = 1;
    public const int FireResist = 2;
}

// 2. Create attribute sheet
// Allocates unmanaged memory - Must be disposed!
using var sheet = new RpgStatSheet(10); // 10 stats

// 3. Set Base Values
sheet.SetBase(Stats.Health, 100f);
sheet.SetBase(Stats.Armor, 10f);
sheet.SetBase(Stats.FireResist, 0.5f); // 50% resist

// 4. Apply modifiers
// +5 flat, +20% mult → (10+5)*1.2 = 18 armor
ref var armorStat = ref sheet.GetRef(Stats.Armor);
RpgStatLogic.AddModifier(ref armorStat, 5f, 0.2f);

// 5. Take damage
var damages = new[] {
    new DamagePacket { ElementId = DmgTypes.Physical, Amount = 50f },
    new DamagePacket { ElementId = DmgTypes.Fire, Amount = 100f }
};

var finalDamage = DamageLogic.ResolveDamage(
    sheet.AsSpan(), 
    damages, 
    new MyConfig());

// Result: (50-18) + (100*0.5) = 32 + 50 = 82 damage
```

---

## 🏗️ Architecture

### Data Layer (Structs)

```csharp
// Complex stat with modifiers
public struct RpgStat {
    public float Base;      // Base value (10 Strength)
    public float ModAdd;    // Flat bonuses (+5 from ring)
    public float ModMult;   // Multipliers (x1.2 from buff)
    public float Min, Max;  // Bounds
    public float Value;     // Cached result
}

// Damage instance
public struct DamagePacket {
    public int ElementId;   // Fire, Physical, etc.
    public float Amount;
    public int Flags;       // Critical, etc.
}

// Attribute container
public unsafe struct RpgStatSheet {
    // Manages a pointer to unmanaged memory (RpgStat*)
    // Provides safe Span<RpgStat> views
}
```

### Logic Layer (Static Methods)

```csharp
// Attribute calculations
public static class RpgStatLogic {
    void Recalculate(ref RpgStat stat)
    void AddModifier(ref RpgStat stat, float flat, float percent)
    void ClearModifiers(ref RpgStat stat)
    float GetValue(ref RpgStat stat)
}

// Damage pipeline
public static class DamageLogic {
    float ResolveDamage(
        Span<RpgStat> stats,
        ReadOnlySpan<DamagePacket> damages,
        IDamageConfig config)
}
```

### Configuration Layer (Interface)

```csharp
public interface IDamageConfig {
    bool TryGetMitigationStat(
        int elementId, 
        out int statId, 
        out bool isFlat)
}
```

---

## 💎 Diamond Architecture

The damage pipeline follows the Diamond pattern:

```
Multiple Sources         Aggregation         Single Result
    ┌─────┐
    │Fire │────┐
    └─────┘    │
    ┌─────┐    ├──→ [Pipeline] ──→  Total Damage
    │Phys │────┤
    └─────┘    │
    ┌─────┐    │
    │Shock│────┘
    └─────┘
```

**Pipeline Steps**:

1. **Lookup**: Map ElementID → MitigationStatID
2. **Calculate**: Apply Armor (flat) or Resist (%)
3. **Aggregate**: Sum all mitigated damages

---

## 🎮 Usage Patterns

### Basic Attributes

```csharp
var stat = new RpgStat(10f, 0f, 1000f);
RpgStatLogic.AddModifier(ref stat, 5f, 0.5f); // +5 flat, +50% mult

var value = RpgStatLogic.GetValue(ref stat);
// (10 + 5) * 1.5 = 22.5
```

### Bounded Attributes

```csharp
var health = new RpgStat(100f, 0f, 200f);
RpgStatLogic.AddModifier(ref health, 150f, 0f);

var val = RpgStatLogic.GetValue(ref health);
// 100 + 150 = 250, clamped to 200
```

### Damage Configuration

```csharp
public struct MyConfig : IDamageConfig {
    public bool TryGetMitigationStat(
        int elementId, 
        out int statId, 
        out bool isFlat)
    {
        switch (elementId) {
            case DmgTypes.Physical:
                statId = Stats.Armor;
                isFlat = true;  // Flat reduction
                return true;
            
            case DmgTypes.Fire:
                statId = Stats.FireResist;
                isFlat = false; // Percentage
                return true;
            
            default:
                statId = -1;
                isFlat = false;
                return false; // No mitigation
        }
    }
}
```

### Damage Resolution

```csharp
// Multi-hit attack (grenade)
var damages = new[] {
    new DamagePacket { ElementId = DmgTypes.Fire, Amount = 80f },
    new DamagePacket { ElementId = DmgTypes.Physical, Amount = 20f },
    new DamagePacket { ElementId = DmgTypes.Shock, Amount = 10f }
};

var totalDamage = DamageLogic.ResolveDamage(
    defender.Stats.AsSpan(),
    damages,
    gameConfig);
```

---

## 🚀 Advanced Features

### Condition System

Query stats with zero-allocation conditions:

```csharp
// "Is Health < 20% of Max?"
var lowHp = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

if (health.Satisfies(lowHp)) {
    // Trigger emergency healing
}

// "Is Strength >= 50?" (Equipment requirement)
var canEquip = RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f);

// "Is Multiplier > 5x?" (Buff stacking check)
var hugeBuff = RpgStatCondition.FieldCheck(
    RpgStatField.ModMult,
    StatComparisonOp.GreaterThan,
    5.0f
);

// Multiple conditions (AND logic)
var requirements = new[] {
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f),
    RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterThan, 0.5f)
};

if (strength.SatisfiesAll(requirements)) {
    // All requirements met
}

// Count satisfied conditions (progression system)
var count = stat.CountSatisfied(milestones);
if (stat.SatisfiesAtLeast(milestones, 3)) {
    // At least 3 of 5 requirements passed
}
```

**Supported Operations**:
- `Equal`, `NotEqual`
- `GreaterThan`, `GreaterOrEqual`
- `LessThan`, `LessOrEqual`

**Reference Sources**:
- `FixedValue` — Raw numbers (e.g., 50)
- `Max` — Percentage of max (e.g., 20% of Max)
- `Base` — Percentage of base (e.g., 150% of Base)
- `Value` — Percentage of current (e.g., 80% of Current)

### Span-Based (Zero Copy)

```csharp
// Works with managed arrays
RpgStat[] stats = new RpgStat[10];
DamageLogic.ResolveDamage(stats.AsSpan(), damages, config);

// Works with NativeArray (Unity Jobs)
NativeArray<RpgStat> stats = ...;
DamageLogic.ResolveDamage(stats, damages, config);

// Works with stackalloc (zero allocation)
Span<RpgStat> stats = stackalloc RpgStat[5];
DamageLogic.ResolveDamage(stats, damages, config);
```

### Amplified Damage (Negative Resistance)

```csharp
// -50% fire resist = +50% damage taken
var stat = new RpgStat(-0.5f, -1f, 1f);

var dmg = new DamagePacket { ElementId = DmgTypes.Fire, Amount = 100f };
// 100 * (1 - (-0.5)) = 150 damage
```

### Over-Armor (Damage Reduction to 0)

```csharp
ref var armor = ref sheet.GetRef(Stats.Armor);
armor.Base = 100f;

var dmg = new DamagePacket { ElementId = DmgTypes.Physical, Amount = 10f };
// 10 - 100 = -90, clamped to 0
```

---

## 📊 Formula Reference

### Attribute Calculation

```
Value = (Base + ModAdd) * ModMult
Clamped to [Min, Max]
```

**Example**:

```
Base = 10
ModAdd = +5 (from items)
ModMult = 1.2 (1.0 + 0.2 from buffs)

Value = (10 + 5) * 1.2 = 18
```

### Damage Mitigation

**Flat (Armor)**:

```
FinalDamage = IncomingDamage - Armor
Clamped to >= 0
```

**Percentage (Resistance)**:

```
FinalDamage = IncomingDamage * (1 - Resistance)
No clamp (allows amplification)
```

---

## ✨ Features

✅ **Zero Allocation** — No GC in hot paths
✅ **Span-Based** — NativeArray, BlobArray, stackalloc compatible
✅ **Framework Agnostic** — No Unity/Unreal dependencies
✅ **Type-Safe Config** — Interface for game-specific rules
✅ **Aggregation** — Multiple damage sources in one call
✅ **Bounded** — Min/Max enforcement
✅ **Condition System** — Query stats with zero-allocation conditions
✅ **Tested** — Comprehensive unit tests covering edge cases

---

## 🧪 Test Coverage

- ✅ Diamond pattern (Base + Flat + Mult)
- ✅ Min/Max clamping
- ✅ Armor (flat mitigation)
- ✅ Resistance (percentage mitigation)
- ✅ Mixed damage types
- ✅ Negative damage (over-armor)
- ✅ Amplified damage (negative resist)
- ✅ Unmapped elements (full damage)
- ✅ Empty damage arrays

---

## 🎯 Use Cases

### Action RPG

```csharp
// Player takes multi-element hit
var damages = new[] {
    new DamagePacket { ElementId = Fire, Amount = 100 },
    new DamagePacket { ElementId = Physical, Amount = 50 }
};
```

### Turn-Based RPG

```csharp
// Calculate damage before animation
var preview = DamageLogic.ResolveDamage(target.Stats.AsSpan(), spell.Damages, config);
// Show preview, then apply
```

### MMO

```csharp
// Server authoritative damage
for (var i = 0; i < targets.Length; i++) {
    var dmg = DamageLogic.ResolveDamage(targets[i].Stats, aoe.Damages, rules);
    ApplyDamage(targets[i], dmg);
}
```

---

## 📚 Documentation

- **This README** — API reference & examples
- **RpgStatCondition_GUIDE.md** — Complete condition system guide with Unity examples
- **Tests** — Variable.RPG.Tests (comprehensive unit tests)

---

**Diamond Architecture. Zero Allocation. AAA-Grade.** 💎
