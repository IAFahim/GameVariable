# VoidClimber - GameVariable Library Demo

A comprehensive text-based roguelike game demonstrating ~90% of the GameVariable library's capabilities.

## 🎮 Game Overview

**VoidClimber** is a high-performance text-based roguelike built using the GameVariable ecosystem. It features:

- **Tick-based simulation** combining real-time systems (Regen, Cooldown) with turn-based combat
- **Procedural dungeon generation** with scaling enemy difficulty
- **RPG progression** with stats, XP, level-ups, and powerup selection
- **Shop system** with gold economy
- **Potion management** using Volume (belt) + Reserve (bag) system
- **Combat system** with damage types, crits, dodge, lifesteal, and thorns

## 📁 Project Structure

```
VoidClimber/
├── Core/                    # Data structures (Layer A)
│   ├── GameEnums.cs        # All game enums
│   ├── Entity.cs           # Base entity with stats
│   ├── PlayerData.cs       # Player-specific data
│   └── GameState.cs        # Complete game state
├── Logic/                   # Game logic (Layer B)
│   ├── CombatLogic.cs      # Damage resolution
│   ├── ProgressionLogic.cs # XP and level-ups
│   ├── ShopLogic.cs        # Shop transactions
│   └── EnemyGeneration.cs  # Procedural enemies
├── Systems/                 # Game systems (Layer C)
│   ├── GameLoop.cs         # Tick simulation
│   ├── GameRenderer.cs     # Console UI
│   └── InputHandler.cs     # Input & combos
├── Program.cs              # Entry point
└── FIXES_NEEDED.md         # API fix guide
```

## 🏗️ Architecture

### Data-Logic-Extension Triad

The game strictly follows the architectural pattern:

1. **Layer A - Data**: Structs hold all data
   - `Entity`: RPG stats, health, cooldowns
   - `PlayerData`: Entity + XP + potions
   - `GameState`: Complete session state

2. **Layer B - Logic**: Static classes manipulate data
   - `CombatLogic`: Damage resolution
   - `ProgressionLogic`: XP and powerups
   - `ShopLogic`: Transactions
   - `EnemyGeneration`: Procedural scaling

3. **Layer C - Extension**: Systems provide gameplay
   - `GameLoop`: Tick-based simulation
   - `GameRenderer`: Console rendering
   - `InputHandler`: Input processing

## 📦 GameVariable Library Usage

### Variable.RPG
- **RpgStatSheet**: 10 stats per entity
- **Damage resolution**: Diamond architecture with mitigation
- **Stat modifiers**: Flat and percentage bonuses
```csharp
entity.Stats.SetBase((int)StatType.AttackDamage, 10f);
RpgStatExtensions.AddModifier(ref stat, flat: 5f, percent: 0.1f);
```

### Variable.Regen
- **RegenFloat**: Health with regeneration
```csharp
entity.Health = new RegenFloat(maxHp: 100f, current: 100f, regenPerSec: 1f);
entity.Health.Tick(deltaTime);
```

### Variable.Timer
- **Cooldown**: Attack and ability cooldowns
```csharp
entity.AttackCooldown = new Cooldown(duration: 1.0f);
entity.AttackCooldown.Tick(deltaTime);
if (entity.AttackCooldown.IsReady()) Attack();
```

### Variable.Experience
- **ExperienceInt**: XP tracking and level-ups
```csharp
player.Experience = new ExperienceInt(max: 100, current: 0, level: 1);
int levelsGained = ExperienceExtensions.Add(ref player.Experience, amount: 50, new LinearXpFormula());
```

### Variable.Reservoir
- **ReservoirInt**: Potion belt + reserve
```csharp
player.Potions = new ReservoirInt(volume: 3, volumeMax: 3, reserve: 10);
player.Potions.Volume--;  // Use potion
player.Potions.Refill();  // Transfer reserve to volume
```

### Variable.Inventory
- **InventoryLogic**: Transaction validation
```csharp
bool canAfford = InventoryLogic.HasEnough(player.Gold, cost);
bool success = InventoryLogic.TryRemoveExact(ref player.Gold, cost);
```

### Variable.Input
- **InputRingBuffer**: Input history for combos
- **ComboGraph**: State machine for combo detection (framework ready)

## 🎯 Game Features

### Stats System (10 stats per entity)
- HealthMax, HealthRegen, AttackDamage, Defense
- AttackSpeed, CritChance, CritMultiplier
- DodgeChance, Lifesteal, Thorns

### Combat Mechanics
- **Physical damage**: Mitigated by Defense (flat)
- **Fire damage**: Mitigated by % Max HP
- **Critical hits**: Based on CritChance stat
- **Dodge**: Based on DodgeChance stat
- **Lifesteal**: Heal % of damage dealt
- **Thorns**: Reflect damage when hit

### Enemy Types (8 types)
- Goblin, Slime, Skeleton, Orc
- Lich, Golem, Dragon (boss), VoidTerror (boss)

### Room Types (8 types)
- Encounter, Elite, Boss, Treasure
- Shrine, Shop, Rest, Mystery

### Powerup System (13 options)
- Flat and percentage bonuses to all stats
- Procedurally generated on level-up

## 🚀 Building and Running

### Prerequisites
- .NET 9.0 SDK
- All GameVariable packages built

### Status: API Fixes Needed

The demo has been fully implemented but requires API fixes to match the actual GameVariable library. See **[FIXES_NEEDED.md](FIXES_NEEDED.md)** for detailed fix instructions.

### Quick Start (After Fixes)
```bash
# Build
dotnet build VoidClimber/VoidClimber.csproj

# Run
dotnet run --project VoidClimber/VoidClimber.csproj
```

## 📊 Code Statistics

- **Lines of Code**: ~2,500
- **Files Created**: 12
- **Game Systems**: 8
- **Library Packages Used**: 9/10 (90%)

## 🎓 Learning Resources

This demo demonstrates:
- ✅ Zero-allocation design patterns
- ✅ Struct-based game entities
- ✅ Composition of multiple variable types
- ✅ Extension method architecture
- ✅ Real-time + turn-based hybrid gameplay
- ✅ Procedural generation with seeds

## 🔧 Current Issues

See **[FIXES_NEEDED.md](FIXES_NEEDED.md)** for:
1. ExperienceInt API access patterns
2. InventoryLogic method signatures
3. RpgStatExtensions usage
4. Missing using directives
5. Ref vs readonly ref issues

## 📝 TODO (After API Fixes)

- [ ] Fix all API mismatches per FIXES_NEEDED.md
- [ ] Build and run the game
- [ ] Add comprehensive unit tests
- [ ] Performance benchmarking
- [ ] Combo system expansion
- [ ] Additional enemy abilities
- [ ] More shrine effects
- [ ] Save/load system

## 🙏 Credits

Built as a demonstration of the GameVariable library's capabilities for high-performance, zero-allocation game variable management in .NET.
