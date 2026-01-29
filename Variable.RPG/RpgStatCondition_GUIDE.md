# RpgStatCondition - Complete Usage Guide

## Table of Contents
1. [Overview](#overview)
2. [Quick Start](#quick-start)
3. [Creating Conditions](#creating-conditions)
4. [Evaluating Conditions](#evaluating-conditions)
5. [Advanced Usage](#advanced-usage)
6. [Unity Integration](#unity-integration)
7. [Performance Notes](#performance-notes)

---

## Overview

The `RpgStatCondition` system provides a **zero-allocation, data-driven** way to query RPG stats. It enables you to define questions like:
- "Is Health below 20%?"
- "Is the damage multiplier above 5x?"
- "Is Strength >= 50?"

### Architecture

The system follows the **Data-Logic-Extension** triad:

| Layer | File | Responsibility |
|-------|------|----------------|
| **A: Data** | `RpgStatCondition.cs` | Pure data struct (16 bytes, serializable) |
| **B: Logic** | `RpgStatConditionLogic.cs` | Stateless evaluation, primitives only |
| **C: API** | `RpgStatConditionExtensions.cs` | Developer-facing extension methods |

---

## Quick Start

### Basic Example

```csharp
using Variable.RPG;

// Create a stat
var health = new RpgStat(100f, 0f, 200f); // Base=100, Min=0, Max=200
health.Value = 35f; // Current health is 35

// Create a condition: "Health < 20% of Max"
var lowHp = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

// Evaluate
if (health.Satisfies(lowHp))
{
    Console.WriteLine("Low health! Trigger healing!");
}
```

---

## Creating Conditions

### 1. Percentage of Max (Most Common)

Use this for threshold checks like "below 30%" or "above 90%".

```csharp
// "Is Health < 20% of Max?"
var lowHp = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

// "Is Health >= 90% of Max?"
var fullHp = RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.9f);

// "Is Mana > 50% of Max?"
var halfMana = RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterThan, 0.5f);
```

### 2. Absolute Value (Fixed Thresholds)

Use this for fixed requirements like "Strength >= 50".

```csharp
// "Is Strength >= 50?"
var canEquipSword = RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f);

// "Is Health == 0?"
var isDead = RpgStatCondition.Absolute(StatComparisonOp.Equal, 0f);

// "Is Weight <= 100?"
var canCarry = RpgStatCondition.Absolute(StatComparisonOp.LessOrEqual, 100f);
```

### 3. Field Checks (Specific Properties)

Use this to check specific stat fields like Multiplier, Base, Modifiers.

```csharp
// "Is Multiplier > 5x?" (Check for huge buffs)
var godMode = RpgStatCondition.FieldCheck(
    RpgStatField.ModMult,
    StatComparisonOp.GreaterThan,
    5.0f
);

// "Is ModAdd == 0?" (Check for no flat modifiers)
var pure = RpgStatCondition.FieldCheck(
    RpgStatField.ModAdd,
    StatComparisonOp.Equal,
    0f
);

// "Is Base != Current?" (Check if stat is modified)
var isModified = new RpgStatCondition
{
    FieldToTest = RpgStatField.Base,
    Operator = StatComparisonOp.NotEqual,
    ReferenceSource = StatReferenceSource.Value,
    ReferenceValue = 1.0f // 100% of current
};
```

### 4. Custom Conditions

Full control over all parameters.

```csharp
// "Is Base < 50% of Current?"
var customCondition = new RpgStatCondition
{
    FieldToTest = RpgStatField.Base,
    Operator = StatComparisonOp.LessThan,
    ReferenceSource = StatReferenceSource.Value,
    ReferenceValue = 0.5f
};

// Or use the factory method
var sameCondition = RpgStatCondition.Custom(
    RpgStatField.Base,
    StatComparisonOp.LessThan,
    StatReferenceSource.Value,
    0.5f
);
```

### 5. Convenience Methods (Fluent API)

Quick factory methods directly on the stat.

```csharp
var health = new RpgStat(100f, 0f, 200f);

// Factory methods that create conditions
var lowHp = health.BelowPercentOfMax(0.2f);
var highHp = health.AbovePercentOfMax(0.9f);
var strongEnough = health.AtLeast(50f);
var weakEnough = health.AtMost(100f);
var hugeBuff = health.MultiplierAbove(2.0f);
var debuffed = health.MultiplierBelow(0.8f);
```

---

## Evaluating Conditions

### Single Condition

```csharp
var health = new RpgStat(100f, 0f, 200f);
health.Value = 30f;

var lowHp = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

bool isLow = health.Satisfies(lowHp); // false (30 is not < 40)
```

### Multiple Conditions (AND Logic)

All conditions must be true.

```csharp
var requirements = new[]
{
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f), // Str >= 50
    RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterThan, 0.5f) // Str > 50% Max
};

var strength = new RpgStat(60f, 0f, 100f);

bool meetsAll = strength.SatisfiesAll(requirements); // true (60 >= 50 AND 60 > 50)
```

### Multiple Conditions (OR Logic)

At least one condition must be true.

```csharp
var options = new[]
{
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 80f), // Str >= 80
    RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.9f) // Str >= 90% Max
};

var strength = new RpgStat(85f, 0f, 100f);

bool meetsAny = strength.SatisfiesAny(options); // true (85 >= 80)
```

### Count Satisfied

Count how many conditions pass.

```csharp
var milestones = new[]
{
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 25f),
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f),
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 75f),
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 100f)
};

var level = new RpgStat(60f);

int count = level.CountSatisfied(milestones); // 2 (passes >= 25 and >= 50)
```

### At Least N Conditions

Check if at least N conditions pass.

```csharp
var requirements = new[]
{
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 10f),
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 20f),
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 30f),
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 40f),
    RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f)
};

var stat = new RpgStat(35f);

// Need to pass at least 3 of the 5 requirements
bool qualified = stat.SatisfiesAtLeast(requirements, 3); // true (35 >= 10, 20, 30)
```

---

## Advanced Usage

### Reference Sources

Compare against different stat values:

```csharp
var stat = new RpgStat(100f, 0f, 200f);
stat.Value = 60f;

// FixedValue: Compare to raw number
// 60 > 50
var fixedCond = new RpgStatCondition
{
    FieldToTest = RpgStatField.Value,
    Operator = StatComparisonOp.GreaterThan,
    ReferenceSource = StatReferenceSource.FixedValue,
    ReferenceValue = 50f
};

// Max: Compare to percentage of max
// 60 < (200 * 0.5) = 60 < 100
var maxCond = new RpgStatCondition
{
    FieldToTest = RpgStatField.Value,
    Operator = StatComparisonOp.LessThan,
    ReferenceSource = StatReferenceSource.Max,
    ReferenceValue = 0.5f
};

// Base: Compare to percentage of base
// 60 > (100 * 0.5) = 60 > 50
var baseCond = new RpgStatCondition
{
    FieldToTest = RpgStatField.Value,
    Operator = StatComparisonOp.GreaterThan,
    ReferenceSource = StatReferenceSource.Base,
    ReferenceValue = 0.5f
};

// Value: Compare to percentage of current value
// Base (100) > (60 * 1.5) = 100 > 90
var valueCond = new RpgStatCondition
{
    FieldToTest = RpgStatField.Base,
    Operator = StatComparisonOp.GreaterThan,
    ReferenceSource = StatReferenceSource.Value,
    ReferenceValue = 1.5f
};
```

### Complex Game Logic Examples

#### Equipment Requirements

```csharp
public class EquipmentRequirements
{
    public RpgStatCondition StrengthRequirement { get; set; }
    public RpgStatCondition DexterityRequirement { get; set; }

    public bool CanEquip(RpgStat strength, RpgStat dexterity)
    {
        return strength.Satisfies(StrengthRequirement) &&
               dexterity.Satisfies(DexterityRequirement);
    }
}

// Usage
var swordReq = new EquipmentRequirements
{
    StrengthRequirement = RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f),
    DexterityRequirement = RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 30f)
};

var playerStr = new RpgStat(60f);
var playerDex = new RpgStat(25f);

bool canEquip = swordReq.CanEquip(playerStr, playerDex); // false (Dex too low)
```

#### Passive Skill Triggers

```csharp
public class PassiveSkill
{
    public string Name;
    public RpgStatCondition TriggerCondition;

    public bool IsActive(RpgStat stat) => stat.Satisfies(TriggerCondition);
}

// "Berserker Rage": Triggers when HP < 20%
var berserkerRage = new PassiveSkill
{
    Name = "Berserker Rage",
    TriggerCondition = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f)
};

var health = new RpgStat(100f, 0f, 200f);
health.Value = 30f; // 15% of max

if (berserkerRage.IsActive(health))
{
    // Enable berserker bonuses
}
```

#### Achievement System

```csharp
public class Achievement
{
    public string Title;
    public RpgStatCondition[] Requirements; // AND logic
    public int RequiredCompletions; // How many requirements needed

    public bool CheckProgress(RpgStat stat)
    {
        return stat.SatisfiesAtLeast(Requirements, RequiredCompletions);
    }
}

// "Master Warrior": Reach level 50 OR have 1000 strength
var achievement = new Achievement
{
    Title = "Master Warrior",
    Requirements = new[]
    {
        RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f), // Level 50
        RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 1000f) // 1000 Str
    },
    RequiredCompletions = 1 // Need only 1 of the 2
};
```

---

## Unity Integration

### Inspector-Friendly

Since `RpgStatCondition` is a `[Serializable]` struct, it works seamlessly in Unity's Inspector:

```csharp
using Variable.RPG;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkill", menuName = "RPG/Passive Skill")]
public class PassiveSkillData : ScriptableObject
{
    public string SkillName;
    [TextArea] public string Description;

    // Fully configurable in Inspector!
    public RpgStatCondition ActivationCondition;

    [Header("Visuals")]
    public GameObject EffectPrefab;

    public void TryActivate(ref RpgStat stat)
    {
        if (stat.Satisfies(ActivationCondition))
        {
            // Activate skill
            Debug.Log($"{SkillName} activated!");
        }
    }
}
```

### Runtime Usage

```csharp
using UnityEngine;
using Variable.RPG;

public class PlayerHealth : MonoBehaviour
{
    private RpgStat health = new RpgStat(100f, 0f, 200f);

    // Inspector-configured conditions
    [SerializeField] private RpgStatCondition lowHealthThreshold;
    [SerializeField] private RpgStatCondition criticalHealthThreshold;

    private void Update()
    {
        if (health.Satisfies(criticalHealthThreshold))
        {
            // Show critical warning
        }
        else if (health.Satisfies(lowHealthThreshold))
        {
            // Show low health indicator
        }
    }
}
```

---

## Performance Notes

### Zero Allocation

The entire system is designed for zero GC allocation:

- **Structs passed by reference** (`in`, `ref`)
- **No boxing/unboxing**
- **No LINQ**
- **Burst-compatible**

### Cache-Friendly

- `RpgStatCondition` is **16 bytes** (fits in a cache line)
- Sequential layout for optimal memory access
- All logic uses primitive types only

### Benchmark Example

```csharp
using BenchmarkDotNet.Attributes;
using Variable.RPG;

[MemoryDiagnoser]
public class ConditionBenchmarks
{
    private RpgStat health = new RpgStat(100f, 0f, 200f);
    private RpgStatCondition condition;

    [GlobalSetup]
    public void Setup()
    {
        health.Value = 35f;
        condition = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);
    }

    [Benchmark]
    public bool SingleCondition()
    {
        return health.Satisfies(condition);
    }

    [Benchmark]
    public bool MultipleConditions()
    {
        var conditions = new[]
        {
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f),
            RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterThan, 0.3f)
        };
        return health.SatisfiesAll(conditions);
    }
}
```

Expected results:
- **Single condition**: ~2-5 ns, **0 allocations**
- **Multiple conditions**: ~5-10 ns, **0 allocations**

---

## Full API Reference

### RpgStatCondition (Struct)

#### Static Factory Methods

| Method | Description |
|--------|-------------|
| `PercentOfMax(op, percent)` | Creates a percentage of Max condition |
| `Absolute(op, value)` | Creates a fixed value condition |
| `FieldCheck(field, op, value)` | Creates a field-specific condition |
| `Custom(field, op, source, value)` | Creates a fully custom condition |

#### Fields

| Field | Type | Description |
|-------|------|-------------|
| `FieldToTest` | `RpgStatField` | Which stat field to check |
| `Operator` | `StatComparisonOp` | The comparison operation |
| `ReferenceSource` | `StatReferenceSource` | What to compare against |
| `ReferenceValue` | `float` | The value/percentage for comparison |

### Extension Methods

| Method | Description |
|--------|-------------|
| `Satisfies(condition)` | Check a single condition |
| `SatisfiesAll(conditions)` | AND logic (all must pass) |
| `SatisfiesAny(conditions)` | OR logic (at least one must pass) |
| `CountSatisfied(conditions)` | Count passing conditions |
| `SatisfiesCount(conditions, count)` | Check exact count |
| `SatisfiesAtLeast(conditions, min)` | Check minimum count |
| `DescribeCondition(condition)` | Get description string |

### Convenience Methods

| Method | Description |
|--------|-------------|
| `BelowPercentOfMax(percent)` | Creates "Value < percent * Max" |
| `AbovePercentOfMax(percent)` | Creates "Value >= percent * Max" |
| `AtLeast(value)` | Creates "Value >= value" |
| `AtMost(value)` | Creates "Value <= value" |
| `MultiplierAbove(value)` | Creates "ModMult > value" |
| `MultiplierBelow(value)` | Creates "ModMult < value" |

---

## Comparison Operations

| Operation | Symbol | Example |
|-----------|--------|---------|
| `Equal` | `==` | Value == 50 |
| `NotEqual` | `!=` | Value != 0 |
| `GreaterThan` | `>` | Value > 100 |
| `GreaterOrEqual` | `>=` | Value >= 50 |
| `LessThan` | `<` | Value < 20 |
| `LessOrEqual` | `<=` | Value <= 100 |

---

## Reference Sources

| Source | Description | Example |
|--------|-------------|---------|
| `FixedValue` | Raw number | 50 |
| `Max` | Percentage of max | Max * 0.5 (50%) |
| `Base` | Percentage of base | Base * 1.5 (150%) |
| `Value` | Percentage of current | Value * 0.8 (80%) |
