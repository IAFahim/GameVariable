# 🎻 GameVariable.Synergy

**The Conductor that makes your systems play together.**

**GameVariable.Synergy** is the glue layer that connects `Experience`, `RPG`, `Bounded`, and `Regen` into a cohesive game loop. It demonstrates the "Conductor" pattern.

---

## 🧠 Mental Model

Imagine an orchestra.
*   **The Musicians:** Your individual systems (Inventory, Stats, Health). They are experts at playing their own instrument but don't know the song.
*   **The Conductor (Synergy):** Doesn't play an instrument. Instead, it reads the sheet music (Logic) and tells the musicians when to play louder (Level Up), faster (Regen), or stop (Death).

In code, **Synergy** reads state from one struct (Level) and pipes it into another (Stats -> Max Health).

---

## 👶 ELI5 (Explain Like I'm 5)

You have a **Health Bar** (Bounded) and a **Level** (Experience).
When you **Level Up**, your **Max Health** should go up.
But `Bounded` doesn't know about `Experience`.
**Synergy** is the helper that says: "Hey! You leveled up! I will tell the Health Bar to get bigger now!"

---

## 📦 Installation

```bash
dotnet add package GameVariable.Synergy
```

---

## 🎮 Usage Guide

### 1. The State Struct

This holds everything. It's a "Mega Struct" (Layer A).

```csharp
using GameVariable.Synergy;

// Create the state
var state = new SynergyState();

// Initialize (Sets up memory, default stats)
state.Initialize(startLevel: 1);
```

### 2. The Game Loop

Call `Update` every frame. This is the Conductor doing its job (Layer C).

```csharp
void Update() {
    float dt = Time.deltaTime;

    // 1. Logic flows: Level -> Stats -> MaxHealth -> Regen
    // 2. Regen ticks: Health += Rate * dt
    state.Update(dt);
}
```

### 3. Modifying State

You change the inputs, Synergy handles the outputs.

```csharp
// Give XP
state.Level += 500;

// Update loop will automatically:
// 1. Detect Level Up
// 2. Increase Vitality
// 3. Increase Max Health
// 4. Increase Health Regen
```

### 4. Cleanup

Since it uses unmanaged memory (RPG Stats), you must dispose it.

```csharp
state.Dispose();
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
