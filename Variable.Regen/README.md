# Variable.Regen

**Variable.Regen** adds time-based regeneration and decay mechanics to your variables. It wraps a bounded value with a rate of change per second.

## Installation

```bash
dotnet add package Variable.Regen
```

## Features

*   **RegenFloat**: A `BoundedFloat` + `Rate`.
*   **Tick(deltaTime)**: Automatically updates the value based on the rate.
*   **Decay Support**: Negative rates handle decay (poison, hunger).

## Usage

### Mana Regeneration
```csharp
using Variable.Regen;

// Max 100, Current 0, Regens 5 per second
RegenFloat mana = new RegenFloat(100f, 0f, 5f);

void Update(float dt) {
    mana.Tick(dt);
}
```

### Hunger Decay
```csharp
// Max 100, Current 100, Decays 1 per second
RegenFloat hunger = new RegenFloat(100f, 100f, -1f);

void Update(float dt) {
    hunger.Tick(dt);
    if (hunger.Value.IsEmpty) Starve();
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
