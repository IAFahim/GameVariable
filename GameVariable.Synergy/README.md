# 🎻 GameVariable.Synergy

**The Orchestra that makes all your systems play together.** 🎶

**GameVariable.Synergy** is the integration layer where `Health` meets `Stamina` meets `Level`. It proves that `GameVariable` isn't just a bunch of loose parts—it's a cohesive ecosystem.

---

## 🧠 Mental Model: The Orchestra

Imagine `Variable.Bounded` is a Violin 🎻, `Variable.Regen` is a Cello cello, and `Variable.Experience` is a Trumpet 🎺.

Individually, they make great sounds. But without a **Conductor** (Synergy), they are just noise.
**GameVariable.Synergy** is the sheet music and the conductor. It tells the Health bar to flash when Stamina is low, or the XP bar to glow when an enemy dies. It turns noise into a symphony.

---

## 👶 ELI5 (Explain Like I'm 5)

You have Lego blocks.
- **Red block** = Health.
- **Blue block** = Mana.
- **Yellow block** = XP.

**Synergy** is the instruction manual that shows you how to snap them together to build a **Cool Robot**. Without it, you just have a pile of blocks.

---

## 📦 Installation

```bash
dotnet add package GameVariable.Synergy
```

---

## 🎮 Usage Guide

### The "Rage Mode" Example

How do you make a character regenerate Stamina faster when Health is low?

**1. The Struct (The Data)**
Define a character that has both Health and Stamina.
```csharp
public struct SynergyCharacter
{
    public BoundedFloat Health;
    public RegenFloat Stamina;
}
```

**2. The Logic (The Interaction)**
Use `SynergyExtensions.Update()` to make them talk.
```csharp
var hero = new SynergyCharacter
{
    Health = new BoundedFloat(100f),
    Stamina = new RegenFloat(100f, 10f) // 10 regen per sec
};

// ... took some damage ...
hero.Health.Set(20f); // Low health!

// update frame
hero.Update(Time.deltaTime);

// Result: Stamina regenerates at DOUBLE speed (20 per sec) because of Rage Mode!
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
