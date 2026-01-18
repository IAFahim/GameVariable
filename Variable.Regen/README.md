# ♻️ Variable.Regen

**Automatic Resource Regeneration & Decay.** 🌱

**Variable.Regen** wraps a bounded value with a `Rate` of change per second. It handles the math of "X per second" so you don't have to.

---

## 📦 Installation

```bash
dotnet add package Variable.Regen
```

---

## 🚀 Features

* **⚡ Auto-Tick:** Just call `.Tick(deltaTime)` and it handles the rest.
* **🧪 Decay:** Negative rates work perfectly for poison, radiation, or hunger.
* **🛡️ Clamped:** Respects Min/Max bounds automatically.
* **🏗️ Zero Allocation:** Pure structs, Burst compatible.

---

## 🎮 Usage Guide

### 1. Mana Regeneration (Positive Rate)

```csharp
using Variable.Regen;

// Max 100, Current 0, +10 per second
var mana = new RegenFloat(100f, 0f, 10f);

void Update()
{
    // Automatically adds 10 * deltaTime
    // Clamps to 100
    mana.Tick(Time.deltaTime);
}
```

### 2. Hunger/Decay (Negative Rate)

```csharp
// Max 100, Current 100, -5 per second
var hunger = new RegenFloat(100f, 100f, -5f);

void Update()
{
    // Automatically subtracts 5 * deltaTime
    // Clamps to 0
    hunger.Tick(Time.deltaTime);

    if (hunger.IsEmpty())
    {
        TakeStarvationDamage();
    }
}
```

### 3. Modifying Values

Since `RegenFloat` wraps a `BoundedFloat`, you can modify it directly or via extensions.

```csharp
// Consume mana (spell cast)
// 'Value' gives access to the underlying BoundedFloat
if (mana.Value.Current >= 20f)
{
    mana.Value.Current -= 20f;
}

// Or use Bounded extensions directly on the wrapper (implicit conversion often handles this,
// but accessing .Value is clearer)
if (mana.Value.TryConsume(20f))
{
    CastSpell();
}
```

## 🤝 Contributing
Found a bug? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---

## 🔧 API Reference

### `RegenFloat`
- `Value`: The underlying `BoundedFloat` (Current, Min, Max).
- `Rate`: Units per second.

### Extensions
- `Tick(deltaTime)`: Applies `Rate * deltaTime` to `Value`.
- `IsFull()`: Is it at max capacity?
- `IsEmpty()`: Is it at min capacity?

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
