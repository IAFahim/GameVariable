# 🎻 GameVariable.Synergy

> **The Conductor**

The integration layer that harmonizes `Variable.RPG`, `Variable.Experience`, `Variable.Regen`, and `Variable.Bounded`. It acts as the "Sergey" of your project, ensuring all systems play together in perfect time.

## 🧠 The Mental Model

Think of **Synergy** as the **Conductor** of an orchestra.
*   **Musicians**: Your individual systems (Stats, XP, Health, Mana).
*   **The Score**: The rules (Level Up -> Stats Up -> Health Up).
*   **The Baton**: The `Update()` loop.

Without the Conductor, the violinists (Stats) might play faster than the cellists (Regen). Synergy ensures that when one changes, the others react instantly and correctly.

## 👶 ELI5 (Explain Like I'm 5)

Imagine you have a toy robot.
*   `Variable.RPG` is the **motor**.
*   `Variable.Experience` is the **battery meter**.
*   `Variable.Regen` is the **self-repair kit**.

**Synergy** is the **chip** that tells the motor to spin faster when the battery levels up, and tells the repair kit to work harder when the motor is strong. It connects all the wires so you don't have to!

## 📦 Installation

Add the project to your solution:

```bash
dotnet add reference GameVariable.Synergy/GameVariable.Synergy.csproj
```

## 🚀 Usage

### The "Sergey" Setup

Create the state and initialize it.

```csharp
using GameVariable.Synergy;

// 1. Create the state (Health, Mana, Starting XP Max)
var state = new SynergyState(100, 50, 1000);

// 2. Initialize (Calculates Level 1 stats and derived values)
state.Initialize();
```

### The Loop

Call `Update` every frame. This handles **everything**: leveling up, updating stats, and ticking regeneration.

```csharp
void Update() {
    // Pass delta time (e.g., Time.deltaTime in Unity)
    state.Update(0.016f);
}
```

### Handling Events

Want to give XP? Just add it. The next `Update` will handle the level up!

```csharp
// Grant XP
state.AddExperience(500);

// If this triggers a level up, Synergy automatically:
// 1. Increases Level
// 2. Increases Base Stats (Vitality, Spirit, etc.)
// 3. Increases Max Health/Mana (based on Vitality/Spirit)
// 4. Increases Regen Rates (based on Vitality/Spirit)
// 5. Refills Health/Mana (optional design choice)
```

### Cleanup

Since `SynergyState` manages unmanaged memory for stats, don't forget to dispose!

```csharp
state.Dispose();
```
