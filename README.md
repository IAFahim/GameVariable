# 🎮 GameVariable Ecosystem

**The "Standard Library" for Game Logic.** 🧱

**GameVariable** is a collection of high-performance, zero-allocation C# structs designed to solve common game development problems once and for all. No more rewriting "Health," "Cooldowns," or "Inventory" for every new project.

---

## 🌟 Why Use This?

* **⚡ Zero Allocation:** Everything is a `struct`. No Garbage Collection spikes. Burst/Jobs compatible.
* **🛡️ Bulletproof:** Values handle their own clamping, logic, and state.
* **🔌 Modular:** Pick only what you need. Each package is standalone (depending only on Core).
* **🧠 Standardized:** A `Health` bar works exactly like an `Ammo` bar or an `XP` bar.

---

## 📦 Packages Overview

| Package | Purpose | Use Case |
| :--- | :--- | :--- |
| **[Variable.Core](Variable.Core/)** | 🧱 **Foundation** | Interfaces (`IBoundedInfo`) & Math Constants. |
| **[Variable.Bounded](Variable.Bounded/)** | 📊 **Limits** | Health, Stamina, Mana (Min/Max/Current). |
| **[Variable.Timer](Variable.Timer/)** | ⏱️ **Time** | Cooldowns (Down) & Timers (Up). |
| **[Variable.Regen](Variable.Regen/)** | ♻️ **Over Time** | Mana Regen, Poison Decay, Hunger. |
| **[Variable.Reservoir](Variable.Reservoir/)** | 🔋 **Reloads** | Ammo Clips, Jetpack Fuel (Active + Reserve). |
| **[Variable.Inventory](Variable.Inventory/)** | 🎒 **Storage** | Logic for "Can I fit this?" & "Move X to Y". |
| **[Variable.Experience](Variable.Experience/)** | ⭐ **Leveling** | XP Curves, Level Ups, Overflow handling. |
| **[Variable.RPG](Variable.RPG/)** | ⚔️ **Stats** | Attributes, Modifiers (Flat/%), Damage Pipeline. |
| **[Variable.Input](Variable.Input/)** | 🎮 **Combos** | Input Buffering & Combo Trees. |
| **[GameVariable.Intent](GameVariable.Intent/)** | 🧠 **AI State** | Hierarchical State Machine for Tasks/AI. |

---

## 🚀 Quick Start

### 1. Install via NuGet
```bash
dotnet add package Variable.Bounded
dotnet add package Variable.Timer
```

### 2. The "Everything" Player Character
Look how clean your data becomes:

```csharp
public struct PlayerData
{
    // Health: 0 to 100
    public BoundedFloat Health;
    
    // Mana: Regenerates 5 per second
    public RegenFloat Mana;
    
    // Ammo: 30 in clip, 120 in bag
    public ReservoirInt Ammo;

    // Dash: 3 second cooldown
    public Cooldown DashCooldown;
    
    // XP: Level 1, 0/1000 XP
    public ExperienceInt XP;
}
```

### 3. Logic Update
Look how simple your logic becomes:

```csharp
public void Update(ref PlayerData player, float dt)
{
    // Auto-regen mana
    player.Mana.Tick(dt);

    // Auto-tick cooldown
    player.DashCooldown.TickAndCheckReady(dt);

    // Check death
    if (player.Health.IsEmpty()) Die();
}
```

---

## 🤝 Contributing

We love contributions! Please read [CONTRIBUTING.md](CONTRIBUTING.md) and [AGENTS.md](AGENTS.md) for our coding standards (Zero Allocation is law!).

---

<div align="center">

**Made with ❤️ by [Md Ishtiaq Ahamed Fahim](https://github.com/iafahim)**
*Performance first. Logic second. Allocations never.*

</div>
