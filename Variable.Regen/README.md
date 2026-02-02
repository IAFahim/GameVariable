# ♻️ Variable.Regen

**Resources that fill (or empty) themselves.** 🔋

**Variable.Regen** adds a time-based component to `BoundedFloat`. It's perfect for Mana that regenerates, Shields that recharge, or Poison that decays health over time.

---

## 🧠 Mental Model: "The Faucet" 🚰

Imagine a bathtub (`BoundedFloat`).
- **Regen:** You leave the faucet on (`Rate > 0`). The tub fills up over time until it hits the rim (Max).
- **Decay:** You pull the plug (`Rate < 0`). The tub drains over time until it's empty (Min).
- **Tick:** Every frame, you check how much water flowed (`Tick(deltaTime)`).

## 👶 ELI5: "Wolverine Healing"

You know how Wolverine gets hurt but then his health bar slowly goes back up? That's `Variable.Regen`. You just tell it "Heal 5 HP per second" and it does the rest.

---

## 📦 Installation

```bash
dotnet add package Variable.Regen
```

---

## 🛠️ Usage Guide

### 1. The "Mana Bar" (Regeneration)

```csharp
using Variable.Regen;

public struct Wizard
{
    // Max 100, Starts at 50, Regens 10 per second
    public RegenFloat Mana;

    public Wizard()
    {
        Mana = new RegenFloat(100f, 50f, 10f);
    }

    public void Update(float dt)
    {
        // Must call Tick() to apply the change!
        Mana.Tick(dt);
    }
}
```

### 2. The "Poison" (Decay)

Negative rates work too!

```csharp
// Max 100 toxicity. Starts at 100. Decays by 5 per second.
var toxicity = new RegenFloat(100f, 100f, -5f);

void Update(float dt)
{
    toxicity.Tick(dt);

    if (toxicity.IsEmpty())
    {
        Console.WriteLine("You are cured!");
    }
}
```

### 3. Modifying Values

Since `RegenFloat` wraps a `BoundedFloat`, you access the current value via `.Value`.

```csharp
// Take damage (reduce mana)
hero.Mana.Value -= 20f;

// Drink Potion (add mana)
hero.Mana.Value += 50f;

// Change Regen Rate (Buff)
hero.Mana.Rate = 20f; // Double regen speed!
```

---

## 🔧 API Reference

### Fields
- **`Value`**: The underlying `BoundedFloat` (Current, Min, Max).
- **`Rate`**: Units per second to add (can be negative).

### Extensions
- **`Tick(deltaTime)`**: Applies `Rate * deltaTime` to `Value`. Clamps result.
- **`IsFull()`**: Is `Value.Current >= Value.Max`?
- **`IsEmpty()`**: Is `Value.Current <= Value.Min`?
- **`GetRatio()`**: Progress (0.0 to 1.0).

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
