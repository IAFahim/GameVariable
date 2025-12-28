# Variable.Stat

**Variable.Stat** provides a clean, data-oriented approach to calculating RPG statistics. It separates the raw data from
the calculation logic, making it ideal for ECS or high-performance architectures.

## Installation

```bash
dotnet add package Variable.Stat
```

## Features

* **StatLogic**: Static methods for calculating final values.
* **Standard Formula**: `(Base + Flat) * (1 + Add%) * Mult%`.
* **Smart Operations**: `Add`, `Subtract`, `Transfer`, `Set` with bounds checking.

## Usage

### Calculating Damage

```csharp
using Variable.Stat;

float baseDmg = 10f;
float flatBonus = 5f;   // +5 Damage from ring
float addPercent = 0.5f; // +50% Damage from potion
float multPercent = 2.0f; // x2 Damage from crit

// (10 + 5) * (1 + 0.5) * 2 = 45
float finalDamage = StatLogic.Calculate(baseDmg, flatBonus, addPercent, multPercent);
```

### Stat Transfer (Life Steal)

```csharp
float playerHp = 50f;
float enemyHp = 100f;
float stealAmount = 10f;

// Transfers 10 HP from enemy to player, respecting 0 and Max bounds
StatLogic.Transfer(ref enemyHp, ref playerHp, 100f, stealAmount);
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
