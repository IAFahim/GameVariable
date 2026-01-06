# Variable.RPG - Quick Reference Card

## 🎯 Create a Stat

```csharp
var health = new RpgStat(100f, 0f, 200f); // Base 100, Min 0, Max 200
var damage = new RpgStat(10f);            // Base 10, unbounded
```

## ➕ Apply Modifiers

```csharp
// Flat addition
var mod = RpgStatModifier.AddFlat(RpgStatField.Base, 5f);
stat.ApplyModifier(mod);

// Percentage
var mod = RpgStatModifier.AddPercent(RpgStatField.Max, 0.2f); // +20%
stat.ApplyModifier(mod);

// Multiple at once
var mods = new[] { mod1, mod2, mod3 };
stat.ApplyModifiers(mods); // Batch apply, one recalculation
```

## ➖ Remove Modifiers (NEW!)

```csharp
// Single modifier - AUTOMATIC INVERSION!
stat.RemoveModifier(mod);

// Multiple - AUTOMATIC INVERSION!
stat.RemoveModifiers(mods);
```

## 🔄 11 Operations

| Operation | Example | Invertible? |
|-----------|---------|-------------|
| `Add` | `+5 Strength` | ✅ |
| `Subtract` | `-10 Health` | ✅ |
| `Multiply` | `×2 Damage` | ✅ |
| `Divide` | `÷2 Speed` | ✅ |
| `AddPercent` | `+20% of Base` | ✅ |
| `SubtractPercent` | `-30% of Base` | ✅ |
| `AddPercentOfCurrent` | `+50% of Current` | ✅ |
| `SubtractPercentOfCurrent` | `-25% of Current` | ✅ |
| `Set` | `Set to 100` | ❌ |
| `Min` | `Cap at 100` | ❌ |
| `Max` | `At least 10` | ❌ |

## 🎮 Common Patterns

### Equipment
```csharp
// Equip
var weapon = new[]
{
    RpgStatModifier.AddFlat(RpgStatField.Base, 15f),
    RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.2f)
};
damage.ApplyModifiers(weapon);

// Unequip
damage.RemoveModifiers(weapon);
```

### Buffs
```csharp
var buff = RpgStatModifier.AddFlat(RpgStatField.ModMult, 0.3f);
strength.ApplyModifier(buff);      // Apply
// ... 60 seconds later ...
strength.RemoveModifier(buff);     // Expires
```

### Level-Up
```csharp
health.TrySetField(RpgStatField.Max, health.Max + 20f);
```

## 📐 Formula

```
Value = (Base + ModAdd) × ModMult
Clamped to [Min, Max]
```

## 🔍 Field Selection

```csharp
// Set specific field
stat.TrySetField(RpgStatField.Max, 200f);

// Get specific field
stat.TryGetField(RpgStatField.Base, out var baseValue);
```

## ⚡ Performance

- **Struct Size**: 12 bytes (RpgStatModifier)
- **Allocation**: Zero
- **Burst**: Compatible
- **Inline**: AggressiveInlining

## 📚 Full Guides

- `README.md` - Main documentation
- `RpgStatOperation_GUIDE.md` - Complete operation guide
- `RpgStatField_GUIDE.md` - Field selector guide
- `UNITY_GUIDE.md` - Unity integration
- `VALIDATION_REPORT.md` - Test results

## 🧪 Testing

```bash
dotnet test Variable.RPG.Tests/Variable.RPG.Tests.csproj
```

**90 tests, all passing** ✅
