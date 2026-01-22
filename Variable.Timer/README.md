# ⏱️ Variable.Timer

**The "Egg Timer" of your game.** 🥚⏰

**Variable.Timer** solves the "Wait for X seconds" problem forever. It gives you two perfect structs: `Cooldown` (counts down) and `Timer` (counts up).

---

## 🧠 Mental Model

### 1. `Cooldown` (The Microwave) 📉
- You set it for 30 seconds.
- It counts **DOWN** to 0.
- When it hits 0, it goes "DING!" (Ready).
- **Use for:** Spells, dashes, gun reloads.

### 2. `Timer` (The Stopwatch) 📈
- You start at 0.
- It counts **UP** to a target.
- When it hits the target, it stops.
- **Use for:** Casting bars, holding a button, bomb fuses.

---

## 👶 ELI5 (Explain Like I'm 5)

Without Variable.Timer:
> **You:** `timer -= Time.deltaTime;`
> **You:** "Wait, did I reset it? Is it less than 0? Or less than or equal to 0?"
> **You:** *Writes the same bug for the 50th time.*

With Variable.Timer:
> **You:** `cooldown.Tick(dt);`
> **You:** `if (cooldown.IsReady()) Attack();`
> **Computer:** "I got you."

---

## 📦 Installation

```bash
dotnet add package Variable.Timer
```

---

## 🎮 Usage Guide

### 1. The Cooldown (Counts DOWN)

```csharp
using Variable.Timer;

public class Hero
{
    // 3 second cooldown. Starts READY.
    public Cooldown DashCd = new Cooldown(3f);

    public void Update(float dt)
    {
        // 1. Tick it every frame
        DashCd.Tick(dt);

        // 2. Check it
        if (Input.GetButton("Dash") && DashCd.IsReady())
        {
            DoDash();

            // 3. Reset it (Start waiting again)
            DashCd.Reset();
        }
    }
}
```

### 2. The Cast Timer (Counts UP)

```csharp
// 2 second cast time. Starts at 0.
public Timer FireballCast = new Timer(2f);

public void Update(float dt)
{
    if (Input.GetButton("Fire"))
    {
        // Tick and check completion in one line!
        // Returns true ONLY on the frame it finishes.
        if (FireballCast.TickAndCheckComplete(dt))
        {
            SpawnFireball();
            FireballCast.Reset();
        }
    }
    else
    {
        // Cancel cast if button released
        FireballCast.Reset();
    }
}
```

### 3. UI Progress Bars

Both types implement `IBoundedInfo`, so they work with your generic UI bars!

```csharp
// 0.0 to 1.0 (Empty to Full)
float castProgress = (float)FireballCast.GetRatio();

// 1.0 to 0.0 (Full to Empty)
float cooldownProgress = (float)DashCd.GetRatio();
```

---

## ⚡ Performance Secrets

### `TickAndCheck` Optimization
Checking `if (timer >= duration)` every frame is fine. But often you want to do something **only once** when it finishes.

`TickAndCheckComplete(dt)` does this efficiently:
1. Adds time.
2. Checks if it *crossed the finish line* this specific frame.
3. Returns `true` only once per cycle.

### Zero Allocation
These are pure structs. Thousands of bullets can have their own timers with zero GC pressure.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
