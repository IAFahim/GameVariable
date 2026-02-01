# ⏱️ Variable.Timer

**The Egg Timer.** 🥚

**Variable.Timer** provides dedicated structs for counting Time. Stop using loose `float` variables that you forget to decrement.

---

## 🧠 Mental Model: The Egg Timer

Think of `Cooldown` as a kitchen **Egg Timer**.
1.  You twist it to 5 minutes (`Reset()`).
2.  It ticks down automatically (`Tick()`).
3.  When it hits 0, it DINGS! (`IsReady()`).

Think of `Timer` as a **Stopwatch**.
1.  You start at 0.
2.  It ticks up.
3.  When it hits the target, it stops.

---

## 👶 ELI5: "Is it ready yet?"

Imagine a kid in the back seat asking "Are we there yet?"
*   **Timer:** "We have driven 1 hour out of 5." (Counts Up)
*   **Cooldown:** "We have 4 hours left." (Counts Down)

Both answer the same question: "When does this end?"

---

## 📦 Installation

```bash
dotnet add package Variable.Timer
```

---

## 🚀 Usage Guide

### 1. Cooldowns (Abilities)

Perfect for Dashes, Spells, or Fire Rates.

```csharp
using Variable.Timer;

public class Hero
{
    // 3 second cooldown. Starts READY (0).
    public Cooldown DashCd = new Cooldown(3f);

    public void Update(float dt)
    {
        // Decrements time. Returns TRUE the exact frame it hits 0.
        if (DashCd.TickAndCheckReady(dt))
        {
            // Just became ready!
            Ui.FlashGreen();
        }

        if (Input.Pressed && DashCd.IsReady())
        {
            Dash();
            DashCd.Reset(); // Sets time back to 3.0
        }
    }
}
```

### 2. Timers (Casting/Loading)

Perfect for "Hold to interact" or "Casting Fireball".

```csharp
// 2 second cast time. Starts at 0.
public Timer CastTimer = new Timer(2f);

public void Update(float dt)
{
    // Increments time. Returns TRUE when it hits 2.0.
    if (CastTimer.TickAndCheckComplete(dt))
    {
        Fireball();
        CastTimer.Reset(); // Back to 0
    }
}
```

### 3. Visuals (UI)

Since they are `IBoundedInfo`, they plug right into your standard UI bars.

```csharp
// Returns 0.0 -> 1.0 (Empty -> Full)
float fill = (float)CastTimer.GetRatio();

// Returns 1.0 -> 0.0 (Full -> Empty)
// Perfect for darkening an icon!
float darkOverlay = (float)DashCd.GetRatio();
```

---

## 🔧 API Reference

### `Cooldown` (Counts DOWN 📉)
- `Reset()`: Set to `Duration`.
- `Finish()`: Set to `0`.
- `IsReady()`: Is `Current <= 0`?
- `Tick(dt)`: `Current -= dt`.

### `Timer` (Counts UP 📈)
- `Reset()`: Set to `0`.
- `Finish()`: Set to `Duration`.
- `IsComplete()`: Is `Current >= Duration`?
- `Tick(dt)`: `Current += dt`.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
