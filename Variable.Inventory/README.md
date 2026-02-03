# 🎒 Variable.Inventory

**The Math of Stuff.** 🍎

**Variable.Inventory** is a pure logic library for handling inventory mathematics. It doesn't tell you *how* to store your items (List, Array, Dictionary) — it just handles the "Can I fit this?" and "Move this there" logic.

---

## 🧠 Mental Model: The Accountant 👨‍💼

Think of an **Accountant** standing between your Chest and your Backpack.
*   He doesn't hold the items.
*   He just counts them and approves transactions.
*   **"Can I fit 100 gold?"** -> Accountant checks the books (Math) and says "No, only 50".
*   **"Move 10 apples."** -> Accountant updates the numbers on both sides.

**Variable.Inventory** is that Accountant. It calculates the logic, you provide the storage.

---

## 📦 Installation

```bash
dotnet add package Variable.Inventory
```

---

## 🚀 Features

* **🧠 Pure Logic:** Static methods that work on `float`, `int`, `byte`.
* **⚖️ Weight Support:** Logic for transferring items with weight limits.
* **🌊 Partial vs Exact:** "Fill it as much as possible" vs "Only if it fits".
* **⚡ Burst Compatible:** All methods are `[AggressiveInlining]`.

---

## 🎮 Usage Guide

### 1. Looting (Partial Add)

You found 100 Gold, but can only carry 50 more.

```csharp
using Variable.Inventory;

float goldInBag = 950f;
float maxGold = 1000f;
float goldOnGround = 100f;

// Try to add as much as possible
bool addedAny = InventoryLogic.TryAddPartial(
    ref goldInBag,
    goldOnGround,
    maxGold,
    out float addedAmount,
    out float overflow
);

// Result:
// goldInBag = 1000
// addedAmount = 50
// overflow = 50 (remains on ground)
```

### 2. Crafting (Exact Remove)

You need exactly 5 Wood to build a chair.

```csharp
float wood = 3f;
float cost = 5f;

// Try to remove EXACTLY 5.
// Returns false if you have less (and removes nothing).
if (InventoryLogic.TryRemoveExact(ref wood, cost))
{
    BuildChair();
}
else
{
    Console.WriteLine("Not enough wood!");
}
```

### 3. Transferring Items (Chest to Player)

Move items from Container A to Container B.

```csharp
float chestCount = 100;
float bagCount = 10;
float bagMax = 50;

InventoryLogic.TryTransferPartial(
    ref chestCount,  // Source (decreases)
    ref bagCount,    // Destination (increases)
    bagMax,          // Dest Capacity
    1000,            // Try to move everything
    out float moved
);

// Result:
// chestCount = 60 (100 - 40)
// bagCount = 50 (Full)
// moved = 40
```

---

## 🔧 API Reference

### `InventoryLogic`
- `TryAddPartial`: Fills up to max. Returns overflow.
- `TryAddExact`: Adds only if total <= max.
- `TryRemovePartial`: Removes up to amount (empties it).
- `TryRemoveExact`: Removes only if current >= amount.
- `TryTransferPartial`: Moves items between variables.
- `CanAccept`: Checks if `current + amount <= max`.
- `HasEnough`: Checks if `current >= required`.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
