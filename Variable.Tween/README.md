# 〰️ Variable.Tween

### High-Performance Struct-Based Tweening

**The "Rubber Band" for your values.** 🪁
Interpolate numbers over time with zero allocations. No classes, no callbacks, just raw math.

## 🧠 Mental Model

Imagine a **Rubber Band**.
*   You stretch it from **Start** to **End**.
*   It takes **Time** to snap back or move.
*   You can control *how* it moves (Linear, Bouncy, Elastic) by changing the material of the band.
*   It is just a small pile of numbers in your pocket (Stack), not a heavy machine (Heap).

## 👶 ELI5 (Explain Like I'm 5)

*   You want to slide a cookie from the jar to your mouth.
*   It takes 2 seconds.
*   `Variable.Tween` calculates exactly where the cookie is at 0.5 seconds, 1 second, 1.9 seconds...
*   It can move steadily (Linear) or start slow and zoom in (EaseIn).

## 🚀 Installation

```bash
dotnet add package Variable.Tween
```

## 🛠️ Usage

### 1. Basic Tween
```csharp
using Variable.Tween;

// Create a tween from 0 to 10 over 2 seconds with QuadOut easing
var tween = new TweenFloat(0f, 10f, 2f, EasingType.QuadOut);

void Update(float dt)
{
    // Update the tween
    tween.Tick(dt);

    // Use the value
    transform.position = new Vector3(tween.CurrentValue, 0, 0);
}
```

### 2. Checking Completion
```csharp
if (tween.IsComplete())
{
    Console.WriteLine("Arrived!");

    // Restart?
    tween.Reset();
}
```

### 3. Immediate Completion
```csharp
// Teleport to the end
tween.Complete();
// CurrentValue is now 10f
```
