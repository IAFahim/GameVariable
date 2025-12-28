# Variable.Range

**Variable.Range** provides structs for defining numerical ranges (Min/Max) and generating random values within them. Essential for damage rolls, loot tables, and procedural generation.

## Installation

```bash
dotnet add package Variable.Range
```

## Features

*   **RangeFloat / RangeInt**: Simple Min/Max structs.
*   **GetRandom()**: Returns a random value within the range.
*   **Lerp()**: Linear interpolation between Min and Max.

## Usage

### Weapon Damage
```csharp
using Variable.Range;

public class Weapon
{
    public RangeInt Damage = new RangeInt(10, 20); // 10 to 20 damage

    public int Attack()
    {
        return Damage.GetRandom(); // Returns e.g., 14
    }
}
```

### Procedural Spawning
```csharp
RangeFloat spawnDelay = new RangeFloat(1.5f, 4.0f);
float nextSpawn = Time.time + spawnDelay.GetRandom();
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
