# 🎭 Variable.Motion

**The Animator & The Spring.** 🚀
Zero-allocation interpolation and physics-based movement for game feel.

---

### 🧠 Mental Model

1.  **TweenFloat ("The Elastic Band"):** You stretch an elastic band from A to B. It snaps back over time. You know exactly where it is at any moment.
2.  **SpringFloat ("The Shock Absorber"):** You attach a heavy weight to a spring. When you move the handle (Target), the weight (Current) bounces and settles smoothly.

### 👶 ELI5

*   **Tween:** "Slide the coin from here to there in 2 seconds."
*   **Spring:** "Pull the coin towards the magnet. It might wiggle a bit before stopping."

---

### 🛠️ Usage

#### 1. TweenFloat (Simple Interpolation)

Perfect for UI transitions, fading lights, or moving platforms.

```csharp
using Variable.Motion;

public struct MyUiPanel
{
    public TweenFloat Scale;

    public void Open()
    {
        // Scale from 0 to 1 over 0.5 seconds with BounceOut
        Scale = new TweenFloat(0f, 1f, 0.5f, EasingType.BounceOut);
    }

    public void Update(float dt)
    {
        if (!Scale.IsComplete)
        {
            Scale.Tick(dt);
            transform.localScale = Vector3.one * Scale.Value;
        }
    }
}
```

#### 2. SpringFloat (Juicy Physics)

Perfect for camera followers, "squash and stretch", or responsive UI.

```csharp
using Variable.Motion;

public struct CameraFollower
{
    // Follows the player X position
    public SpringFloat CameraX;

    public CameraFollower(float startX)
    {
        // Frequency 5Hz, Damping 0.6 (slightly bouncy)
        CameraX = SpringFloat.FromFrequency(startX, startX, 5f, 0.6f);
    }

    public void Update(float dt, float playerX)
    {
        // Update target
        CameraX.Target = playerX;

        // Tick physics
        CameraX.Tick(dt);

        // Apply position
        transform.position = new Vector3(CameraX.Current, transform.position.y, -10f);
    }
}
```

---

### 🔧 Features

*   **Zero Allocation:** Everything is a `struct`. No GC pressure.
*   **30+ Easing Functions:** `Quad`, `Cubic`, `Elastic`, `Bounce`, `Back`, etc.
*   **Real Physics:** Springs use semi-implicit Euler integration for stability.
*   **Dynamic:** Change `SpringFloat.Target` or `TweenFloat.End` on the fly.
