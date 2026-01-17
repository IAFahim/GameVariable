# 🎒 Variable.Inventory

**Math, not Arrays.** A pure logic library for inventory calculations.

`Variable.Inventory` is **not** a list of items. It is a set of static algorithms (`InventoryLogic`) to calculate transfers, check capacities, and handle stack sizes. It assumes *you* manage the storage (array, list, whatever) and helps you do the math correctly.

[![NuGet](https://img.shields.io/nuget/v/Variable.Inventory?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Inventory)

## 📦 Installation

```bash
dotnet add package Variable.Inventory
```

## 🔥 Features

* **🧮 Pure Math**: Calculates "How much can I fit?" without mutating anything unless you ask.
* **📦 Stack Logic**: Handles max stack sizes and partial transfers.
* **⚡ Zero Allocation**: Just integers and structs. No garbage.
* **🔄 Stateless**: Safe to use from any thread or job.

## 🛠️ Usage Guide

### 1. Can I Add This?
Check if an item fits before trying to add it.

```csharp
using Variable.Inventory;

int currentAmount = 5;
int maxStack = 10;
int amountToAdd = 8;

// Calculate how much fits
int added = InventoryLogic.CalculateAddAmount(currentAmount, maxStack, amountToAdd);

Console.WriteLine($"Can add: {added}"); // 5 (because 5+5=10)
Console.WriteLine($"Leftover: {amountToAdd - added}"); // 3
```

### 2. Transferring Items
Moving items between slots or containers.

```csharp
int source = 10;
int destination = 2;
int maxStack = 10;

// Calculate transfer
InventoryLogic.Transfer(
    ref source,      // 10 -> 2
    ref destination, // 2 -> 10
    maxStack
);
```

### 3. Managing Capacity
If you have a weight limit or volume limit.

```csharp
int currentWeight = 90;
int maxWeight = 100;
int itemWeight = 5;
int quantity = 3; // Total weight = 15

bool fits = InventoryLogic.CanFit(currentWeight, maxWeight, itemWeight * quantity);
// False (90 + 15 > 100)
```

## ❓ Why no `List<Item>`?
Because every game is different.
* Minecraft needs a grid.
* Diablo needs Tetris-packing.
* Final Fantasy needs a linear list.

`Variable.Inventory` provides the **mathematical truths** common to all of them (e.g., you can't have more than MaxStack), leaving the data structure up to you. This makes it compatible with Unity ECS, standard OOP, or even database backends.

## 🤝 Contributing
Found a bug? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
