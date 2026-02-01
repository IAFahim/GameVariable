# 🕸️ Variable.Grid

**The Spreadsheet.** 📊

**Variable.Grid** wraps a simple 1D array (`T[]`) with math that makes it behave like a 2D grid (`T[,]`). This gives you the performance of a flat array with the convenience of coordinates.

---

## 🧠 Mental Model: The Spreadsheet

Memory is a long strip of tape (1D).
A Grid is a Spreadsheet (2D).

**Variable.Grid** is the math that says "Cell C5 is actually Index 22 on the tape."

Because it's just a flat array underneath:
1.  It is **Cache Friendly** (CPU loves reading it).
2.  It is **Zero Allocation** to index.
3.  It serializes easily (just save the array!).

---

## 👶 ELI5: "It's just a long line."

Imagine you have a long line of 100 LEGOs.
You want to make a 10x10 floor.
You chop the line every 10 bricks and stack them.

**Variable.Grid** does the "chopping" in its head, so you can say "Row 2, Column 5" without actually cutting the LEGO line.

---

## 📦 Installation

```bash
dotnet add package Variable.Grid
```

---

## 🚀 Usage Guide

### 1. Creating a Map

Stop using `Tile[,]` (slow) or `List<List<Tile>>` (garbage city).

```csharp
using Variable.Grid;

// Create a 10x10 grid of ints
var map = new Grid2D<int>(10, 10);

// It allocates ONE array of size 100.
```

### 2. Accessing Data

Use `[x, y]` like normal.

```csharp
// Write
map[2, 5] = 1;

// Read
int value = map[2, 5];

// It uses 'ref', so you can modify structs in-place!
ref int cell = ref map[2, 5];
cell++;
```

### 3. High Performance Iteration

If you need to process the whole map (e.g., pathfinding cleanup), don't use X/Y loops. Use the flat array!

```csharp
// ❌ Slow (CPU jumps around)
for (int y = 0; y < map.Height; y++)
    for (int x = 0; x < map.Width; x++)
        map[x, y] = 0;

// ✅ Fast (Linear memory access)
for (int i = 0; i < map.Length; i++)
    map[i] = 0;

// 🚀 Turbo (Span/MemSet)
map.Data.AsSpan().Fill(0);
```

### 4. Rows and Columns

```csharp
// Get Row 3 as a Span (Zero allocation view)
Span<int> row = map.GetRow(3);

// Copy Column 2 into a buffer
Span<int> colBuffer = stackalloc int[map.Height];
map.CopyColumn(2, colBuffer);
```

---

## 🔧 API Reference

### `Grid2D<T>`
- `Data`: The raw `T[]` array.
- `Width` / `Height`
- `this[x, y]`: Access cell by coordinate.
- `this[i]`: Access cell by flat index.
- `IsValid(x, y)`: Bounds check.

### Extensions
- `GetRow(y)`: Returns `Span<T>`.
- `CopyColumn(x, dest)`: Copies column data.
- `Fill(value)`: Sets every cell.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
