# 📈 Variable.Curve

**Zero-Allocation Mathematical Curves for Game Mechanics.**

[![NuGet](https://img.shields.io/nuget/v/Variable.Curve?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Curve)

Stop writing hardcoded math formulas for your game logic.
**Variable.Curve** provides a struct-based, zero-allocation way to define and evaluate common game curves.

## ✨ Features

*   **Zero Allocation:** Just a struct. No classes, no GC pressure.
*   **Burst Compatible:** Ready for Unity DOTS and Jobs.
*   **Standard Curves:** Linear, Power (Polynomial), Exponential, Logarithmic, Logistic (S-Curve), Sine.
*   **Clean API:** `curve.Evaluate(level)`

## 🚀 Quick Start

### 1. XP Curve (Quadratic)
Standard RPG leveling: Level 1 needs 100 XP, Level 10 needs 10,000 XP.

```csharp
using Variable.Curve;

// y = 100 * x^2
var xpCurve = CurveFloat.Power(100f, 2f);

float xpForLevel1 = xpCurve.Evaluate(1); // 100
float xpForLevel10 = xpCurve.Evaluate(10); // 10000
```

### 2. Diminishing Returns (Logarithmic)
Armor or cooldown reduction where adding more stats gives less benefit.

```csharp
// y = 50 * log2(armor + 1)
var armorCurve = CurveFloat.Logarithmic(50f, 2f);

float reduction10 = armorCurve.Evaluate(10); // ~172
float reduction100 = armorCurve.Evaluate(100); // ~332 (Not 10x more!)
```

### 3. Population Growth (Logistic/S-Curve)
City builder population growth that starts slow, explodes, then caps out.

```csharp
// Max 1000 people, steepness 1, midpoint at day 10
var popCurve = CurveFloat.Logistic(1000f, 1f, 10f);

float day1 = popCurve.Evaluate(1); // Near 0
float day10 = popCurve.Evaluate(10); // 500 (Halfway)
float day20 = popCurve.Evaluate(20); // Near 1000 (Capped)
```

## 📦 Supported Curves

| Type | Formula | Use Case |
|------|---------|----------|
| **Linear** | `y = mx + c` | Simple scaling, shop prices. |
| **Power** | `y = m * x^p + c` | RPG Leveling (Square/Cubic), Physics drag. |
| **Exponential** | `y = m * b^x + c` | Inflation, Cell growth. |
| **Logarithmic** | `y = m * log_b(x+1) + c` | Armor, Efficiency, Diminishing returns. |
| **Logistic** | `S-Curve` | Population, Market saturation, Smooth transitions. |
| **Sine** | `Wave` | Day/Night cycle, Tides, Breathing animation. |

## 🔧 Advanced

### Combining with Variable.Experience

```csharp
public struct Hero
{
    public ExperienceInt XP;
    public CurveFloat XPCurve;

    public void CheckLevelUp()
    {
        int req = XPCurve.EvaluateInt(XP.Level);
        if (XP.Current >= req)
        {
            // Level Up!
        }
    }
}
```
