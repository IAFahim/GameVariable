# ♻️ Variable.Regen

**The Faucet.** 🚰

**Variable.Regen** creates resources that fill up (or drain) automatically over time. It's `Variable.Bounded` but with a motor attached.

---

## 🧠 Mental Model: The Faucet

Imagine a **Bathtub (BoundedFloat)** with a **Faucet (Rate)** turned on.
*   **Positive Rate:** The faucet is filling the tub.
*   **Negative Rate:** The drain is open.
*   **Zero Rate:** The water is still.

You just tell it how much time passed (`Tick(dt)`), and it calculates the water level.

---

## 👶 ELI5: "It comes back on its own!"

*   **Mana** is a cup that refills itself.
*   **Stamina** refills when you stop running.
*   **Shields** recharge after 5 seconds.
*   **Radiation Poisoning** kills you slowly.

They all work the same way: `Current += Rate * Time`.

---

## 📦 Installation

```bash
dotnet add package Variable.Regen
```

---

## 🚀 Usage Guide

### 1. Mana (Regeneration)

```csharp
using Variable.Regen;

// Max 100, Start at 0, +10 Mana per second
var mana = new RegenFloat(100f, 0f, 10f);

void Update()
{
    // Add (10 * dt) to current.
    // Automatically stops at 100.
    mana.Tick(Time.deltaTime);
}

void CastSpell()
{
    // Access the underlying BoundedFloat via .Value
    if (mana.Value.Current >= 20f)
    {
        mana.Value -= 20f;
        Fire();
    }
}
```

### 2. Hunger (Decay)

```csharp
// Max 100, Start Full, -5 Hunger per second
var hunger = new RegenFloat(100f, 100f, -5f);

void Update()
{
    // Subtracts (5 * dt). Stops at 0.
    hunger.Tick(Time.deltaTime);

    // IsEmpty checks the underlying value
    if (hunger.Value.IsEmpty())
    {
        DieOfStarvation();
    }
}
```

### 3. Rate Modifications (Buffs/Debuffs)

You can change the speed of the faucet at any time.

```csharp
public void OnDrinkCoffee()
{
    stamina.Rate *= 2f; // Double regen speed!
}

public void OnPoisoned()
{
    health.Rate = -5f; // Start draining health!
}
```

---

## 🔧 API Reference

### Properties
- `Value`: The `BoundedFloat` container (Current, Min, Max).
- `Rate`: How much to add per second.

### Methods
- `Tick(float deltaTime)`: Updates the value based on the rate.
- `implicit operator float`: Returns `Value.Current`.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
