# 📊 Variable.Bounded

**The "Cup" that never spills.** ☕

**Variable.Bounded** gives you numeric types that *know their limits*. They automatically clamp themselves between a minimum and maximum value, so you never have to write `if (health < 0) health = 0;` ever again.

---

## 🧠 Mental Model: "The Cup"

Think of a `BoundedFloat` like a coffee cup.
- **Min:** The bottom of the cup (usually 0).
- **Max:** The rim of the cup (Capacity).
- **Current:** The coffee inside.

If you pour too much coffee in (add value), it just overflows neatly (clamps to Max). It doesn't explode.
If you drink too much (subtract value), you just hit the bottom (clamps to Min). You can't drink negative coffee.

## 👶 ELI5: "The Magic Box"

Imagine a box that can hold 10 toys.
- If you try to put 100 toys in, the box just says "Nope, I'm full" and keeps exactly 10.
- If you try to take 20 toys out, the box says "I only have 0 left" and stops at 0.
It does the math for you so you don't make mistakes!

---

## 📦 Installation

```bash
dotnet add package Variable.Bounded
```

---

## 🛠️ Types Available

| Type | Size | Best For... |
|------|------|-------------|
| **`BoundedFloat`** | 12 bytes | Health, Mana, Stamina, Temperature |
| **`BoundedInt`** | 12 bytes | Ammo, Inventory Slots, Skill Points |
| **`BoundedByte`** | 2 bytes | Grid Coordinates, Small Counts (0-255) |

---

## ✨ Features

### 1. Auto-Clamping Math
Just use normal math operators. The safety is built-in.

```csharp
var health = new BoundedFloat(100f); // Max 100, Current 100

health -= 50f;  // Current is 50
health += 1000f; // Current is 100 (Clamped!)
health -= 9999f; // Current is 0 (Clamped!)
```

### 2. Logic & Checks
Ask the variable about its state.

```csharp
if (ammo.IsEmpty()) Reload();
if (mana.IsFull()) CastSuperSpell();
if (health.GetRatio() < 0.3f) PlayLowHealthSound();
```

### 3. Consumption Check
Try to use a resource, but only if you have enough.

```csharp
// "TryConsume" subtracts if possible, returns false if not.
if (mana.TryConsume(50f))
{
    CastFireball(); // Only runs if we had 50 mana!
}
else
{
    ShowNoManaError();
}
```

---

## 🎮 Usage Examples

### The "Health Bar" (Standard 0-100)

```csharp
using Variable.Bounded;

public struct Character
{
    public BoundedFloat Health;

    public Character(float maxHp)
    {
        // 0 to maxHp, starts full
        Health = new BoundedFloat(maxHp);
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health.IsEmpty()) Die();
    }
}
```

### The "Temperature" (Negative Ranges)

Not everything starts at zero!

```csharp
// Thermometer: -50°C to +50°C. Starts at Room Temp (20°C).
var thermometer = new BoundedFloat(50f, -50f, 20f);

// It gets cold...
thermometer -= 40f;
// Result: -20f. (20 - 40 = -20). Within bounds!

// Absolute zero freeze!
thermometer -= 100f;
// Result: -50f. (Clamped to Min).
```

### The "Inventory Stack" (BoundedInt)

Use Integers for things that can't be fractions (like bullets or apples).

```csharp
// Stack of 64 items. Starts with 1.
var arrows = new BoundedInt(64, 0, 1);

arrows += 10; // 11 arrows.
```

---

## ⚠️ Important: Direct Modification

For performance (Unity ECS/Burst), the fields `Current`, `Min`, and `Max` are public.
**Be careful!** If you set `Current` directly, you bypass the safety checks.

```csharp
// ❌ DANGEROUS: Bypasses clamping!
health.Current = 9999f;

// ✅ SAFE: Uses extension method to clamp.
health.Set(9999f);

// ✅ MANUAL SAFETY: If you MUST modify Current directly...
health.Current += 10f;
health.Normalize(); // Snaps it back to valid range.
```

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
