# Variable.Bounded

**Variable.Bounded** offers high-performance structs for handling values that must stay within a specific range (Min/Max). Perfect for Health, Stamina, Mana, and other "bar-based" resources.

## Installation

```bash
dotnet add package Variable.Bounded
```

## Features

*   **BoundedFloat / BoundedInt**: Automatically clamps values between 0 and Max (or custom Min/Max).
*   **Allocation-Free**: Implemented as `struct` for zero garbage collection overhead.
*   **Operator Overloads**: Use `+`, `-`, `++`, `--` naturally.

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

### Custom Range (Temperature)
```csharp
// Max 100, Current 20 (Min is always 0)
// Note: BoundedFloat is designed for 0-to-Max resources.
// For ranges with negative values (like -50 to 100), you would typically 
// handle the offset in your logic (e.g., value - 50).
BoundedFloat temp = new BoundedFloat(150f, 70f); // Represents -50 to 100 range shifted up by 50
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
