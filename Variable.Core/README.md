# 🔷 Variable.Core

**The "Common Tongue" of GameVariable.** 🗣️

**Variable.Core** defines the interfaces that allow Health, Mana, XP, and Timers to all play nicely together. It's the "USB Type-C" of this ecosystem—universal, essential, and just makes things work.

---

## 🧠 Mental Model

Think of `Variable.Core` as the **Contract**. 📜

If you have a `Health` struct and a `Mana` struct, they are totally different types. But if they both sign the `IBoundedInfo` contract, they promise to have:
1.  A Minimum (`Min`)
2.  A Maximum (`Max`)
3.  A Current Value (`Current`)

This means your UI code doesn't need to know *what* it is displaying. It just asks: "What's your percentage?" And the contract guarantees an answer.

---

## 👶 ELI5 (Explain Like I'm 5)

Imagine you have a **Health Bar** and a **Mana Bar** in your game.
Normally, you'd have to write code like:
> "If it's Health, look at `hp.Value`. If it's Mana, look at `mana.Amount`."

With **Variable.Core**, you just say:
> "Hey you! How full are you?"

And both the Health Bar and Mana Bar understand you! 🤝

---

## 📦 Installation

```bash
dotnet add package Variable.Core
```

---

## 🔑 The Contracts (Interfaces)

### 1. `IBoundedInfo`
**"I have limits!"** 🛑

Used for anything that has a range.
- **Health:** 0 to 100
- **Ammo:** 0 to 30
- **Thermostat:** -50 to +50

```csharp
public interface IBoundedInfo
{
    float Min { get; }
    float Max { get; }
    float Current { get; }
}
```

### 2. `ICompletable`
**"Are we there yet?"** ⏱️

Used for anything that finishes over time.
- **Cooldowns:** "Is the spell ready?"
- **Timers:** "Is the bomb exploded?"
- **Quests:** "Is the objective done?"

```csharp
public interface ICompletable
{
    bool IsComplete { get; }
}
```

---

## ⚡ The Superpowers (Extensions)

Because of these contracts, `Variable.Core` gives you magical extension methods that work on **everything**.

| You write... | It does... | Works on... |
|--------------|------------|-------------|
| `.IsFull()` | `Current >= Max` | Health, Mana, XP, Ammo... |
| `.IsEmpty()` | `Current <= Min` | Health, Mana, XP, Ammo... |
| `.GetRatio()` | `(Current - Min) / Range` | Anything! Returns 0.0 to 1.0 |
| `.GetRange()` | `Max - Min` | Anything! |
| `.GetRemaining()` | `Max - Current` | XP to next level, Ammo needed... |

---

## 🏗️ For Plugin Creators

Want to make your own custom stat? Maybe a `Shield` that has specialized logic? Just implement `IBoundedInfo`!

```csharp
using Variable.Core;

public struct EnergyShield : IBoundedInfo
{
    public float Charge;
    public float MaxCharge;

    // The Contract
    float IBoundedInfo.Min => 0f;
    float IBoundedInfo.Max => MaxCharge;
    float IBoundedInfo.Current => Charge;
}
```

Now your `EnergyShield` automatically works with all `GameVariable` UI tools! 🎉

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
