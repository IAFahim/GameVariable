# 📈 Variable.RPG

**Stats, Attributes, and Damage.** The backbone of your Role-Playing Game.

`Variable.RPG` provides a high-performance system for managing stats (Strength, Agility, Attack) and calculating damage. It uses a **Diamond Architecture** for stat modifiers (Base -> Flat -> Percent Add -> Percent Mult) to ensure math behaves exactly how game designers expect.

[![NuGet](https://img.shields.io/nuget/v/Variable.RPG?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.RPG)

## 📦 Installation

```bash
dotnet add package Variable.RPG
```

## 🔥 Features

* **💎 Diamond Architecture**: Standardized stat calculation pipeline.
* **⚡ Unmanaged Speed**: `RpgStatSheet` uses unmanaged memory for blazing fast lookups.
* **⚔️ Damage Pipeline**: `DamageLogic` handles mitigations, resistances, and element types.
* **🔧 Modifiers**: Easily add `+5 Strength` or `+10% Speed` buffs.

## 📚 Guides

* [**Unity Integration**](./UNITY_GUIDE.md): How to use in Unity.
* [**Stat Fields**](./RpgStatField_GUIDE.md): Deep dive into fields.
* [**Operations**](./RpgStatOperation_GUIDE.md): How modifiers combine.
* [**Quick Reference**](./QUICK_REFERENCE.md): Cheat sheet.

## 🛠️ Usage Guide

### 1. Defining Stats
You use `StatId` enums to identify your stats.

```csharp
public enum MyStats : int
{
    Strength = 0,
    Agility = 1,
    Attack = 2,
    Defense = 3
}
```

### 2. Using `RpgStatSheet`
This is your character's sheet. It holds all the values.

```csharp
using Variable.RPG;

// Create a sheet with 4 stats
var sheet = new RpgStatSheet(4);

// Set Base Strength to 10
sheet.SetBase((int)MyStats.Strength, 10f);

// Add a modifier: +5 Strength (Flat)
sheet.AddModifier((int)MyStats.Strength, new RpgStatModifier(5f, RpgStatOperation.Flat));

// Calculate final value
// (10 + 5) = 15
float str = sheet.GetValue((int)MyStats.Strength);
```

### 3. The Calculation Formula
The "Diamond" formula is:
```
Final = (Base + Flat) * (1 + PercentAdd) * PercentMult
```

Example:
* Base: 100
* Flat: +10 (sword)
* PercentAdd: +0.20 (20% buff)
* PercentMult: x1.5 (Critical hit or special stance)

Result: `(100 + 10) * (1 + 0.20) * 1.5` = `110 * 1.2 * 1.5` = **198**

### 4. Calculating Damage
Use `DamageLogic` to process an attack.

```csharp
var damage = new DamagePacket
{
    Amount = 50f,
    Element = ElementId.Fire
};

float defense = 10f;
float resistance = 0.5f; // 50% fire resistance

float taken = DamageLogic.Calculate(damage, defense, resistance);
// (50 - 10) * (1 - 0.5) = 20 damage
```

## ⚠️ Advanced: Memory Management
`RpgStatSheet` uses **Unmanaged Memory** to be incredibly fast and garbage-free.
**You must Dispose it!**

```csharp
sheet.Dispose();
```

In Unity, put this in `OnDestroy`.

## 🤝 Contributing
Found a bug? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
