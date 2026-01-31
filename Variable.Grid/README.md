# 🕸️ Variable.Grid

### High-Performance 2D Grid Primitives for C#

**The "Excel Sheet" for your Game State.** 📊
Stop allocating `List<List<T>>` or `Tile[,]`. Use a flat array with 2D superpowers.

## 🧠 Mental Model

Imagine a **Chessboard**.
*   Instead of making 64 individual squares, we take a **long strip of 64 squares** and chop it into 8 rows.
*   `Variable.Grid` handles the math to pretend that long strip is a 2D board.
*   This is how your computer memory actually works. We just made it friendly.

## 👶 ELI5 (Explain Like I'm 5)

*   You have a big box of Legos lined up in a row.
*   You want to build a wall 10 bricks wide.
*   `Variable.Grid` is the magic ruler that tells you: "Brick #23 goes in Row 2, Column 3!"

## 🚀 Installation

```bash
dotnet add package Variable.Grid
```

## 🛠️ Usage

### 1. Create a Grid
```csharp
using Variable.Grid;

// Create a 10x10 map of integers
var map = new Grid2D<int>(10, 10);
```

### 2. Access Cells
```csharp
// Set values like a 2D array
map[5, 5] = 99;

// Or use the raw index if you need speed!
map[55] = 99;
```

### 3. Check Bounds
```csharp
if (map.IsValid(12, 0))
{
    // Safe!
}
```

### 4. Extensions
```csharp
// Fill the whole map with 0
map.Fill(0);

// Get the 3rd row (Zero allocation!)
Span<int> row = map.GetRow(3);
```
