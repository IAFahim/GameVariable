# 🔋 Variable.Reservoir

**Magazines, Tanks, and Batteries.** 🔫

**Variable.Reservoir** manages the classic "Active + Reserve" pattern found in almost every game.
- **Active (Volume):** What's in the clip/tank right now.
- **Reserve:** What's in your backpack/stockpile.

---

## 🧠 Mental Model: The Clip & Backpack 🎒

Think of a **Shooter Game**.
*   **Volume:** The Ammo in your **Gun's Clip** (Active). You shoot from here.
*   **Reserve:** The Ammo in your **Backpack** (Storage). You can't shoot from here.
*   **Refill:** You take ammo from the Backpack to fill the Clip.

**Variable.Reservoir** handles this 2-stage resource system effortlessly.

---

## 📦 Installation

```bash
dotnet add package Variable.Reservoir
```

---

## 🚀 Features

* **🔫 Reload Logic:** Built-in `Refill()` handles the math of moving from Reserve to Volume.
* **🔢 Int & Float:** `ReservoirInt` (Ammo) and `ReservoirFloat` (Fuel/Energy).
* **🛡️ Safe Limits:** Can't reload more than capacity, can't take more than you have.

---

## 🎮 Usage Guide

### 1. Weapon Ammo (Integers)

```csharp
using Variable.Reservoir;

public class Gun
{
    // Clip: 30, Current: 30, Backpack: 120
    public ReservoirInt Ammo = new ReservoirInt(30, 30, 120);

    public void Fire()
    {
        if (Ammo > 0) // Implicit conversion to current volume!
        {
            Ammo.Volume--;
            SpawnBullet();
        }
        else
        {
            Reload();
        }
    }

    public void Reload()
    {
        // Moves ammo from Reserve to Volume
        // Returns amount actually moved (e.g. 5 bullets)
        int reloaded = Ammo.Refill();

        if (reloaded > 0) PlayReloadSound();
        else PlayClickSound(); // No ammo left!
    }
}
```

### 2. Jetpack Fuel (Floats)

```csharp
// Tank: 50.0, Current: 50.0, Reserve Tank: 200.0
public ReservoirFloat Fuel = new ReservoirFloat(50f, 50f, 200f);

public void Fly(float dt)
{
    if (Fuel.Volume.TryConsume(10f * dt))
    {
        ApplyThrust();
    }
    else
    {
        // Auto-refill from reserve tank?
        Fuel.Refill();
    }
}
```

---

## 🔧 API Reference

### `ReservoirInt` / `ReservoirFloat`
- `Volume`: The active `Bounded` value (Clip/Tank).
- `Reserve`: The available backup supply.

### Extensions
- `Refill()`: Fills `Volume` to Max using `Reserve`. Decreases Reserve. Returns amount moved.
- `Refill(amount)`: Tries to move specific `amount` from Reserve to Volume.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
