# 🔋 Variable.Reservoir

**The Clip & Backpack.** 🎒

**Variable.Reservoir** solves the "Reloading" problem. It manages two connected numbers: what you have *ready* (Volume) and what you have *stored* (Reserve).

---

## 🧠 Mental Model: The Magazine (Clip)

Think of a **Gun Magazine**.
*   **Volume (Active):** The bullets in the gun (Bounded, 0 to 30).
*   **Reserve (Stored):** The bullets in your backpack (Unbounded).

When you **Reload (`Refill`)**, you take bullets from the Backpack and put them in the Gun.
*   You can't overfill the gun (Bounded Max).
*   You can't take more than you have (Reserve).

This also applies to **Fuel Tanks** (Main Tank + Reserve Tank) or **Batteries**.

---

## 👶 ELI5: "I need to reload!"

If you have 5 bullets in your gun and 100 in your pocket...
And you reload...
You now have 30 in your gun (Max) and 75 in your pocket.

Writing the math for this is annoying: `if (reserve < needed) ... else ...`
**Variable.Reservoir** does it in one line: `ammo.Refill()`.

---

## 📦 Installation

```bash
dotnet add package Variable.Reservoir
```

---

## 🚀 Usage Guide

### 1. The Shooter (Ammo)

```csharp
using Variable.Reservoir;

public class Gun
{
    // Clip Size: 30
    // Current in Clip: 30
    // In Backpack: 60
    public ReservoirInt Ammo = new ReservoirInt(30, 30, 60);

    public void Fire()
    {
        // Implicitly checks Volume
        if (Ammo > 0)
        {
            Ammo.Volume--;
            Bang();
        }
        else
        {
            Reload();
        }
    }

    public void Reload()
    {
        // Magic logic:
        // 1. Checks how many bullets we need to be full.
        // 2. Checks how many we have in reserve.
        // 3. Moves the bullets.
        // 4. Returns how many were moved.
        int reloadedAmount = Ammo.Refill();

        if (reloadedAmount > 0) PlayReloadAnim();
    }
}
```

### 2. The Jetpack (Fuel)

```csharp
// Tank Capacity: 100.0
// Current: 50.0
// Reserve Canister: 500.0
public ReservoirFloat Fuel = new ReservoirFloat(100f, 50f, 500f);

public void Update(float dt)
{
    // Burn fuel
    if (Fuel.Volume.TryConsume(10f * dt))
    {
        Fly();
    }

    // Auto-refuel from reserve when empty
    if (Fuel.Volume.IsEmpty() && Fuel.Reserve > 0)
    {
        Fuel.Refill();
        PlayRefuelSound();
    }
}
```

---

## 🔧 API Reference

### Properties
- `Volume`: The active container (Bounded).
- `Reserve`: The backup supply (Raw value).

### Methods
- `Refill()`: Fills Volume to Max. Takes from Reserve. Returns amount moved.
- `Refill(amount)`: Tries to move specific amount.
- `MoveToReserve()`: Dumps Volume back into Reserve (Unload).

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
