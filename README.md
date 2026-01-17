<div align="center">

# 🎮 GameVariable

### High-Performance Game State Management for C#

[![NuGet](https://img.shields.io/nuget/v/Variable.Core?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Core)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-Standard%202.1+-purple.svg)](https://dotnet.microsoft.com/)
[![Unity](https://img.shields.io/badge/Unity-ECS%20Compatible-black.svg)](https://unity.com/)

**Zero-allocation structs for health bars, cooldowns, experience systems, and more.**
*Stop writing `if (health < 0) health = 0` forever.*

[Quick Start](#-quick-start) •
[Packages](#-packages) •
[Examples](#-examples) •
[API Reference](#-api-reference) •
[Contributing](#-contributing)

</div>

---

## ✨ Why GameVariable?

Building game systems means managing **bounded values** everywhere—health that can't go negative, cooldowns that tick down, experience that overflows into levels. GameVariable provides battle-tested primitives that handle all the edge cases, so you can focus on gameplay.

<table>
<tr>
<td width="50%">

### ❌ Without GameVariable
```csharp
// Manual clamping everywhere
health -= damage;
if (health < 0) health = 0;
if (health > maxHealth) health = maxHealth;

// Manual timer logic
cooldown -= deltaTime;
if (cooldown < 0) cooldown = 0;
bool canUse = cooldown <= 0;

// Manual regen with edge cases
mana += regenRate * deltaTime;
if (mana > maxMana) mana = maxMana;
```

</td>
<td width="50%">

### ✅ With GameVariable
```csharp
// Auto-clamped, natural syntax
health -= damage;
if (health.IsEmpty()) Die();

// Built-in timer semantics
cooldown.Tick(deltaTime);
if (cooldown.IsFull()) UseAbility();

// One-liner regeneration
mana.Tick(deltaTime); // Automatic!
```

</td>
</tr>
</table>

### 🎯 Key Features

| Feature | Description |
|---------|-------------|
| **🚀 Zero Allocation** | Pure `struct` types—no heap, no GC pressure. |
| **⚡ Burst Compatible** | Works with Unity's Burst compiler & DOTS/ECS. |
| **🔧 Operator Overloads** | Natural syntax: `hp -= 10`, `timer++`, `ammo--`. |
| **📊 Built-in Ratios** | `GetRatio()` for health bars (0.0 to 1.0). |
| **🎨 Serializable** | `[Serializable]` for Unity Inspector & save systems. |
| **🌐 Network Ready** | Blittable structs for easy network serialization. |

---

## 🚀 Quick Start

### Installation

```bash
# Install all packages (if you're hardcore)
dotnet add package Variable.Core
dotnet add package Variable.Bounded
dotnet add package Variable.Timer
dotnet add package Variable.Regen
dotnet add package Variable.Experience
dotnet add package Variable.RPG
dotnet add package Variable.Reservoir
dotnet add package Variable.Input
dotnet add package Variable.Inventory
dotnet add package GameVariable.Intent

# Or just grab what you need (Recommended)
dotnet add package Variable.Bounded
```

### Basic Usage

```csharp
using Variable.Bounded;
using Variable.Timer;
using Variable.Regen;

// Health: auto-clamped between 0 and 100
var health = new BoundedFloat(100f);
health -= 25f;                    // Take damage
Console.WriteLine(health);        // "75/100"
Console.WriteLine(health.GetRatio()); // 0.75

// Cooldown: 3 second ability
var fireball = new Cooldown(3f);
fireball.Tick(deltaTime);         // Count down
if (fireball.IsFull()) {          // Ready when 0
    CastFireball();
    fireball.Reset();             // Start cooldown
}

// Mana: regenerates 10/sec
var mana = new RegenFloat(100f, 50f, 10f);
mana.Tick(deltaTime);             // Auto-regenerate
```

---

## 📦 Packages

Each package is a specialist. Choose your weapons.

| Package | Purpose | Use Cases |
|---------|---------|-----------|
| [**Variable.Core**](./Variable.Core) | 🔷 **Foundation** | The shared interfaces `IBoundedInfo` and `ICompletable`. |
| [**Variable.Bounded**](./Variable.Bounded) | 📊 **Limits** | `BoundedFloat`, `BoundedInt`. Health, stamina, resources. |
| [**Variable.Timer**](./Variable.Timer) | ⏱️ **Time** | `Timer` (up), `Cooldown` (down). Spells, casting, delays. |
| [**Variable.Regen**](./Variable.Regen) | ♻️ **Growth** | `RegenFloat`. Mana regeneration, radiation decay. |
| [**Variable.Reservoir**](./Variable.Reservoir) | 🔋 **Capacity** | `ReservoirInt`. Ammo clips + reserves, battery packs. |
| [**Variable.Experience**](./Variable.Experience) | ⭐ **Progression** | `ExperienceInt`. Leveling up, XP curves, overflow. |
| [**Variable.RPG**](./Variable.RPG) | 📈 **Stats** | `RpgStatSheet`. Attributes (STR/DEX), modifiers, damage math. |
| [**Variable.Inventory**](./Variable.Inventory) | 🎒 **Logistics** | `InventoryLogic`. Calculating stack limits and transfers. |
| [**Variable.Input**](./Variable.Input) | 🎮 **Controls** | `ComboGraph`. Input buffering, fighting game combos. |
| [**GameVariable.Intent**](./GameVariable.Intent) | 🧠 **Brain** | `IntentState`. Hierarchical State Machine for AI. |

---

## 💡 Examples

### 🏥 Health System
```csharp
public struct Player
{
    public BoundedFloat Health;
    public BoundedFloat Shield;
    
    public void TakeDamage(float damage)
    {
        // Damage shields first
        if (Shield > 0)
        {
            float absorbed = Math.Min(Shield, damage);
            Shield -= absorbed;
            damage -= absorbed;
        }
        
        // Remaining damage hits health
        Health -= damage;
        
        if (Health.IsEmpty())
            Die();
    }
}
```

### ⚔️ Ability Cooldown
```csharp
public struct Ability
{
    public Cooldown Cooldown;
    public float ManaCost;
    
    public bool TryUse(ref BoundedFloat mana)
    {
        if (!Cooldown.IsReady()) return false; // On cooldown
        if (mana < ManaCost) return false;     // Not enough mana
        
        mana -= ManaCost;
        Cooldown.Reset();  // Start cooldown
        return true;
    }
    
    public void Update(float deltaTime)
    {
        Cooldown.Tick(deltaTime);
    }
}
```

### 🔫 Ammo & Reloading
```csharp
public struct Weapon
{
    public ReservoirInt Ammo;
    
    public bool TryFire()
    {
        if (Ammo.Volume.IsEmpty()) return false;
        Ammo.Volume--;
        return true;
    }
    
    public void Reload()
    {
        Ammo.Refill();  // Transfer from reserve to magazine automatically
    }
}
```

---

## 🎮 Unity Integration

These structs are **Unity ECS / DOTS** ready. They are blittable and contain no managed references.

```csharp
using Unity.Entities;
using Unity.Burst;
using Variable.Bounded;

public struct HealthComponent : IComponentData
{
    public BoundedFloat Value;
}

[BurstCompile]
public partial struct HealthSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // Blazing fast parallel processing
        foreach (var health in SystemAPI.Query<RefRW<HealthComponent>>())
        {
            if (health.ValueRO.Value.IsEmpty())
            {
                // Handle death
            }
        }
    }
}
```

---

## 📝 Contributing

Contributions are welcome! Please read our [Contributing Guidelines](CONTRIBUTING.md) before submitting PRs.

### Development Setup
1. Clone the repo.
2. Run `dotnet restore`.
3. Run `dotnet test` to make sure everything is green.
4. Hack away!

---

## 📄 License

MIT License - see [LICENSE](LICENSE) for details.

---

<div align="center">

**Made with ❤️ for game developers**

**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)  
**Email:** iafahim.dev@gmail.com

</div>
