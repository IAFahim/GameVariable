# ♻️ Variable.Regen

**Grow it back.** Automatic regeneration (or decay) for your game values.

`Variable.Regen` adds a time component to `BoundedFloat`. It's perfect for mana that recharges over time, stamina that recovers when you stop running, or radiation poisoning that slowly decays.

[![NuGet](https://img.shields.io/nuget/v/Variable.Regen?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Regen)

## 📦 Installation

```bash
dotnet add package Variable.Regen
```

## 🔥 Features

* **⏳ Automatic Logic**: Just call `.Tick(deltaTime)` and let the math happen.
* **📈 Positive or Negative**: Supports both regeneration (Mana +5/sec) and decay (Poison -2/sec).
* **🎯 Precision**: Uses `BoundedFloat` under the hood, so it never overflows.
* **⚡ Zero Allocation**: Still a struct. Still fast.

## 🛠️ Usage Guide

### 1. The Basics

```csharp
using Variable.Regen;

// Create mana: Max=100, Current=0, RegenRate=10 per second
var mana = new RegenFloat(100f, 0f, 10f);

// In your Update loop:
mana.Tick(Time.deltaTime);

// Check value (accessed via .Value property)
Console.WriteLine(mana.Value.Current);
```

### 2. Accessing the Data
`RegenFloat` is a wrapper around `BoundedFloat`.

```csharp
// Get the underlying bounded value
float current = mana.Value.Current;
float max = mana.Value.Max;

// Implicit conversion works too!
float val = mana; // Returns Current
```

### 3. Decay (Negative Regen)
Great for temporary shields, radiation, or drunk effects.

```csharp
// Shield: Starts at 100, decays by 5 per second
var shield = new RegenFloat(100f, 100f, -5f);

shield.Tick(1f); // shield is 95
shield.Tick(1f); // shield is 90
```

### 4. Modifying Rate or Value

```csharp
// Changing the regeneration rate
mana.Rate = 20f; // Turbo mode!

// Consuming resource (e.g., casting a spell)
// You modify the inner .Value
mana.Value -= 50f;
```

### 5. Advanced: Custom Logic with `RegenLogic`
If you want to keep your data pure and logic separate (Data-Oriented Design), you can use the static logic class directly.

```csharp
BoundedFloat myData = new BoundedFloat(100f);
float myRate = 5f;

// Apply regeneration step manually
RegenLogic.Tick(ref myData, myRate, Time.deltaTime);
```

## 🧩 Common Patterns

### Stamina System
```csharp
public struct StaminaSystem
{
    public RegenFloat Stamina;

    public void Update(float dt, bool isSprinting)
    {
        if (isSprinting)
        {
            // Drain while sprinting (negative regen effectively)
            Stamina.Value -= 10f * dt;
        }
        else
        {
            // Regenerate when resting
            Stamina.Tick(dt);
        }
    }
}
```

## 🤝 Contributing
Found a bug? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
