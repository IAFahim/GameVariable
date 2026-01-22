# 🔋 Variable.Reservoir

**The "Reload" Mechanic.** 🔫

**Variable.Reservoir** solves the **Clip + Backpack** problem. It manages two numbers: what you have *ready* (Volume) and what you have *stored* (Reserve).

---

## 🧠 Mental Model

Think of a **Gun Magazine** or a **Fuel Tank**. ⛽
- **Volume:** The bullets in the gun (Bounded: Min 0, Max 30).
- **Reserve:** The bullets in your backpack (Simple Integer).
- **Reloading:** Moving numbers from Reserve to Volume.

Use this for **Ammo**, **Battery Power**, **Potion Stacks** (Belt vs Inventory), or **Rocket Fuel**.

---

## 👶 ELI5 (Explain Like I'm 5)

Without Variable.Reservoir:
> **You:** "Reload!"
> **You:** `clip += 30;`
> **You:** `reserve -= 30;`
> **Bug:** "Wait, I only had 5 bullets in reserve! Now reserve is -25!" 😱

With Variable.Reservoir:
> **You:** `ammo.Refill();`
> **Computer:** "I took 5 from reserve because that's all you had. Clip is now 5. Reserve is 0."

---

## 📦 Installation

```bash
dotnet add package Variable.Reservoir
```

---

## 🎮 Usage Guide

### 1. The Shooter (Integers) 🔫

```csharp
using Variable.Reservoir;

// Clip Size: 30
// Current Clip: 30
// Backpack Ammo: 120
var ammo = new ReservoirInt(30, 30, 120);

// Shooting
if (ammo.Volume > 0)
{
    ammo.Volume--;
    FireBullet();
}

// Reloading
if (Input.GetButtonDown("Reload"))
{
    // Moves as much as possible from Reserve to Volume
    int reloadedAmount = ammo.Refill();

    if (reloadedAmount > 0) PlayReloadSound();
    else ShowMessage("NO AMMO!");
}
```

### 2. The Jetpack (Floats) 🚀

```csharp
// Tank Capacity: 100.0
// Current Fuel: 100.0
// Reserve Fuel: 500.0
var jetpack = new ReservoirFloat(100f, 100f, 500f);

void Update()
{
    // Flying consumes Active Volume
    if (Input.GetKey(KeyCode.Space) && jetpack.Volume.TryConsume(10f * Time.deltaTime))
    {
        ApplyThrust();
    }

    // Auto-refill from reserve when grounded?
    if (IsGrounded)
    {
        jetpack.Refill();
    }
}
```

---

## 🔧 API Reference

### Types
- `ReservoirInt`: For countable things (Bullets, Arrows).
- `ReservoirFloat`: For liquid things (Fuel, Energy).

### Properties
- `Volume` (`Bounded`): The active container. Has `.Current`, `.Max`.
- `Reserve`: The raw backup amount.

### Extensions
- `Refill()`: Fills Volume to Max using Reserve. Returns amount moved.
- `Refill(amount)`: Tries to move specific amount.
- `AddReserve(amount)`: Picking up an ammo box? Use this.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
