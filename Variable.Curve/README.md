# 〰️ Variable.Curve

**Smooth moves, bouncy UI, and juicy animations.** 🍎

**Variable.Curve** gives you the classic easing equations (Robert Penner's) in a **zero-allocation**, **pure math** library. No objects, no garbage, just raw speed.

---

## 🧠 The Mental Model: The Rollercoaster 🎢

Imagine moving an object from Point A to Point B.
*   **Linear:** A boring conveyor belt. Constant speed. 😐
*   **EaseIn:** A slide! Starts slow, gets fast. ⛷️
*   **EaseOut:** A car braking. Starts fast, stops smooth. 🚗
*   **Elastic:** A rubber band. Snaps past the target and wiggles back. 🪀

**Variable.Curve** is the math engine that defines these shapes.

---

## 👶 ELI5 (Explain Like I'm 5)

If you tell a robot to walk to the wall:
*   **Without Curves:** It walks at full speed and SMACKS into the wall. 💥
*   **With Variable.Curve:** It walks fast, then slows down gently before touching the wall. 🤫

---

## 📦 Installation

```bash
dotnet add package Variable.Curve
```

---

## 🚀 Features

*   **Zero Allocation:** Pure static methods. No `new` keywords.
*   **Burst Compatible:** Uses only primitives (`float`).
*   **30+ Curves:** All the standards (Quad, Cubic, Expo, Elastic, Bounce, Back).
*   **Extension Method:** `myFloat.Ease(EaseType.OutQuad)` syntax.

---

## 🎮 Usage Guide

### 1. The Basics

You have a value `t` going from 0 to 1 (like a progress bar).

```csharp
using Variable.Curve;

float t = 0.5f; // Halfway through time

// Apply a curve!
float curvedT = t.Ease(EaseType.OutQuad);

// Use it to lerp your position
transform.position = Vector3.Lerp(start, end, curvedT);
```

### 2. Available Curves

The `EaseType` enum has everything you need:

| Family | Feel | Best For |
|--------|------|----------|
| **Linear** | Mechanical, robotic | Scrolling backgrounds, timers |
| **Quad / Cubic** | Natural acceleration | Character movement, fading |
| **Expo / Circ** | Sharp, snappy | UI pop-ups, critical hits |
| **Back** | Anticipation (pull back then shoot) | Button presses, jump prep |
| **Elastic** | Wobbly, jelly-like | Slimes, spring traps |
| **Bounce** | Bouncing ball | Loot drops, gravity effects |

### 3. High Performance (Layer B)

If you are in a tight loop (like a Unity Job) and don't want extension methods, use the raw logic:

```csharp
using Variable.Curve;

// Raw calculation (Aggressive Inlining)
CurveLogic.Evaluate(in t, in EaseType.InBounce, out float result);
```

---

## ⚠️ Important Notes

*   **Inputs:** `t` is usually expected to be `0` to `1`.
*   **Overshoot:** Curves like `Back` and `Elastic` will output values **outside** `0` to `1` (e.g., -0.5 or 1.2). This is intentional! It creates the "overshoot" effect.
    *   *Warning:* If you use this with `Color.Lerp`, standard Unity colors clamp, but `Vector3.Lerp` does not (which is good!).

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
