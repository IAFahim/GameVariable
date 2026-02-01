# 🔷 Variable.Core

**The Universal Adapter.** 🔌

**Variable.Core** is the shared DNA of the entire GameVariable ecosystem. It defines the common interfaces that let Health, Mana, Cooldowns, and XP all speak the same language.

---

## 🧠 Mental Model: The Universal Adapter

Imagine you have a US plug, a UK plug, and a Japanese plug. Usually, you need three different sockets.

**Variable.Core** is the Universal Travel Adapter. It turns *any* distinct game mechanic (Health, Timer, Ammo) into a generic "Thing with a Value" that your UI and Systems can understand without knowing the details.

---

## 👶 ELI5: "How full is the cup?"

A **Health Bar** is just a cup of water.
A **Cooldown Timer** is an hourglass (sand falling).
A **Gun Magazine** is a box of bullets.

They are all totally different things, but they all share one question:
**"How full are you right now?"**

`Variable.Core` asks that question. It doesn't care *what* the thing is, only *how full* it is.

---

## 📦 Installation

```bash
dotnet add package Variable.Core
```

---

## 🚀 Usage Guide

### 1. The "One Script to Rule Them All" (UI)

Stop writing `HealthBar.cs`, `ManaBar.cs`, and `StaminaBar.cs`. Write **one** script.

```csharp
using Variable.Core;

public class ResourceBar : MonoBehaviour
{
    public Image FillImage;

    // Takes Health, Mana, Timer, Ammo... ANYTHING!
    public void UpdateView(IBoundedInfo resource)
    {
        // GetRatio() returns 0.0 to 1.0 automatically.
        FillImage.fillAmount = (float)resource.GetRatio();
    }
}
```

### 2. Magic Extensions ✨

Just by importing `Variable.Core`, any object implementing `IBoundedInfo` gets these superpowers:

| Method | Returns | Description |
|--------|---------|-------------|
| `GetRatio()` | `0.0` - `1.0` | Percentage full. |
| `IsFull()` | `bool` | Is `Current == Max`? |
| `IsEmpty()` | `bool` | Is `Current == Min`? |
| `GetRange()` | `float` | `Max - Min` (Capacity). |
| `GetRemaining()` | `float` | `Max - Current` (Space left). |

---

## 🏗️ For Plugin Creators

Want to make your own custom variable? Implement `IBoundedInfo`.

```csharp
public struct Shield : IBoundedInfo
{
    public float Integrity;
    public float MaxIntegrity;
    
    // Interface Implementation
    float IBoundedInfo.Min => 0f;
    float IBoundedInfo.Max => MaxIntegrity;
    float IBoundedInfo.Current => Integrity;
}
```

Now your Shield works with your UI bars automatically! 🤯

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
