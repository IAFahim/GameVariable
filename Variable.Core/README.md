# 🔷 Variable.Core

**The "Universal Adapter" for Game Mechanics.** 🧱

**Variable.Core** is the shared DNA of the entire GameVariable ecosystem. It defines the common language (`IBoundedInfo`, `ICompletable`) that allows Health, Mana, XP, Ammo, and Timers to talk to your UI, Save System, and AI seamlessly.

---

## 🧠 Mental Model: "The Universal Adapter"

Imagine you are building a house. You have power tools from different brands (Makita, DeWalt, Milwaukee). Normally, the batteries don't fit each other.

**Variable.Core** is the adapter that makes **any** battery fit **any** tool.
- Your UI script is the "Tool".
- Your Health/Mana/XP classes are the "Batteries".
- `IBoundedInfo` is the shape of the connector.

Because they all implement `IBoundedInfo`, you can plug *anything* into your UI bar script, and it just works.

## 👶 ELI5: "The Lego Baseplate"

Think of this package as the green Lego baseplate. It doesn't look like a castle or a spaceship yet, but it's the thing you snap all your other blocks onto so they don't fall apart.

---

## 📦 Installation

```bash
dotnet add package Variable.Core
```

---

## 🔑 Key Interfaces

### 1. `IBoundedInfo` (The "Container")
Represents anything that has a **Current** value between a **Min** and **Max**.

```csharp
public interface IBoundedInfo
{
    float Min { get; }
    float Max { get; }
    float Current { get; }
}
```

**Common uses:**
- Health (0 to 100)
- Ammo (0 to 30)
- Temperature (-50 to +50)
- Progress Bars (0.0 to 1.0)

### 2. `ICompletable` (The "Timer")
Represents anything that eventually "finishes".

```csharp
public interface ICompletable
{
    bool IsComplete { get; }
}
```

**Common uses:**
- Cooldowns (Ready when time <= 0)
- Timers (Finished when time >= Duration)
- Quests (Complete when objectives met)

---

## ✨ Features & Extensions

Importing `Variable.Core` gives you superpowers on *any* class that implements `IBoundedInfo`.

### Logic Checkers
Stop writing `if (health <= 0)` manually!

```csharp
// Is it full? (Current >= Max)
bool isFull = myMana.IsFull();

// Is it empty? (Current <= Min)
bool isDead = myHealth.IsEmpty();
```

### Math Helpers
Get useful numbers without doing the math yourself.

```csharp
// Get a 0.0 to 1.0 value for UI sliders
float percent = (float)myXP.GetRatio();

// How big is the tank? (Max - Min)
float capacity = myFuel.GetRange();

// How much more to fill it? (Max - Current)
float needed = myShield.GetRemaining();
```

---

## 🛠️ Usage Example

### The "One Script to Rule Them All" (UI)

Because of `Variable.Core`, you only need **one** script to handle every progress bar in your game.

```csharp
using Variable.Core;

public class ResourceBar : MonoBehaviour
{
    public Image FillImage;
    
    // Accepts Health, Mana, XP, Ammo... anything!
    public void UpdateView(IBoundedInfo resource)
    {
        // GetRatio() comes from Variable.Core extensions
        FillImage.fillAmount = (float)resource.GetRatio();

        if (resource.IsEmpty())
        {
            FillImage.color = Color.red; // Danger!
        }
    }
}
```

### The "Custom Container"

You can make your own weird containers compatible with the ecosystem just by adding `: IBoundedInfo`.

```csharp
public struct MagicBucket : IBoundedInfo
{
    public float WaterAmount;

    // Implement the interface
    public float Min => 0f;
    public float Max => 10f;
    public float Current => WaterAmount;
}

// Now you can use extensions on it!
var bucket = new MagicBucket { WaterAmount = 10f };
if (bucket.IsFull())
{
    Console.WriteLine("Bucket is full!");
}
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
