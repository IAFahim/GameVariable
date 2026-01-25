# 🏁 Variable.Grid

### The Tactic Board

> "The map is not the territory... but it's a very efficient array."

**Variable.Grid** provides zero-allocation, struct-based 2D grid primitives. It wraps a flat 1D array with 2D coordinate logic, allowing you to perform spatial operations (like finding neighbors or checking bounds) without the overhead of multidimensional arrays or object graphs.

---

## 🧠 The Mental Model

Think of a **Grid** like a **Chessboard** 🏁.
But instead of 64 separate "Square" objects, we just store the pieces in a single long line (an array) and use math to figure out where the "rows" break.

It separates the **Storage** (the array) from the **Shape** (width/height).

---

## 👶 ELI5 (Explain Like I'm 5)

Imagine you have a long strip of paper with 100 stickers on it.
You want to make a square map, so you cut the strip every 10 stickers and paste them below each other.
Now you have a 10x10 map!

**Variable.Grid** does the "cutting" using math, so you don't actually have to cut the paper (which takes time).

---

## 🛠 Installation

```bash
dotnet add package Variable.Grid
```

---

## 🚀 Usage

### 1. Basic Setup

```csharp
using Variable.Grid;

// Create a 10x10 grid of integers
var grid = new Grid2D<int>(10, 10);

// Set value at (x: 5, y: 3)
grid.Set(5, 3, 99);

// Get value
int value = grid.Get(5, 3); // Returns 99
```

### 2. Checking Neighbors (Zero Allocation)

Find out who lives next door without creating new lists!

```csharp
// Prepare a buffer for results (up to 8 neighbors)
Span<int> neighborIndices = stackalloc int[8];

// Get neighbors of cell (5, 5)
grid.GetNeighbors(5, 5, neighborIndices, out int count);

// Iterate safely
for (int i = 0; i < count; i++)
{
    int idx = neighborIndices[i];
    Console.WriteLine($"Neighbor at index {idx} has value {grid.Get(idx)}");
}
```

### 3. Coordinate Math

```csharp
// Convert 2D (x,y) to 1D Index
int index = grid.ToIndex(2, 4);

// Convert 1D Index to 2D (x,y)
grid.ToXY(index, out int x, out int y);
```
