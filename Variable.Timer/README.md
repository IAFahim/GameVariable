# Variable.Timer

**Variable.Timer** provides robust structs for handling time-based mechanics like cooldowns, countdowns, and stopwatches. It simplifies `Time.deltaTime` management.

## Installation

```bash
dotnet add package Variable.Timer
```

## Features

*   **Timer**: A simple countdown timer.
*   **Cooldown**: A specialized timer for ability availability.
*   **State Queries**: `IsReady`, `IsFinished`, `TimeRemaining`, `Progress` (0-1).

## Usage

### Ability Cooldown
```csharp
using Variable.Timer;

public class Ability
{
    public Cooldown Cooldown = new Cooldown(5f); // 5 seconds

    public void Update(float deltaTime)
    {
        Cooldown.Tick(deltaTime);
    }

    public void Cast()
    {
        if (Cooldown.IsReady) {
            // Perform ability
            Cooldown.Restart();
        }
    }
}
```

### Match Timer
```csharp
Timer matchTimer = new Timer(300f); // 5 minutes

void Update(float dt) {
    matchTimer.Tick(dt);
    if (matchTimer.IsFinished) EndMatch();
    
    Console.WriteLine($"Time Left: {matchTimer.CurrentTime}");
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
