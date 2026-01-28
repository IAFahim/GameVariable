# 🎻 GameVariable.Synergy

**The Conductor of the Ecosystem.** 🎼
Integration layer for `Variable.*` packages.

## 🧠 The Mental Model: "The Conductor"

Imagine an orchestra.
*   **Violins** are `Variable.Timer` (playing fast notes).
*   **Percussion** is `Variable.Input` (hitting beats).
*   **Brass** is `Variable.RPG` (loud stats).

Individually, they are noise. You need a **Conductor** to make them play a symphony.

**Synergy** is that conductor. It holds your Stats, XP, Health, and Mana in one optimized, unsafe (for performance!) struct and ensures they talk to each other perfectly.

## 👶 ELI5 (Explain Like I'm 5)

You have a toy robot.
*   One hand holds a **calculator** (Stats).
*   One hand holds a **cup** (Health).
*   It has a **badge** (XP Level).

**Synergy** is the brain that says: "When I get a new Badge (Level Up), punch the number into the Calculator (Update Stats), and get a bigger Cup (Max Health)!"

## 🚀 Installation

```bash
dotnet add package GameVariable.Synergy
```

## 🛠 Usage

### 1. The "Sergey" State

Create the conductor.

```csharp
using GameVariable.Synergy;

// 1. Create State (Allocates memory!)
// 10 Stats, 1000 XP to level, 100 Health, 50 Mana
var state = new SynergyState(10, 1000, 100f, 50f);

// 2. Setup Stats (e.g. 0=MaxHP, 1=MaxMana, 2=RegenHP)
state.Stats.SetBase(0, 200f); // Max HP
state.Stats.SetBase(1, 100f); // Max Mana
state.Stats.SetBase(2, 5f);   // HP Regen
```

### 2. The Game Loop

Let the conductor lead.

```csharp
void Update(float dt)
{
    // Ticks Health and Mana regen
    state.Tick(dt);
}
```

### 3. Level Up & Sync

When things change, sync them up!

```csharp
void OnLevelUp()
{
    // Update stats based on level...
    state.Stats.SetBase(0, 300f); // New Max HP

    // Sync to Bounded/Regen values
    // Map: MaxHP=0, MaxMana=1, RegenHP=2, RegenMana=3
    state.SyncStats(0, 1, 2, 3);
}
```

### 4. Cleanup

Don't leave the conductor on stage!

```csharp
state.Dispose();
```
