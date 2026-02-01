# 📊 Variable.Bounded

**The Cup.** 🥤

**Variable.Bounded** gives you zero-allocation structs that *know their limits*. No more manually checking `if (health < 0) health = 0;` spread across 50 different scripts.

---

## 🧠 Mental Model: The Cup

Think of `BoundedFloat` as a physical **Cup**.
*   **Max:** The size of the cup.
*   **Current:** The amount of liquid inside.
*   **Min:** Empty (usually 0).

If you try to pour a gallon of water into a pint glass, it just overflows. The glass doesn't break; the extra water just disappears.
If you try to drink from an empty cup, you just get air.

**Variable.Bounded** enforces the physics of the cup automatically.

---

## 👶 ELI5: "I can't have 110% Health!"

If your character has 100 Max Health, and they drink a Potion that heals 500 HP... they should still have 100 HP. They shouldn't explode.

**Variable.Bounded** makes sure math works like video games, not like algebra.
*   `100 + 500 = 100` (Clamped to Max)
*   `0 - 50 = 0` (Clamped to Min)

---

## 📦 Installation

```bash
dotnet add package Variable.Bounded
```

---

## 🚀 Usage Guide

### 1. The Basics (Health System)

```csharp
using Variable.Bounded;

// Create a cup of size 100. Starts full.
var health = new BoundedFloat(100f);

// Take damage
health -= 25f;  // Result: 75. No if-checks needed!

// Check status (using Variable.Core extensions)
if (health.IsEmpty())
{
    Die();
}

// Heal
health += 50f;  // Result: 100. Can't over-heal.

// UI
float fillAmount = (float)health.GetRatio(); // 0.0 to 1.0
```

### 2. Advanced Ranges (Temperature)

Not every cup starts at 0. Some start at -50!

```csharp
// Range: -50 to +50. Starting at 20.
var temperature = new BoundedFloat(50f, -50f, 20f);

temperature -= 100f; // Clamps to -50f (Absolute Zero-ish)
```

### 3. Ammo Clips (Integers)

Use `BoundedInt` for discrete things like bullets or inventory slots.

```csharp
var ammo = new BoundedInt(12); // Max 12, Current 12

// Fire!
if (!ammo.IsEmpty())
{
    ammo--;
    FireBullet();
}
```

---

## ⚠️ Important: The "Public Field" Trap

For maximum performance (Unity ECS/Burst), we expose public fields. This is great for speed, but requires care.

### ❌ The Wrong Way
```csharp
health.Current = 9999f; // 😱 OH NO!
// You just bypassed the limits! The struct doesn't know you changed this.
```

### ✅ The Right Way (Extension Methods)
```csharp
health.Set(9999f); // Correct! Will clamp to Max.
```

### ✅ The "I Know What I'm Doing" Way
If you *must* modify fields directly (e.g. inside a Job), you **must** normalize afterwards:
```csharp
health.Current += calculation;
health.Normalize(); // Snaps value back to bounds
```

---

## 📚 Types Available

| Type | Use Case | Size |
|------|----------|------|
| `BoundedFloat` | Health, Mana, Stamina, Temperature | 12 bytes |
| `BoundedInt` | Ammo count, Inventory slots, Skill points | 12 bytes |
| `BoundedByte` | Small counts (0-255), Grid coordinates | 2 bytes |

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
