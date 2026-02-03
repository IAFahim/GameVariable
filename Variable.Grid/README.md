# 🕸️ Variable.Grid

**The "Excel Sheet" for your Game State.** 📊

**Variable.Grid** gives you high-performance 2D grid primitives. Stop allocating `List<List<T>>` or `Tile[,]`. Use a flat array with 2D superpowers.

---

## 🧠 Mental Model: The Spreadsheet 📉

Imagine a **Spreadsheet** (or Chessboard).
*   Instead of making 64 individual squares, we take a **long strip of 64 squares** and chop it into 8 rows.
*   `Variable.Grid` handles the math to pretend that long strip is a 2D board.
*   This is how your computer memory actually works (Sequential). We just made it friendly.

---

## 📦 Installation

```bash
dotnet add package Variable.Grid
```

---

## 🚀 Features

*   **⚡ Blazing Fast:** Uses `ref` returns and `Span<T>` for zero-copy access.
*   **💾 Cache Friendly:** Flat 1D array layout ensures sequential memory access.
*   **🛡️ Bounds Checking:** Safe access by default (with Unsafe options for raw speed).

---

## 🎮 Usage Guide

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

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
