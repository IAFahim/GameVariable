# VoidClimber Demo - Fixes Needed

## Summary
The VoidClimber demo has been created with comprehensive game systems, but there are API mismatches with the actual GameVariable library that need to be fixed before the game can compile and run.

## Files Created

### Core Data Structures (Layer A)
1. **Core/GameEnums.cs** - All game enums (StatType, GamePhase, DamageElement, EnemyType, etc.)
2. **Core/Entity.cs** - Entity struct with RpgStatSheet, RegenFloat, Cooldown composition
3. **Core/PlayerData.cs** - PlayerData with ExperienceInt and ReservoirInt
4. **Core/GameState.cs** - Complete game state container

### Core Logic Systems (Layer B)
5. **Logic/CombatLogic.cs** - Damage resolution using Variable.RPG
6. **Logic/ProgressionLogic.cs** - XP and level-up system using Variable.Experience
7. **Logic/ShopLogic.cs** - Shop transactions using Variable.Inventory
8. **Logic/EnemyGeneration.cs** - Procedural enemy scaling

### Game Systems (Layer C)
9. **Systems/GameLoop.cs** - Tick-based simulation loop
10. **Systems/GameRenderer.cs** - Console UI rendering
11. **Systems/InputHandler.cs** - Input handling with combo support
12. **Program.cs** - Main entry point

## Required API Fixes

### 1. ExperienceInt API
**Current (incorrect):**
```csharp
xp.Value.Current
xp.Value.Max
```

**Correct:**
```csharp
xp.Current
xp.Max
xp.Level
```

### 2. InventoryLogic API
**Current (incorrect):**
```csharp
InventoryLogic.AddPartial(ref current, toAdd, max, out overflow)
```

**Correct:**
```csharp
InventoryLogic.TryAddPartial(ref current, toAdd, max, out actualAdded, out overflow)
```

### 3. RpgStatExtensions API
**Current (incorrect):**
```csharp
RpgStatModifier.AddFlat(RpgStatField.Base, 10f)
RpgStatExtensions.ApplyModifier(ref stat, modifier)
```

**Correct:**
```csharp
RpgStatExtensions.AddModifier(ref stat, flat: 10f, percent: 0f)
```

### 4. Missing Using Directives
Add to files that reference Logic classes:
```csharp
using VoidClimber.Logic;
```

### 5. Missing Fields
Add to PlayerData:
```csharp
public int ShopsVisited;
public int ShrinesVisited;
public int FloorsCleared;
```

### 6. Ref vs Readonly Ref
Change readonly parameters to ref when modifications are needed:
```csharp
// Change:
public static void SomeMethod(in GameState state)

// To:
public static void SomeMethod(ref GameState state)
```

### 7. Float Conversion
Add explicit casts where needed:
```csharp
filled = (int)(ratio * width);  // Already correct
float ratio = (float)Math.Sqrt(value);  // Add cast
```

## Quick Fix Steps

### Step 1: Fix ExperienceInt access
In all files, replace:
- `xp.Value.Current` → `xp.Current`
- `xp.Value.Max` → `xp.Max`
- `state.Player.Experience.Value.Current` → `state.Player.Experience.Current`

### Step 2: Fix InventoryLogic calls
Replace all `AddPartial` with `TryAddPartial` and add the `out` parameters.

### Step 3: Fix RpgStat usage
Replace all `RpgStatModifier` usage with direct `AddModifier(ref stat, flat, percent)` calls.

### Step 4: Add missing fields
Add `ShopsVisited`, `ShrinesVisited`, `FloorsCleared` to GameState.

### Step 5: Fix ref issues
Change `in GameState` to `ref GameState` where state is modified.

## Testing After Fixes

1. Build the project:
```bash
dotnet build VoidClimber/VoidClimber.csproj
```

2. Run the game:
```bash
dotnet run --project VoidClimber/VoidClimber.csproj
```

## Architecture Demonstrated

Once fixed, this demo showcases:

- **Variable.RPG**: 10 stats per entity, damage calculation with diamond architecture
- **Variable.Regen**: Health regeneration with RegenFloat
- **Variable.Timer**: Attack and special cooldowns
- **Variable.Experience**: XP tracking and level-ups
- **Variable.Reservoir**: Potion belt + reserve system
- **Variable.Inventory**: Gold transactions and shop logic
- **Variable.Input**: Combo system framework

## Data-Logic-Extension Triad

The code strictly follows:
- **Data**: Structs hold all data (Entity, PlayerData, GameState)
- **Logic**: Static classes manipulate data (CombatLogic, etc.)
- **Extension**: Systems provide gameplay (GameLoop, Renderer, Input)
