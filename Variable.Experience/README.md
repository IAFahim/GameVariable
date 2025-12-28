# Variable.Experience

**Variable.Experience** handles the logic for leveling systems, including experience accumulation, level caps, and overflow handling.

## Installation

```bash
dotnet add package Variable.Experience
```

## Features

*   **ExperienceInt**: Manages Level, Current XP, and Target XP.
*   **Level Up Logic**: Automatically handles multiple level-ups if huge XP is gained.
*   **Custom Curves**: You define the "Target XP" for next level.

## Usage

### RPG Leveling
```csharp
using Variable.Experience;

// Level 1, 0 XP, Need 100 for Level 2
ExperienceInt playerXp = new ExperienceInt(1, 0, 100);

public void OnKillMonster()
{
    // Add 150 XP. Returns true if leveled up.
    // Logic handles overflow: 150 XP -> Level 2 (uses 100), 50 XP remaining.
    if (playerXp.Add(150)) {
        // Update target for next level (e.g., linear growth)
        playerXp.TargetForNextLevel = playerXp.Level * 100;
        PlayLevelUpEffect();
    }
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
