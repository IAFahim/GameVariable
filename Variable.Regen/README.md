# ♻️ Variable.Regen

**The "Wolverine" Factor.** 🧬

**Variable.Regen** takes a number and makes it **alive**. It automatically grows (regeneration) or shrinks (decay) over time.

---

## 🧠 Mental Model

Think of a **Leaking Bucket** or a **Filling Pool**. 💧
- It's a `BoundedFloat` (the bucket).
- Plus a `Rate` (the leak or the hose).
- You just say "Time passed," and it calculates the new level.

Use this for **Mana**, **Stamina**, **Energy Shields**, or **Radiation Poisoning**.

---

## 👶 ELI5 (Explain Like I'm 5)

Normal Mana:
> **You:** "Update loop! Add 5 mana times delta time. Check if mana is over 100. Set to 100."
> **You:** *Writes this logic in Player, Enemy, Boss, Minion...*

Regen Mana:
> **You:** `mana.Tick(dt);`
> **Computer:** "Done. Added mana. Clamped to max. Next?"

---

## 📦 Installation

```bash
dotnet add package Variable.Regen
```

---

## 🎮 Usage Guide

### 1. Mana Regeneration (Growing)

```csharp
using Variable.Regen;

// Max 100. Starts at 0. Regens +10 per second.
var mana = new RegenFloat(100f, 0f, 10f);

void Update()
{
    // The magic line
    mana.Tick(Time.deltaTime);

    // Use it like a normal BoundedFloat
    if (mana.Value.Current >= 50f)
    {
        // ...
    }
}
```

### 2. Hunger System (Decaying) 📉

```csharp
// Max 100. Starts Full. Loses -1 per second.
var hunger = new RegenFloat(100f, 100f, -1f);

void Update()
{
    hunger.Tick(Time.deltaTime);

    if (hunger.Value.IsEmpty())
    {
        DieOfStarvation();
    }
}
```

### 3. Boosting Rate (Buffs)

You can change the rate dynamically!

```csharp
public void OnDrinkPotion()
{
    // Double regen speed!
    mana.Rate = 20f;
}
```

---

## 🔧 Inner Workings

`RegenFloat` is a **Composite Struct**.
It contains:
1.  `Value` (`BoundedFloat`): The actual number.
2.  `Rate` (`float`): The speed.

Because `Value` is a struct, accessing it directly gives you a **copy** (unless you use `ref`). But `RegenFloat` methods handle this for you.

```csharp
// Accessing the internal value
float currentMana = mana.Value.Current;

// Modifying the internal value
// Note: Since RegenFloat is a struct, be careful with copies!
// Best practice: Re-assign or use in a class.
mana.Value.Current -= 50f; // Works if 'mana' is a field in a class.
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
