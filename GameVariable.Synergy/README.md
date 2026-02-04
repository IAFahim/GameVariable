# 🎻 GameVariable.Synergy

**The Orchestra of Variables.** 🎶

**GameVariable.Synergy** is the integration layer that proves all GameVariable components work together in perfect harmony. It provides composite structures to demonstrate how `Health`, `Mana`, `Cooldowns`, and `Experience` interact in a real game character.

---

## 🧠 Mental Model

Think of **The Orchestra**.
*   **Variable.Bounded (The Cup)** is the Violins.
*   **Variable.Timer (The Egg Timer)** is the Percussion.
*   **Variable.Regen (The Faucet)** is the Woodwinds.
*   **GameVariable.Synergy** is the **Conductor**. It makes sure they all play at the same tempo (Time.deltaTime) and stay in sync.

---

## 👶 ELI5 (Explain Like I'm 5)

*   You have a **Health Bar** (Cup).
*   You have a **Mana Bar** that refills itself (Faucet).
*   You have a **Skill Button** that takes time to recharge (Egg Timer).
*   **Synergy** is the "Character Sheet" that holds them all together in one place so you can update them all at once.

---

## 📦 Installation

```bash
dotnet add package GameVariable.Synergy
```

---

## 🚀 Features

*   **🎻 SynergyCharacter:** A zero-allocation struct combining Health, Mana, Cooldown, and XP.
*   **⚡ Unified Tick:** Update everything with one `character.Tick(deltaTime)` call.
*   **🏗️ Architecture Proof:** Demonstrates the Data-Logic-Extension pattern in action.

---

## 🎮 Usage Guide

### 1. Creating a Character

```csharp
using Variable.Bounded;
using Variable.Regen;
using Variable.Timer;
using Variable.Experience;
using GameVariable.Synergy;

// Create individual components
var health = new BoundedFloat(100f);
var mana = new RegenFloat(100f, 0f, 5f); // 5 mana/sec
var cooldown = new Cooldown(3f);         // 3 sec cooldown
var xp = new ExperienceInt(1000);

// Combine them into a Synergy Character
var player = new SynergyCharacter(health, mana, cooldown, xp);
```

### 2. The Game Loop

```csharp
void Update()
{
    // Updates Mana (Regen) and Cooldown (Timer) automatically
    player.Tick(Time.deltaTime);

    // Check state
    if (player.AbilityCooldown.IsReady())
    {
        Console.WriteLine("Ready to cast!");
    }
}
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
