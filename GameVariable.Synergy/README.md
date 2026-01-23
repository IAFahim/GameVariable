# 🤝 GameVariable.Synergy

**The Glue that holds it all together.**

## 🧠 Mental Model

Think of `GameVariable.Synergy` as the **Conductor of an Orchestra**.

Each module (`Variable.Bounded`, `Variable.Experience`, `Variable.RPG`) is a skilled musician playing their own instrument perfectly. But without a conductor, they might play at different tempos or volumes.

`Synergy` provides the **Sheet Music** (`SynergyState`) and the **Baton** (`SynergyLogic`) to ensure:
*   **Level Up** (Experience) triggers **Stat Growth** (RPG).
*   **Stat Growth** (RPG) increases **Max Health/Mana** (Bounded).
*   **Max Mana** (Bounded) influences **Regeneration Rates** (Regen).

It transforms isolated components into a cohesive **Game System**.

## 👶 ELI5 (Explain Like I'm 5)

Imagine you have a box of Lego bricks.
*   `Variable.Experience` is a blue brick (XP).
*   `Variable.RPG` is a red brick (Strength).
*   `Variable.Bounded` is a green brick (Health).

`GameVariable.Synergy` is the **Instruction Manual** that shows you how to snap these bricks together to build a cool **Robot** (your Game Character).

## 📦 Installation

```xml
<PackageReference Include="GameVariable.Synergy" Version="..." />
```

## 🚀 Usage

### 1. Define the State
Create a `SynergyState` that holds all your data.

```csharp
// Create a state with 2 stats (Vitality, Wisdom)
var state = new SynergyState(2);
```

### 2. Update the Logic
Run the update loop to process regeneration and level ups.

```csharp
// Add XP
state.AddExperience(100);

// Update logic (handles LevelUp -> Stats -> MaxValues -> Regen)
state.Update(Time.deltaTime);
```

### 3. Cleanup
Since `SynergyState` uses unmanaged memory for high performance, you must dispose it.

```csharp
state.Dispose();
```
