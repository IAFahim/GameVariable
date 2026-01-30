<div align="center">

# 🎢 Variable.Curve

### Standard Easing Functions & Bezier Utilities

[![NuGet](https://img.shields.io/nuget/v/Variable.Curve?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Curve)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](../LICENSE)
[![.NET](https://img.shields.io/badge/.NET-Standard%202.1+-purple.svg)](https://dotnet.microsoft.com/)

**The "Rollercoaster" of Game Mechanics.** 📉
Smooth transitions, bouncy animations, and beautiful curves without the math headache.

</div>

---

## 🧠 Mental Model

Think of **Variable.Curve** as a **set of tracks** for your values.
Instead of moving linearly from A to B like a robot, your values can accelerate, decelerate, bounce, or follow complex paths.

- **Easing:** Predetermined tracks (Sine, Quad, Bounce) for polished feel.
- **Bezier:** Custom tracks defined by control points.

---

## 👶 ELI5

Imagine sliding down a slide.
- **Linear:** You slide at the exact same speed the whole way down. Boring.
- **Ease In:** You start slow and get faster (like gravity!).
- **Bounce:** You hit the bottom and bounce up a bit before settling. Fun!

**Variable.Curve** gives you the math to make your numbers move in these fun ways.

---

## 🛠️ Installation

```bash
dotnet add package Variable.Curve
```

---

## 🚀 Usage

### 1. Basic Easing

Apply easing to any normalized value (0 to 1).

```csharp
using Variable.Curve;

float t = 0.5f; // Halfway through animation
float eased = t.Ease(EasingType.OutBounce);
// Result: A bouncy value > 0.5f
```

### 2. Manual Easing

Use the static methods directly if you prefer.

```csharp
float val = EasingLogic.OutQuad(0.5f);
```

### 3. Bezier Curves

Evaluate points along a curve.

```csharp
// Quadratic Bezier (Start, Control, End)
float p0 = 0f;
float p1 = 2f;
float p2 = 0f;

float mid = BezierLogic.Evaluate(0.5f, p0, p1, p2);
// Result: 1.0f (The peak of the curve)
```

---

## 📦 Features

- **Standard Easings:** Linear, Quad, Cubic, Quart, Quint, Sine, Expo, Circ, Back, Elastic, Bounce.
- **Variations:** In, Out, InOut for all types.
- **Bezier:** Quadratic (3 points) and Cubic (4 points) evaluation.
- **Extensions:** Clean `.Ease()` syntax for float.
- **Zero Allocation:** Pure static math. burst-compatible.
