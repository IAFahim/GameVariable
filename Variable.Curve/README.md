# 🎢 Variable.Curve

### Zero-Allocation Easing Functions

**The "Rollercoaster" of GameVariable.**
Make your numbers move with style.

---

## 🧠 The Mental Model: "The Rollercoaster"

Imagine a rollercoaster.
*   It doesn't start at full speed (that would kill you). It climbs slowly (`In`).
*   It doesn't stop instantly (that would also kill you). It brakes smoothly (`Out`).
*   Sometimes it bounces at the end (`Bounce`).

**Variable.Curve** is the physics of that rollercoaster applied to your numbers.

---

## 👶 ELI5 (Explain Like I'm 5)

If you move a car from A to B:
*   **Linear:** The car teleports to full speed, hits a wall at B, and stops instantly. 🤖
*   **EaseInOut:** The car accelerates, drives, and brakes gently. 🚗

We help you drive the car, not the robot.

---

## 🚀 Installation

```bash
dotnet add package Variable.Curve
```

---

## 🛠 Usage

### Basic Easing

Just use the `.Ease()` extension on any float!
The input should be a value between `0` (start) and `1` (end).

```csharp
using Variable.Curve;

float progress = 0.5f; // Halfway through time

// Get the eased value
float smoothVal = progress.Ease(EaseType.InOutQuad);
// Result: 0.5 (Quad is symmetrical at 0.5)

float fastStart = 0.1f;
float val = fastStart.Ease(EaseType.OutExpo);
// Result: Much higher than 0.1 because it starts fast!
```

### The Types

| Ease Type | Description |
|-----------|-------------|
| **Linear** | 😐 Boring. Straight line. |
| **Quad** | 🐢 Simple curve. |
| **Cubic** | 🐇 Steeper curve. |
| **Expo** | 🚀 Very steep curve. |
| **Elastic** | 橡 Rubber band effect. |
| **Bounce** | 🏀 Bouncing ball effect. |
| **Back** | 🔙 Pulls back before shooting forward. |

Each comes in `In`, `Out`, and `InOut` flavors.
