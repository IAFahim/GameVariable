# 🔷 Variable.Core

**The Universal Adapter.** 🔌

**Variable.Core** isn't just a package; it's the *blueprint* for the entire GameVariable ecosystem. It defines the shared DNA that makes `Health`, `Mana`, `Cooldowns`, and `Experience` all speak the same language.

---

## 🧠 Mental Model: The Universal Adapter

Imagine traveling to a different country. Your plug doesn't fit the wall. You need an adapter.
**Variable.Core** is that adapter for your game data.
*   It doesn't matter if it's Health (0-100), Ammo (0-30), or a Timer (0-5s).
*   They are all just "Something with a Max and a Current value".
*   This package lets you write **one UI script** that works for *everything*.

---

## 📦 Installation

```bash
dotnet add package Variable.Core
```

---

## 🚀 Why Does This Exist?

Imagine writing a UI Health Bar script.

**Without Core:**
You write one script for `PlayerHealth`, another for `EnemyHealth`, another for `Mana`, another for `Stamina`... because they are all different classes. 😫

**With Core:**
You write **ONE** script that takes `IBoundedInfo`.
```csharp
public void UpdateBar(IBoundedInfo resource)
{
    fillImage.fillAmount = (float)resource.GetRatio(); // Works for EVERYTHING!
}
```

---

## 🔑 Key Interfaces

### `IBoundedInfo`
**"I have limits!"** 🛑

Anything that has a `Min`, `Max`, and `Current` value.
- **Health:** 0 to 100
- **Ammo:** 0 to 30
- **Temperature:** -50 to +50

**Properties:**
- `float Min`
- `float Max`
- `float Current`

### `ICompletable`
**"Are we there yet?"** ⏱️

Anything that finishes over time.
- **Timer:** Counts UP to duration.
- **Cooldown:** Counts DOWN to zero.

**Properties:**
- `bool IsComplete`

---

## 🛠️ Extensions (The Magic ✨)

Importing `Variable.Core` gives you superpowers on *any* `IBoundedInfo`:

| Method | What it does | Example |
|--------|--------------|---------|
| `IsFull()` | `Current >= Max` | Is mana full? |
| `IsEmpty()` | `Current <= Min` | Is health zero? |
| `GetRatio()` | `(Current-Min) / Range` | Health bar % (0.0 to 1.0) |
| `GetRange()` | `Max - Min` | Total capacity |
| `GetRemaining()` | `Max - Current` | How much more XP needed? |

---

## 🏗️ For Plugin Creators

Building your own variable type? Implement `IBoundedInfo` to instantly gain compatibility with the entire ecosystem (UI tools, save systems, etc.).

```csharp
using Variable.Core;

public struct Shield : IBoundedInfo
{
    public float Integrity;
    public float MaxIntegrity;
    
    // Explicit implementation keeps your public API clean!
    float IBoundedInfo.Min => 0f;
    float IBoundedInfo.Max => MaxIntegrity;
    float IBoundedInfo.Current => Integrity;
}

// Now you can do:
var shield = new Shield { ... };
if (shield.IsFull()) PlayShieldSound();
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
