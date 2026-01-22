# 🎒 Variable.Inventory

**The "Tetris" Logic.** 🧩

**Variable.Inventory** provides the pure math for inventory systems. It answers questions like "Can I fit this?" and "How much fits?" without caring if you use Arrays, Lists, or Dictionaries.

---

## 🧠 Mental Model

Think of **pouring sand into buckets**. ⏳
- **Partial Add:** You have 10kg of sand. The bucket fits 4kg. You pour 4kg, and keep 6kg.
- **Exact Add:** You have a solid brick. It fits or it doesn't.
- **Transfer:** Pouring from Bucket A to Bucket B.

**Variable.Inventory** is the physics engine for these operations.

---

## 👶 ELI5 (Explain Like I'm 5)

Without Variable.Inventory:
> **You:** "Give him the gold."
> **Bug:** "He has max gold! Now he has `Max + 100` gold."
> **You:** *Writes another if-check.*

With Variable.Inventory:
> **You:** `InventoryLogic.TryAddPartial(ref gold, amount, max, ...);`
> **Computer:** "I filled his wallet to the max, and dropped the rest on the floor."

---

## 📦 Installation

```bash
dotnet add package Variable.Inventory
```

---

## 🎮 Usage Guide

### 1. Looting (The "Take All" Button)

You try to pick up 100 Gold. You can only carry 50 more.

```csharp
using Variable.Inventory;

float myGold = 950f;
float maxGold = 1000f;
float lootAmount = 100f;

// TryAddPartial: Adds what fits. Returns what's left.
bool pickedUpAny = InventoryLogic.TryAddPartial(
    ref myGold,
    lootAmount,
    maxGold,
    out float added,
    out float leftovers
);

// Result:
// myGold = 1000 (Full)
// added = 50
// leftovers = 50 (Still on ground)
```

### 2. Crafting (The "All or Nothing" Check)

You need 5 Wood to craft a chair. If you have 4, you can't craft "80% of a chair".

```csharp
float myWood = 4f;
float cost = 5f;

// TryRemoveExact: Removes ONLY if you have enough.
if (InventoryLogic.TryRemoveExact(ref myWood, cost))
{
    CraftChair();
}
else
{
    ShowMessage("Need more wood!");
}
```

### 3. Moving Items (Chest ➔ Player)

```csharp
// Move from Chest (Source) to Player (Dest)
InventoryLogic.TryTransferPartial(
    ref chest.Apples,    // Source
    ref player.Apples,   // Destination
    player.MaxApples,    // Capacity
    10,                  // Amount to move
    out float moved      // Actual amount moved
);
```

---

## 🔧 API Reference

### Logic Methods
All methods are static and optimized (`[AggressiveInlining]`).

- `TryAddPartial`: Fills until Max. Returns overflow.
- `TryAddExact`: Fills only if `Current + Amount <= Max`.
- `TryRemovePartial`: Removes up to Amount (until empty).
- `TryRemoveExact`: Removes only if `Current >= Amount`.
- `TryTransferPartial`: Moves items safely between two variables.
- `CanAccept`: Prediction check ("Would this fit?").
- `HasEnough`: Prediction check ("Do I have enough?").

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
