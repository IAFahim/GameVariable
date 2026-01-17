# 🔷 Variable.Core

**The Foundation of the GameVariable Ecosystem.**

`Variable.Core` defines the shared DNA that makes all other GameVariable packages work together seamlessly. It provides the essential interfaces (`IBoundedInfo`, `ICompletable`) and math constants that ensure your health bars, timers, and reservoirs all speak the same language.

[![NuGet](https://img.shields.io/nuget/v/Variable.Core?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Core)

## 📦 Installation

```bash
dotnet add package Variable.Core
```

> **Note:** You usually don't need to install this directly if you're using other packages like `Variable.Bounded` or `Variable.Timer`, as they already include it.

## 🔑 Key Components

### 1. `IBoundedInfo` Interface
The holy grail of "bar-based" values. If it has a Min, a Max, and a Current value, it implements this.

```csharp
public interface IBoundedInfo
{
    float Min { get; }
    float Max { get; }
    float Current { get; }
}
```

**Why is this cool?**
Because you can write generic code that handles *anything* with a progress bar!

```csharp
// Works for Health, Mana, Experience, Ammo, Cooldowns...
public void UpdateUI(IBoundedInfo info)
{
    _slider.value = info.GetRatio(); // 0.0 to 1.0
}
```

### 2. `ICompletable` Interface
For things that finish.

```csharp
public interface ICompletable
{
    bool IsComplete { get; }
}
```

Useful for checking if a `Timer` is done, a `Quest` is finished, or a `LoadingBar` is full without caring about the implementation details.

### 3. Math Constants & Utilities
We enforce standard floating-point comparisons to keep your game deterministic and bug-free.

* `MathConstants.DefaultTolerance`: `0.001f` - The standard epsilon for "close enough".
* `CoreMath.Approximately(a, b)`: Safe float comparison.

## 🛠️ Usage Examples

### Creating a Custom Resource
Want to make your own weird resource type? Just implement `IBoundedInfo`!

```csharp
using Variable.Core;

public struct MyWeirdShield : IBoundedInfo
{
    public float Power;
    public float MaxPower;
    
    // Implement the interface
    public float Current => Power;
    public float Max => MaxPower;
    public float Min => 0f;

    // Now you get all extensions for free!
    // .IsFull(), .IsEmpty(), .GetRatio()...
}

// Usage
var shield = new MyWeirdShield { Power = 50, MaxPower = 100 };
if (shield.GetRatio() < 0.5f)
    PlayWarningSound();
```

### Generic Systems
Build systems that don't care if it's health or ammo.

```csharp
public class BarVisualizer : MonoBehaviour
{
    public void Display(IBoundedInfo value)
    {
        Console.WriteLine($"Status: {value.Current}/{value.Max} ({value.GetRatio():P0})");
    }
}
```

## 🤝 Contributing
Found a bug? Want to add a feature? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
