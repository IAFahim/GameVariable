# ⏱️ Variable.Timer

**Time management for games, solved.** ⌛

**Variable.Timer** gives you `Timer` (count up) and `Cooldown` (count down) structs that just work. No more spaghetti `float timer = 0f;` and `timer += Time.deltaTime;` checks scattered everywhere.

---

## 📦 Installation

```bash
dotnet add package Variable.Timer
```

---

## 🚀 Features

* **🔥 Cooldowns:** Manage ability availability effortlessly.
* **⏳ Timers:** Track durations (casting, loading, buffs).
* **🧠 Logic Separation:** Tick logic is separated from state.
* **⚡ Zero Allocation:** Pure structs, Burst compatible.

---

## 🎮 Usage Guide

### 1. Cooldowns (The "Can I do this yet?" Pattern)

```csharp
using Variable.Timer;

public class Hero
{
    // 3 second cooldown, starts READY
    public Cooldown DashCd = new Cooldown(3f);

    public void Update(float dt)
    {
        // 1. Tick the cooldown
        // TickAndCheckReady returns true if it hits 0 THIS FRAME
        if (DashCd.TickAndCheckReady(dt))
        {
            PlayReadySound();
        }

        // 2. Use Ability
        if (Input.GetButton("Dash") && DashCd.IsReady())
        {
            DoDash();
            DashCd.Reset(); // Sets value to 3.0
        }
    }
}
```

### 2. Timers (The "Are we there yet?" Pattern)

```csharp
// 5 second cast time
public Timer CastTimer = new Timer(5f);

public void Update(float dt)
{
    // TickAndCheckComplete returns true if it hits Duration THIS FRAME
    if (CastTimer.TickAndCheckComplete(dt))
    {
        SpawnFireball();
        CastTimer.Reset(); // Sets value to 0.0
    }
}
```

### 3. Progress Bars (UI)

Since `Timer` and `Cooldown` implement `IBoundedInfo`, they work with standard UI code!

```csharp
// 0.0 to 1.0
float progress = (float)CastTimer.GetRatio();

// 1.0 to 0.0 (inverse for cooldowns usually)
float cdProgress = (float)DashCd.GetProgress();
```

---

## 🔧 API Reference

### `Cooldown` (Counts DOWN 📉)
- `Reset()`: Sets current time to Duration.
- `Finish()`: Sets current time to 0 (Ready).
- `IsReady()`: True if time <= 0.
- `TickAndCheckReady(dt)`: Advances time, returns true if it *just* became ready.

### `Timer` (Counts UP 📈)
- `Reset()`: Sets current time to 0.
- `Finish()`: Sets current time to Duration.
- `IsFull()`: True if time >= Duration.
- `TickAndCheckComplete(dt)`: Advances time, returns true if it *just* finished.

### Common
- `Current`: The raw time value.
- `Duration`: The target time.
- `GetRatio()`: Normalized progress.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
