# RpgStatField - Selective Field Modification System

## Overview

The `RpgStatField` enum provides a type-safe way to selectively read and write individual fields of an `RpgStat` struct,
following the Data-Oriented Design principles.

## Architecture

This implementation follows the **Data-Logic-Extension Triad**:

### 1. Data Layer (`RpgStatField` enum)

```csharp
public enum RpgStatField
{
    None = 0,
    Base = 1,      // Base value
    ModAdd = 2,    // Flat modifier
    ModMult = 4,   // Multiplier modifier
    Min = 8,       // Minimum bound
    Max = 16,      // Maximum bound
    Value = 32     // Cached calculated value
}
```

### 2. Logic Layer (`RpgStatLogic` static class)

Pure functions operating on primitives:

```csharp
// Set a specific field
public static bool TrySetField(RpgStatField field, float newValue, 
    ref float baseVal, ref float modAdd, ref float modMult, 
    ref float min, ref float max, ref float value)

// Get a specific field
public static bool TryGetField(RpgStatField field, 
    float baseVal, float modAdd, float modMult, 
    float min, float max, float value, 
    out float result)
```

### 3. Extension Layer (`RpgStatExtensions`)

Developer-friendly API:

```csharp
// Set field with auto-recalculation
public static bool TrySetField(ref this RpgStat stat, 
    RpgStatField field, float newValue, bool autoRecalculate = true)

// Get field value
public static bool TryGetField(ref this RpgStat stat, 
    RpgStatField field, out float value)
```

## Key Features

- ✅ **Type-Safe**: Enum-based field selection prevents typos
- ✅ **Auto-Recalculation**: Automatically updates `Value` when modifying fields
- ✅ **Pure Logic**: Logic layer uses only primitives (Burst-compatible)
- ✅ **Early Exit Pattern**: Returns `false` for invalid fields
- ✅ **Zero Allocation**: No boxing, no temporary objects

## Common Use Cases

### 1. Dynamic Max Health

```csharp
var health = new RpgStat(100f, 0f, 100f);

// Player levels up, increase max health
health.TrySetField(RpgStatField.Max, 150f);
```

### 2. Equipment System

```csharp
var damage = new RpgStat(10f);

// Equip weapon: +5 base damage
damage.TryGetField(RpgStatField.Base, out var baseVal);
damage.TrySetField(RpgStatField.Base, baseVal + 5f);

// Unequip: restore original
damage.TrySetField(RpgStatField.Base, baseVal);
```

### 3. Level-Up System

```csharp
for (var level = 2; level <= 5; level++)
{
    health.TryGetField(RpgStatField.Max, out var currentMax);
    health.TrySetField(RpgStatField.Max, currentMax + 20f);
}
```

### 4. Temporary Buffs

```csharp
// Store original
stat.TryGetField(RpgStatField.Max, out var originalMax);

// Apply buff
stat.TrySetField(RpgStatField.Max, originalMax + 50f);

// Buff expires
stat.TrySetField(RpgStatField.Max, originalMax);
```

### 5. Clamping After Max Reduction

```csharp
var health = new RpgStat(200f, 0f, 200f);
health.TrySetField(RpgStatField.Base, 200f); // Full HP

// Apply debuff that reduces max
health.TrySetField(RpgStatField.Max, 100f);

// Current health is auto-clamped to 100
Assert.Equal(100f, health.GetValue());
```

## API Reference

### TrySetField

```csharp
bool TrySetField(ref this RpgStat stat, 
    RpgStatField field, 
    float newValue, 
    bool autoRecalculate = true)
```

**Parameters:**

- `field`: Which field to modify
- `newValue`: The value to set
- `autoRecalculate`: If true (default), recalculates `Value` after setting

**Returns:** `true` if field was valid, `false` otherwise

**Example:**

```csharp
var success = stat.TrySetField(RpgStatField.Max, 200f);
if (!success)
{
    Debug.LogError("Invalid field");
}
```

### TryGetField

```csharp
bool TryGetField(ref this RpgStat stat, 
    RpgStatField field, 
    out float value)
```

**Parameters:**

- `field`: Which field to read

**Returns:** `true` if field was valid, `false` otherwise

**Example:**

```csharp
if (stat.TryGetField(RpgStatField.Max, out var maxValue))
{
    Debug.Log($"Max: {maxValue}");
}
```

## Pattern: TryX with Early Exit

The API follows the **TryX pattern** for safe operations:

```csharp
// ❌ BAD: No error handling
stat.SetField(RpgStatField.Max, 100f);

// ✅ GOOD: Check success
if (stat.TrySetField(RpgStatField.Max, 100f))
{
    // Success
}
else
{
    // Handle invalid field
}
```

## Performance Characteristics

- **No Allocations**: All operations are struct-based
- **Inline-Friendly**: Marked with `AggressiveInlining`
- **Burst-Compatible**: Logic layer uses primitives only
- **Cache-Friendly**: Sequential layout (`StructLayout.Sequential`)

## Testing

Comprehensive test suite with 34 tests:

- ✅ Basic field get/set operations
- ✅ Auto-recalculation behavior
- ✅ Clamping after bound changes
- ✅ Invalid field handling
- ✅ Real-world scenarios (leveling, equipment, buffs)

Run tests:

```bash
dotnet test Variable.RPG.Tests/Variable.RPG.Tests.csproj
```

## Integration with Existing Code

The field selector system **extends** existing functionality:

```csharp
// Old way (still works)
var stat = new RpgStat(10f);
stat.AddModifier(5f, 0.5f);
var value = stat.GetValue();

// New way (selective modification)
stat.TrySetField(RpgStatField.Max, 200f);
stat.TryGetField(RpgStatField.Base, out var baseValue);
```

## Best Practices

1. **Always check return value**:
   ```csharp
   if (!stat.TrySetField(field, value))
   {
       // Handle error
   }
   ```

2. **Use auto-recalculation** (default):
   ```csharp
   // This automatically recalculates Value
   stat.TrySetField(RpgStatField.Max, 100f);
   ```

3. **Disable recalculation for batch updates**:
   ```csharp
   stat.TrySetField(RpgStatField.Base, 50f, autoRecalculate: false);
   stat.TrySetField(RpgStatField.ModAdd, 10f, autoRecalculate: false);
   stat.Recalculate(); // Single recalculation at the end
   ```

4. **Store original values for reversible changes**:
   ```csharp
   stat.TryGetField(RpgStatField.Max, out var originalMax);
   // ... apply temporary buff ...
   stat.TrySetField(RpgStatField.Max, originalMax); // Restore
   ```

## Compliance with DOD Principles

### ✅ Data Layer (Pure Storage)

- `RpgStatField` is a pure enum
- No logic, only symbolic names

### ✅ Logic Layer (Pure Functions)

- `TrySetField` and `TryGetField` take primitives only
- No struct parameters
- Stateless static methods
- Early exit on invalid input

### ✅ Extension Layer (Mutation)

- Only layer allowed to mutate via `ref this`
- Decomposes struct into primitives
- Calls Logic layer
- Assigns result back

## Migration Guide

Existing code continues to work unchanged. Add field selectors where needed:

```csharp
// Before: Direct field access
stat.Max = 200f;
stat.Recalculate();

// After: Type-safe with auto-recalculation
stat.TrySetField(RpgStatField.Max, 200f);
```

---

**Status:** ✅ All 34 tests passing  
**Architecture:** Data-Logic-Extension Triad  
**Burst Compatible:** Yes (Logic layer)  
**Zero Allocation:** Yes
