# 📊 Variable.Bounded

**The star of the show.** Unbreakable numbers that never go out of bounds.

`Variable.Bounded` provides high-performance structs (`BoundedFloat`, `BoundedInt`, `BoundedByte`) that automatically clamp their values between a `Min` and `Max`. No more `if (health < 0) health = 0`. It just works.

[![NuGet](https://img.shields.io/nuget/v/Variable.Bounded?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Bounded)

## 📦 Installation

```bash
dotnet add package Variable.Bounded
```

## 🔥 Features

* **🛡️ Bulletproof Clamping**: Set it and forget it. Values *cannot* escape their bounds.
* **🚀 Zero Allocation**: Pure `struct` types. No garbage collection. Fast enough for Update loops.
* **➕ Natural Math**: Use `+`, `-`, `*`, `/`, `++`, `--` just like normal numbers.
* **📏 Standardized**: Implements `IBoundedInfo` for easy UI integration.

## 🛠️ Usage Guide

### 1. The Basics
Health bars, mana pools, stamina, shield capacity... anything that fills up and empties out.

```csharp
using Variable.Bounded;

// Create a health bar with Max=100, Min=0 (default), Current=100
var health = new BoundedFloat(100f);

// Take damage
health -= 45f;
Console.WriteLine(health); // "55/100"

// Heal up
health += 200f;
Console.WriteLine(health); // "100/100" (Clamped!)

// Check states
if (health.IsFull()) Console.WriteLine("Ready to fight!");
if (health.IsEmpty()) Console.WriteLine("You died.");
```

### 2. Constructors for Every Occasion

```csharp
// 1. Just Max (starts full, min is 0)
var stamina = new BoundedFloat(100f);

// 2. Max and Current (starts with specific value, min is 0)
var mana = new BoundedFloat(100f, 50f); // 50/100

// 3. Full Custom Range (Min, Max, Current)
// Great for temperature, reputation, or alignment systems (-100 to 100)
var reputation = new BoundedFloat(100f, -100f, 0f);
```

### 3. Implicit Magic
You can treat these structs like regular numbers for reading values.

```csharp
BoundedFloat speed = new BoundedFloat(100f);

// Implicit conversion to float
float mySpeed = speed;

// Compare directly
if (speed > 50f)
    RunFast();
```

### 4. Advanced Tricks

#### Percentage/Ratio
Perfect for UI sliders.
```csharp
float fillAmount = health.GetRatio(); // Returns 0.0 to 1.0
image.fillAmount = fillAmount;
```

#### Direct Modification
Sometimes you need to set values directly (e.g., from a save file).
```csharp
// Warning: Direct field access bypasses clamping!
health.Current = 9999f;

// Fix it with Normalize()
health.Normalize();
// health.Current is now clamped back to Max
```

#### BoundedInt and BoundedByte
Need discrete values? We got you.
```csharp
var ammo = new BoundedInt(30);
ammo--;
// ammo is 29/30
```

## 🧩 Integration

### Unity Inspector
Since these are serializable structs, they show up beautifully in the Unity Inspector.

```csharp
public class Player : MonoBehaviour
{
    public BoundedFloat Health; // Shows up as fields!

    void Start()
    {
        Health.Normalize(); // Good practice to ensure bounds on start
    }
}
```

### Network Serialization
These structs are "blittable" (except for reference types, which we don't use). You can memcpy them or send them over the network easily.

## 🤝 Contributing
Found a bug? Want to add `BoundedLong`? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
