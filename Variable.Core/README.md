# Variable.Core

**Variable.Core** provides the fundamental interfaces and types for the **GameVariable** ecosystem. It defines the
contract that other variable types follow, ensuring consistency across your game's data structures.

## Installation

```bash
dotnet add package Variable.Core
```

## Features

* **IBoundedInfo**: The base interface for all variable types that have min/max bounds.
* **ICompletable**: The interface for time-based types that track completion state.
* **Common Extensions**: Shared utility methods like `IsFull()`, `IsEmpty()`, `GetRatio()`.

## Interfaces

### IBoundedInfo

Represents any value constrained by a minimum and maximum range (health, mana, stamina, etc.).

### ICompletable

Represents time-based values that progress toward completion (timers, cooldowns).

## Usage

While rarely used directly in game logic, this package is essential if you are building custom variable types that
integrate with the ecosystem.

```csharp
using Variable.Core;

// Bounded value example
public struct MyCustomHealth : IBoundedInfo
{
    public float Current;
    public float Max;
    
    float IBoundedInfo.Min => 0f;
    float IBoundedInfo.Current => Current;
    float IBoundedInfo.Max => Max;
}

// Completable example
public struct MyCustomCast : ICompletable, IBoundedInfo
{
    public float Progress;
    public float Duration;
    
    bool ICompletable.IsComplete => Progress >= Duration;
    float IBoundedInfo.Current => Progress;
    float IBoundedInfo.Max => Duration;
    float IBoundedInfo.Min => 0f;
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
