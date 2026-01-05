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
var sheet = new AttributeSheet(10); // 10 stats
sheet.SetBase(Stats.Health, 100f);
sheet.SetBase(Stats.Armor, 10f);
sheet.SetBase(Stats.FireResist, 0.5f); // 50% resist

// 3. Apply modifiers
AttributeLogic.AddModifier(ref sheet.Attributes[Stats.Armor], 5f, 0.2f); 
// +5 flat, +20% mult → (10+5)*1.2 = 18 armor

// 4. Take damage
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
public struct Attribute {
    public float Base;      // Base value (10 Strength)
    public float ModAdd;    // Flat bonuses (+5 from ring)
    public float ModMult;   // Multipliers (x1.2 from buff)
    public float Min, Max;  // Bounds
    public float CachedValue;
    public bool IsDirty;
}

// Damage instance
public struct DamagePacket {
    public int ElementId;   // Fire, Physical, etc.
    public float Amount;
    public int Flags;       // Critical, etc.
}

// Attribute container
public struct AttributeSheet {
    public Attribute[] Attributes;
}
```

### Logic Layer (Static Methods)

```csharp
// Attribute calculations
public static class AttributeLogic {
    void Recalculate(ref Attribute attr)
    void AddModifier(ref Attribute attr, float flat, float percent)
    void ClearModifiers(ref Attribute attr)
    float GetValue(ref Attribute attr)
}

// Damage pipeline
public static class DamageLogic {
    float ResolveDamage(
        Span<Attribute> stats,
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
var attr = new Attribute(10f); // Base 10
AttributeLogic.AddModifier(ref attr, 5f, 0.5f); // +5 flat, +50% mult

var value = AttributeLogic.GetValue(ref attr);
// (10 + 5) * 1.5 = 22.5
```

### Bounded Attributes

```csharp
var health = new Attribute(100f, 0f, 200f); // Min 0, Max 200
AttributeLogic.AddModifier(ref health, 150f, 0f);

var val = AttributeLogic.GetValue(ref health);
// 100 + 150 = 250, clamped to 200
```

### Caching & Dirty Tracking

```csharp
var attr = new Attribute(10f);
var val1 = AttributeLogic.GetValue(ref attr); // Calculates & caches

// Modifiers changed
AttributeLogic.AddModifier(ref attr, 5f, 0f);
// attr.IsDirty == true

var val2 = AttributeLogic.GetValue(ref attr); // Recalculates
// attr.IsDirty == false
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
    defender.Attributes.AsSpan(),
    damages,
    gameConfig);

// Apply to health
defender.Attributes[Stats.Health].Base -= totalDamage;
```

---

## 🚀 Advanced Features

### Span-Based (Zero Copy)

```csharp
// Works with managed arrays
Attribute[] stats = new Attribute[10];
DamageLogic.ResolveDamage(stats.AsSpan(), damages, config);

// Works with NativeArray (Unity Jobs)
NativeArray<Attribute> stats = ...;
DamageLogic.ResolveDamage(stats, damages, config);

// Works with stackalloc (zero allocation)
Span<Attribute> stats = stackalloc Attribute[5];
DamageLogic.ResolveDamage(stats, damages, config);
```

### Amplified Damage (Negative Resistance)

```csharp
// -50% fire resist = +50% damage taken
var attr = new Attribute(-0.5f, -1f, 1f); // Allow negative

var dmg = new DamagePacket { ElementId = DmgTypes.Fire, Amount = 100f };
// 100 * (1 - (-0.5)) = 150 damage
```

### Over-Armor (Damage Reduction to 0)

```csharp
sheet.SetBase(Stats.Armor, 100f);

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
✅ **Cached Calculations** — Dirty tracking avoids redundant math  
✅ **Span-Based** — NativeArray, BlobArray, stackalloc compatible  
✅ **Framework Agnostic** — No Unity/Unreal dependencies  
✅ **Type-Safe Config** — Interface for game-specific rules  
✅ **Aggregation** — Multiple damage sources in one call  
✅ **Bounded** — Min/Max enforcement  
✅ **Tested** — 14 unit tests covering edge cases

---

## 🧪 Test Coverage

- ✅ Diamond pattern (Base + Flat + Mult)
- ✅ Min/Max clamping
- ✅ Dirty tracking & caching
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
- **Tests** — Variable.RPG.Tests (14 tests, 100% coverage)

---

**Diamond Architecture. Zero Allocation. AAA-Grade.** 💎
