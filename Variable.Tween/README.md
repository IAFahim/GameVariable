# Variable.Tween

**Variable.Tween** provides zero-allocation tweening structs for the GameVariable ecosystem.
It allows for interpolation between values over time using various easing functions, without generating garbage.

## Installation

```bash
dotnet add package Variable.Tween
```

## Features

* **TweenFloat**: Struct for tweening float values.
* **EasingType**: Enum with standard easing functions (Linear, Quad, Cubic, Sine, Back, Elastic, Bounce, etc.).
* **TweenExtensions**: Helper methods for ticking and checking completion.

## Usage

### Basic Tween

```csharp
using Variable.Tween;

// Create a tween from 0 to 100 over 2 seconds
var tween = new TweenFloat(0f, 100f, 2f, EasingType.QuadOut);

void Update(float dt)
{
    // Tick the tween
    if (tween.Tick(dt))
    {
        Console.WriteLine("Tween Complete!");
    }

    // Use the current value
    transform.position = new Vector3(tween.Current, 0, 0);
}
```

### Resetting

```csharp
tween.Reset(); // Restarts the tween from the beginning
```

### Checking Status

```csharp
if (tween.IsComplete())
{
    // Do something
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
