# 🔋 Variable.Reservoir

**Reload!** Managing clips, magazines, and batteries.

`Variable.Reservoir` is a composite structure that manages two values: a **Volume** (what's currently available, like a clip) and a **Reserve** (the backpack, total ammo). It automates the complex logic of reloading: moving value from Reserve to Volume, handling partial fills, and checking capacities.

[![NuGet](https://img.shields.io/nuget/v/Variable.Reservoir?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Reservoir)

## 📦 Installation

```bash
dotnet add package Variable.Reservoir
```

## 🔥 Features

* **🔫 Weapon Logic**: Perfect for Guns (Mag/Reserve) or Magic (Mana Pool/Potions).
* **🔄 Auto-Refill**: One method to move resources from Reserve to Volume.
* **🔢 Int & Float**: `ReservoirInt` for bullets, `ReservoirFloat` for battery charge.
* **⚡ Zero Allocation**: Struct-based high performance.

## 🛠️ Usage Guide

### 1. The Basics: Ammo System
Imagine a pistol: 12 rounds in the mag, 36 in your pocket.

```csharp
using Variable.Reservoir;

// VolumeMax=12, VolumeCurrent=12, ReserveCurrent=36
var pistol = new ReservoirInt(12, 12, 36);

// Shoot!
pistol.Volume--;
// Volume: 11/12, Reserve: 36

// Reload!
pistol.Refill();
// Volume: 12/12, Reserve: 35
```

### 2. Properties
The struct exposes two main properties, both are `Bounded` types!

```csharp
// The Magazine (BoundedInt)
pistol.Volume.Current;
pistol.Volume.Max;

// The Backpack (BoundedInt)
pistol.Reserve.Current;
```

### 3. Partial Reloads
What if you interrupt the reload? Or reload a shotgun shell by shell?

```csharp
// Transfer 1 unit from Reserve to Volume
pistol.TransferFromReserve(1);
```

### 4. Battery / Energy (Float)
Using `ReservoirFloat` for continuous values.

```csharp
// Battery: Holds 100 energy, connected to a power grid (Reserve) of 5000
var battery = new ReservoirFloat(100f, 0f, 5000f);

// Drain battery
battery.Volume -= 10f * Time.deltaTime;

// Recharging from grid
if (ConnectedToGrid)
{
    battery.Refill(20f * Time.deltaTime); // Transfer at rate of 20/sec
}
```

## 🧩 Common Patterns

### "Infinite" Ammo
Just set the Reserve to a huge number, or handle the logic by only checking `Volume`.

### Consumables
Use `ReservoirInt` for potions.
* Volume: 1 (Potion in hand)
* Reserve: 99 (Potions in bag)
* On Drink: `Volume--`, then `Refill()` to grab another.

## 🤝 Contributing
Found a bug? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
