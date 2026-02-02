# 🔋 Variable.Reservoir

**Two-stage resources: Active vs Reserve.** 🎒

**Variable.Reservoir** models resources that have a "Clip" (Active) and a "Backpack" (Reserve). It handles the logic of reloading/refilling from the reserve into the active container.

---

## 🧠 Mental Model: "The Clip & Backpack" 🔫

Think of a shooter game.
- **Volume (The Clip):** The ammo currently in your gun. Bounded (0 to 12).
- **Reserve (The Backpack):** The total ammo you are carrying. Unbounded (or just a large int).
- **Refill (Reload):** Moves ammo from Backpack to Clip, up to the Clip's limit.

It's not just for guns!
- **Stamina:** Sprint Energy (Volume) vs Exhaustion Limit (Reserve).
- **Batteries:** Flashlight charge (Volume) vs Spare Batteries (Reserve).

## 👶 ELI5: "Pocket vs Backpack"

You have 5 candies in your pocket (Volume) and a big bag of 50 candies in your backpack (Reserve).
When you eat the candies in your pocket, you can grab more from the backpack to fill your pocket again. But you can't hold more than 5 in your pocket at once.

---

## 📦 Installation

```bash
dotnet add package Variable.Reservoir
```

---

## 🛠️ Usage Guide

### 1. The "FPS Reload" (ReservoirInt)

```csharp
using Variable.Reservoir;

public struct Gun
{
    // Clip Size 12. Starts Full (12). Reserve has 36 rounds.
    public ReservoirInt Ammo;

    public Gun()
    {
        Ammo = new ReservoirInt(12, 12, 36);
    }

    public void Fire()
    {
        if (Ammo.Volume > 0)
        {
            Ammo.Volume--; // Reduces Clip
            Console.WriteLine("Bang!");
        }
        else
        {
            Console.WriteLine("Click... Need reload!");
        }
    }

    public void Reload()
    {
        // Moves from Reserve -> Volume
        // Returns amount actually moved (e.g., 5 rounds)
        int reloaded = Ammo.Refill();
        Console.WriteLine($"Reloaded {reloaded} rounds.");
    }
}
```

### 2. The "Energy Shield" (ReservoirFloat)

Maybe your shield regenerates from a battery pack.

```csharp
public struct Robot
{
    // Shield 100.0. Battery 500.0.
    public ReservoirFloat Shield;

    public void Update(float dt)
    {
        // Auto-recharge shield from battery if shield is low
        if (!Shield.Volume.IsFull() && Shield.Reserve > 0)
        {
            // Calculate how much to transfer this frame
            float transfer = 10f * dt;

            // Manual transfer logic (custom refill)
            // Or just use Refill() for instant full recharge
            Shield.Refill();
        }
    }
}
```

---

## 🔧 API Reference

### Fields
- **`Volume`**: The active container (`BoundedInt` / `BoundedFloat`).
- **`Reserve`**: The backup supply (`int` / `float`).

### Extensions
- **`Refill()`**: Fills `Volume` to Max using `Reserve`. Decreases `Reserve` by amount used. Returns amount transferred.
- **Implicit Conversion**: `ReservoirInt` converts to `int` (returns Volume.Current).

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
