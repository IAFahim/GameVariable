# 🔷 Variable.Core

**The Foundation of GameVariable.** 🧱

**Variable.Core** isn't just a package; it's the *blueprint* for the entire GameVariable ecosystem. It defines the shared DNA that makes `Health`, `Mana`, `Cooldowns`, and `Experience` all speak the same language.

---

## 🧠 Mental Model

Think of **The Universal Adapter**.
*   In the real world, you have different plugs (US, EU, UK).
*   **Variable.Core** is the travel adapter that lets you plug *anything* (Health, Ammo, Time) into *any* socket (UI Bars, Save Systems, AI Logic).
*   If it has a limit (`Max`) and a value (`Current`), it fits.

---

## 👶 ELI5 (Explain Like I'm 5)

*   You have a **Health Bar** script.
*   You have a **Mana Bar** script.
*   You have a **Stamina Bar** script.
*   **Variable.Core** lets you delete all of those and write just **ONE** script called "Resource Bar" that works for everything.

---

## 📦 Installation

```bash
dotnet add package Variable.Core
```

---

## 🚀 Features

*   **🔌 Universal Interfaces:** `IBoundedInfo` works for anything with limits.
*   **✨ Magic Extensions:** `GetRatio()`, `IsFull()`, `IsEmpty()` work on everything.
*   **🏗️ Foundation:** The base layer for creating your own custom variables.

---

## 🎮 Usage Guide

### 1. The Universal UI Script

```csharp
using Variable.Core;

// This method works for Health, Mana, Ammo, and Timers!
public void UpdateProgressBar(IBoundedInfo value)
{
    // GetRatio() returns 0.0 to 1.0 automatically
    image.fillAmount = (float)value.GetRatio();

    if (value.IsEmpty())
    {
        image.color = Color.red;
    }
}
```

### 2. Creating Custom Variables

Implement `IBoundedInfo` to instantly gain compatibility with the entire ecosystem.

```csharp
public struct Shield : IBoundedInfo
{
    public float Integrity;
    public float MaxIntegrity;
    
    // Explicit implementation
    float IBoundedInfo.Min => 0f;
    float IBoundedInfo.Max => MaxIntegrity;
    float IBoundedInfo.Current => Integrity;
}

// Now you can do:
shield.IsFull(); // Works automatically!
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
