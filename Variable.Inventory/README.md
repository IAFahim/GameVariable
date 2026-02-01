# 🎒 Variable.Inventory

**The Accountant.** 🧾

**Variable.Inventory** doesn't care if your inventory is a Grid, a List, or a Dictionary. It cares about **The Numbers**. It handles the math of moving items, checking limits, and calculating weight.

---

## 🧠 Mental Model: The Accountant

You are the Warehouse Manager. You don't move the boxes; you just update the ledger.

*   "Can I fit 50 apples?" -> `CanAccept()`
*   "Move 10 apples from Box A to Box B." -> `TryTransferPartial()`
*   "I need 5 wood to build this." -> `TryRemoveExact()`

**Variable.Inventory** is the set of rules that ensures you never have negative apples or overflow your warehouse.

---

## 👶 ELI5: "I found 100 Gold, but my wallet is full!"

You have space for 50 Gold. You find a pile of 100.
*   **Without Logic:** You pick up 100, now you have 150/50. (Bug!)
*   **With Logic (`TryAddPartial`):** You pick up 50 (Wallet Full), and leave 50 on the ground.

**Variable.Inventory** handles the "Partial Add" math so you don't have to write `min(space, amount)` every time.

---

## 📦 Installation

```bash
dotnet add package Variable.Inventory
```

---

## 🚀 Usage Guide

### 1. Looting (The Overflow Problem)

The most common bug in RPGs: picking up more than you can carry.

```csharp
using Variable.Inventory;

float myGold = 950f;
float maxGold = 1000f;
float lootPile = 100f;

// 1. Try to pick it all up
InventoryLogic.TryAddPartial(
    ref myGold,      // My Wallet (Modified)
    lootPile,        // Amount to add
    maxGold,         // Capacity
    out float taken, // How much I actually took (50)
    out float left   // How much is left (50)
);

// 2. Update the world
lootPile = left;
if (lootPile == 0) Destroy(pileObject);
```

### 2. Crafting (All or Nothing)

When crafting, you can't use "half" the required items.

```csharp
float wood = 3f;
float cost = 5f;

// TryRemoveExact returns FALSE if you don't have enough.
// It DOES NOT modify 'wood' if it returns false.
if (InventoryLogic.TryRemoveExact(ref wood, cost))
{
    CraftChair(); // Success! Wood is now -2... wait, no, logic prevents that!
    // Wood is essentially treated atomically here.
    // If successful, wood is reduced.
}
else
{
    Ui.ShowError("Not enough wood!");
}
```

### 3. Transfer (Chest to Bag)

Moving items between two containers.

```csharp
float chest = 100f;
float bag = 10f;
float bagMax = 50f;

InventoryLogic.TryTransferPartial(
    ref chest,   // Source (Will decrease)
    ref bag,     // Destination (Will increase)
    bagMax,      // Dest Limit
    1000f,       // Amount to try moving (Move All)
    out float movedAmount
);

// Result:
// Bag fills to 50 (+40).
// Chest drops to 60 (-40).
```

---

## 🔧 API Reference

### Logic Methods
All methods are `static` and `pure`.
- `TryAddPartial`: Fills until full. Returns overflow.
- `TryAddExact`: Adds only if the *whole* amount fits.
- `TryRemovePartial`: Removes everything up to amount.
- `TryRemoveExact`: Removes only if you have enough.
- `TryTransferPartial`: Moves from A to B.

### Checks
- `CanAccept()`: Will this fit?
- `HasEnough()`: Can I afford this?
- `IsFull()` / `IsEmpty()`

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
