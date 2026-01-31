# 🎢 Variable.Curve

### Zero-Allocation Easing Functions for C#

[![NuGet](https://img.shields.io/nuget/v/Variable.Curve?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Curve)

**The "Standard Library" for Interpolation.**
Stop writing custom lerp functions and `Math.Sin` calls.

---

## 🧠 The Mental Model: "The Rollercoaster"

Think of `CurveFloat` as a rollercoaster ride.
*   **Start**: The station.
*   **End**: The exit.
*   **Duration**: How long the ride lasts.
*   **EaseType**: The shape of the track (Slow start? Fast loop? Bouncy landing?).

You just push the rollercoaster forward (`Tick`), and it tells you exactly how high you are off the ground (`GetValue`).

---

## 🚀 Quick Start

### Installation

```bash
dotnet add package Variable.Curve
```

### Usage

```csharp
using Variable.Curve;

// 1. Create a curve: 0 to 100 over 2 seconds, starting slow then accelerating (InQuad)
var jump = new CurveFloat(2f, 0f, 100f, EaseType.InQuad);

// 2. In your update loop...
void Update(float dt)
{
    // Advance time
    jump.Tick(dt);

    // Get the current value
    float height = jump.GetValue();

    // Check if finished
    if (jump.IsFinished())
    {
        Console.WriteLine("Landed!");
    }
}
```

### Easing Types

We support all standard Robert Penner easing functions:
*   **Linear**: Constant speed.
*   **Quad, Cubic, Quart, Quint, Expo**: Smooth acceleration/deceleration curves.
*   **Sine**: Gentle sinusoidal movement.
*   **Circ**: Circular arc.
*   **Back**: Overshoots and returns (like a bowstring).
*   **Elastic**: Wobbles like jelly.
*   **Bounce**: Bounces like a ball.

Each comes in **In** (start), **Out** (end), and **InOut** (both) variants.

---

## 🔧 Advanced

### Zero Allocation
`CurveFloat` is a `struct`. `Tick` and `GetValue` do not allocate memory. Perfect for Unity `Update` loops and DOTS.

### Stateless Logic
You can use the math directly without the struct if you prefer:

```csharp
// Just want the math?
CurveLogic.Evaluate(in t, EaseType.OutBounce, out float result);
```
