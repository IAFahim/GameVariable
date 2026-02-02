# ⏱️ Variable.Timer

**Time management for games, minus the headaches.** 🤯

**Variable.Timer** provides two zero-allocation structs: `Timer` (counts UP) and `Cooldown` (counts DOWN). They handle all the `deltaTime` math, state checking, and "is it ready yet?" logic for you.

---

## 🧠 Mental Model

### 1. `Timer` = "The Stopwatch" 🏃
- Starts at 0.
- Counts **UP** to a Target Duration.
- **Complete** when it hits the Target.
- *Use for: Casting bars, fuse timers, cinematics.*

### 2. `Cooldown` = "The Egg Timer" 🥚
- Starts at Duration (or 0 if ready).
- Counts **DOWN** to 0.
- **Ready** when it hits 0.
- *Use for: Ability cooldowns, fire rates, invulnerability frames.*

## 👶 ELI5: "Are we there yet?"

- **Timer:** "We are driving to Disney World. We have driven 2 hours (Current) out of 4 hours (Duration). Are we there yet? No."
- **Cooldown:** "You can't eat another cookie for 10 minutes. 9... 8... 7... (Current). When it hits 0, you can eat a cookie (Ready)."

---

## 📦 Installation

```bash
dotnet add package Variable.Timer
```

---

## 🛠️ Usage Guide

### 1. The `Cooldown` (Abilities, Guns)

Most common use case. Note: `IsReady()` checks if `Current <= 0`.

```csharp
using Variable.Timer;

public struct Hero
{
    public Cooldown DashCooldown;

    public Hero()
    {
        // 3 second cooldown. Starts Ready (0).
        DashCooldown = new Cooldown(3f);
    }

    public void Update(float dt)
    {
        // Ticks down. Returns TRUE if ready (<= 0).
        // Does NOT auto-reset. You decide when to use it.
        DashCooldown.Tick(dt);
    }

    public void TryDash()
    {
        if (DashCooldown.IsReady())
        {
            DoDash();
            DashCooldown.Reset(); // Sets Current = Duration (3f)
        }
    }
}
```

### 2. The `Timer` (Casting, Loading)

Counts up.

```csharp
var castingBar = new Timer(2.5f); // 2.5 sec cast

void Update(float dt)
{
    // TickAndCheckComplete returns TRUE if Current >= Duration
    if (castingBar.TickAndCheckComplete(dt))
    {
        CastSpell();
        castingBar.Reset(); // Back to 0
    }
}
```

### 3. One-Liner Checks (Functional Style)

You can tick and check in the same line.

```csharp
// Fire Rate Limiter
// TickAndCheckReady returns true ONLY if it is ready (<= 0)
// It does NOT reset automatically.
if (fireRate.TickAndCheckReady(deltaTime) && Input.IsFiring)
{
    Shoot();
    fireRate.Reset();
}
```

### 4. Overflow Handling (Advanced) 🌊

For precision games (rhythm games, high-speed shooters), you don't want to lose time between frames.
If a 1.0s timer finishes with `dt` overflow of 0.1s, the next timer should start at 0.1s, not 0.0s.

```csharp
// Returns true if complete, AND gives you the extra time
if (timer.TickAndCheckComplete(dt, out float overflow))
{
    NextTimer.Reset(overflow); // Start next timer with head start!
}
```

---

## 🔧 API Reference

### `Cooldown`
- **`Current`**: Time remaining (0 = Ready).
- **`Duration`**: Total wait time.
- **`Reset()`**: Sets `Current = Duration`.
- **`Finish()`**: Sets `Current = 0` (Ready immediately).
- **`IsReady()`**: Returns `true` if `Current <= 0`.
- **`GetRatio()`**: Returns 0.0 to 1.0 (Progress towards 0).

### `Timer`
- **`Current`**: Time elapsed.
- **`Duration`**: Target time.
- **`Reset()`**: Sets `Current = 0`.
- **`Finish()`**: Sets `Current = Duration` (Complete immediately).
- **`IsComplete()`**: Returns `true` if `Current >= Duration`.
- **`GetRatio()`**: Returns 0.0 to 1.0 (Progress towards Duration).

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
