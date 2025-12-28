# Variable.Inventory

**Variable.Inventory** encapsulates the logic for managing quantities, capacities, and transfers between containers. It abstracts the math of "Do I have enough?" and "Can I fit this?" into simple, reliable methods.

## Installation

```bash
dotnet add package Variable.Inventory
```

## Features

*   **InventoryLogic**: Static methods for inventory operations.
*   **Partial & Exact Transfers**: Support for "Move all you can" vs "Move only if all fits".
*   **Capacity Checks**: `CanAccept`, `GetRemainingSpace`.

## Usage

### Looting Items
```csharp
using Variable.Inventory;

float backpackCurrent = 10f;
float backpackMax = 50f;
float lootAmount = 5f;
float overflow;

// Adds loot to backpack, returns amount actually added
float added = InventoryLogic.AddPartial(ref backpackCurrent, lootAmount, backpackMax, out overflow);

if (overflow > 0) {
    Console.WriteLine($"Could not fit {overflow} items!");
}
```

### Crafting (Consume Materials)
```csharp
float wood = 20f;
float cost = 5f;

if (InventoryLogic.TryRemoveExact(ref wood, cost)) {
    CraftItem();
} else {
    Console.WriteLine("Not enough wood!");
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
