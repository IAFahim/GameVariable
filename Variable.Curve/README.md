# 📉 Variable.Curve

**Zero-allocation easing functions for C#.**

---

## 🧠 The Mental Model: Time → Value

Animation is simply a function that takes **Time** (0 to 1) and returns a **Value** (0 to 1).
That's it.

`Variable.Curve` provides the standard "Penner Easing Functions" as a pure, static library.

---

## 📦 Installation

```bash
dotnet add package Variable.Curve
```

---

## 🚀 Usage

### 1. Basic Easing

```csharp
using Variable.Curve;

float t = 0.5f; // Halfway through time

// "Ease In Quad" starts slow, ends fast
float val = t.Ease(EaseType.InQuad);

// Result: 0.25f
```

### 2. The "Tween" Recipe (Timer + Curve)

Combine `Variable.Timer` (Time) with `Variable.Curve` (Shape) to make a Tween.

```csharp
using Variable.Timer;
using Variable.Curve;

public struct Tween
{
    public Timer Time;
    public float StartValue;
    public float EndValue;
    public EaseType Ease;

    public float Evaluate()
    {
        // 1. Get Normalized Time (0 to 1)
        // Timer.Current / Timer.Duration
        float t = Time.GetRatio();

        // 2. Apply Easing (0 to 1 -> 0 to 1 curved)
        float curvedT = t.Ease(Ease);

        // 3. Lerp Value
        return StartValue + (EndValue - StartValue) * curvedT;
    }
}
```

---

## 📊 Supported Types

All standard Penner easings are supported (In, Out, InOut):

*   **Linear**
*   **Quad** (t²)
*   **Cubic** (t³)
*   **Quart** (t⁴)
*   **Quint** (t⁵)
*   **Sine**
*   **Expo**
*   **Circ**
*   **Back** (Overshoot)
*   **Elastic**
*   **Bounce**

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**

</div>
