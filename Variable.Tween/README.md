# 📉 Variable.Tween

**Zero-allocation tweening library for the GameVariable ecosystem.**

`Variable.Tween` provides a high-performance, struct-based tweening system that generates no garbage and is compatible with Unity's Burst compiler. It separates data, logic, and extensions to ensure pure Data-Oriented Design.

## 📦 Installation

```bash
dotnet add package Variable.Tween
```

## 🚀 Usage

### Basic Linear Tween

```csharp
using Variable.Tween;

// Create a tween from 0 to 100 over 2 seconds
var tween = new TweenFloat(0f, 100f, 2f);

// In your update loop
tween.Tick(deltaTime);

Console.WriteLine(tween.Current); // Interpolated value

if (tween.IsComplete())
{
    Console.WriteLine("Finished!");
}
```

### Using Easing Functions

```csharp
// Bounce effect
var bounce = new TweenFloat(0f, 10f, 1f, EasingType.BounceOut);

bounce.Tick(deltaTime);
transform.position = new Vector3(0, bounce.Current, 0);
```

### Reusing Tweens

```csharp
// Reset to start
tween.Reset();

// Play with new values (no allocation)
tween.Play(100f, 0f, 0.5f, EasingType.QuadIn);
```

## 🏗️ Architecture

| Layer | Type | Description |
|-------|------|-------------|
| **Data** | `TweenFloat` | Pure struct holding Start, End, Current, Duration, Elapsed, Easing. |
| **Logic** | `TweenLogic` | Static class containing pure math and easing algorithms (Linear, Quad, Cubic, Elastic, Bounce, etc.). |
| **Extensions** | `TweenExtensions` | Extension methods for `Tick`, `Play`, `Reset`. |

## 📈 Supported Easings

- Linear
- Quad (In, Out, InOut)
- Cubic (In, Out, InOut)
- Quart (In, Out, InOut)
- Quint (In, Out, InOut)
- Sine (In, Out, InOut)
- Expo (In, Out, InOut)
- Circ (In, Out, InOut)
- Elastic (In, Out, InOut)
- Back (In, Out, InOut)
- Bounce (In, Out, InOut)

## 🔧 Zero Allocation

`Variable.Tween` is designed to be used in hot paths. `TweenFloat` is a struct, so it lives on the stack or embedded in other structs/arrays. The logic is static and stateless.

```csharp
// Perfect for ECS Components
public struct MoveComponent : IComponentData
{
    public TweenFloat PositionTween;
}
```
