# Variable.Timer

**Variable.Timer** provides robust structs for handling time-based mechanics like cooldowns, countdowns, and
stopwatches. It simplifies `Time.deltaTime` management.

## Installation

```bash
dotnet add package Variable.Timer
```

## Features

* **Timer**: A simple countdown timer.
* **Cooldown**: A specialized timer for ability availability.
* **State Queries**: `IsReady`, `IsFinished`, `TimeRemaining`, `Progress` (0-1).

## Usage

### Ability Cooldown

```csharp
using Variable.Timer;

public class Ability
{
    public Cooldown DashCooldown = new Cooldown(5f); // 5 seconds

    public void Update(float deltaTime)
    {
        // TickAndCheckReady returns true when cooldown hits zero
        if (DashCooldown.TickAndCheckReady(deltaTime) && Input.GetKey(KeyCode.Space))
        {
            Cast();
            DashCooldown.Reset(); // Start cooldown again
        }
    }

    private void Cast()
    {
        // Perform ability
        Debug.Log("Ability Cast!");
    }
}
```

### Match Timer

```csharp
Timer matchTime = new Timer(300f); // 5 minutes

void Update(float dt) {
    // TickAndCheckComplete returns true when timer finishes
    if (matchTime.TickAndCheckComplete(dt)) {
        EndMatch();
    }
    
    Console.WriteLine($"Time Left: {matchTime.Duration - matchTime.Current}");
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
