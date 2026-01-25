# 🤝 GameVariable.Synergy

**The "Sergey" of the family.** Integration layer for GameVariable libraries.

---

## 🎯 What Is This?

A reference implementation showing how to combine `Variable.Core`, `Variable.Experience`, `Variable.RPG`, and `Variable.Regen` into a cohesive game loop.

It implements a **Diamond Architecture** integration where:
1. **Experience** drives **Level**.
2. **Level** drives **Stats** (RPG).
3. **Stats** drive **Resources** (Regen).

---

## 🧠 Mental Model

Think of **Synergy** as the **conductor** of an orchestra.
- The **Violins** are your Stats.
- The **Drums** are your Experience.
- The **Brass** is your Regen.
- Synergy makes them play the same song.

## 👶 ELI5

If you get XP, you level up.
If you level up, your Max HP goes up.
If your Max HP goes up, your Health Bar gets bigger.
This library does all that wiring for you.

---

## 🚀 Usage

### 1. Setup

Define your stat IDs and create the state.

```csharp
using GameVariable.Synergy;

const int MaxHp = 0;
const int MaxMp = 1;
const int HpRegen = 2;
const int MpRegen = 3;

// Create state with space for 10 stats
using var state = new SynergyState(10);

// Initialize base stats
state.Stats.SetBase(MaxHp, 100f);
state.Stats.SetBase(HpRegen, 1f);

// Sync initial regen values
state.SyncStats(MaxHp, MaxMp, HpRegen, MpRegen);
```

### 2. The Game Loop

```csharp
void Update(float dt) {
    // Ticks health/mana regen
    state.Tick(dt);
}
```

### 3. Leveling Up

```csharp
void OnKillMonster() {
    // Adds XP, handles level up, increases stats, updates health bar
    state.AddXp(500, MaxHp, MaxMp, HpRegen, MpRegen);
}
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**

</div>
