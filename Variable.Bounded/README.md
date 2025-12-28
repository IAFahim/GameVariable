# Variable.Bounded

**Variable.Bounded** offers high-performance structs for handling values that must stay within a specific range (
Min/Max). Perfect for Health, Stamina, Mana, and other "bar-based" resources.

## Installation

```bash
dotnet add package Variable.Bounded
```

## Features

* **BoundedFloat / BoundedInt**: Automatically clamps values between 0 and Max (or custom Min/Max).
* **Allocation-Free**: Implemented as `struct` for zero garbage collection overhead.
* **Operator Overloads**: Use `+`, `-`, `++`, `--` naturally.

## Usage

### Health System

```csharp
using Variable.Bounded;

public class Player
{
    public BoundedFloat Health = new BoundedFloat(100f); // Max 100, Current 100

    public void TakeDamage(float amount)
    {
        Health -= amount; // Automatically clamps to 0
        if (Health.IsEmpty) Die();
    }

    public void Heal(float amount)
    {
        Health += amount; // Automatically clamps to Max
    }
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
