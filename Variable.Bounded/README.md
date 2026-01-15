# Variable.Bounded

**Variable.Bounded** offers high-performance structs for handling values that must stay within a specific range (Min/Max). Perfect for Health, Stamina, Mana, and other "bar-based" resources.

## Installation

```bash
dotnet add package Variable.Bounded
```

## Features

* **BoundedFloat / BoundedInt / BoundedByte**: Automatically clamps values.
* **Allocation-Free**: Implemented as `struct` for zero garbage collection overhead.
* **Operator Overloads**: Use `+`, `-`, `++`, `--` naturally.
* **Extensions**: `IsFull()`, `IsEmpty()`, `GetRatio()`, `Normalize()`.

## Usage

### 1. Basic Construction

```csharp
using Variable.Bounded;

// 1. Max only (Min=0, Current=Max)
// Useful for health starting full.
var health = new BoundedFloat(100f);
// Min: 0, Max: 100, Current: 100

// 2. Max + Current (Min=0)
// Useful for loading saved state.
var mana = new BoundedFloat(100f, 50f);
// Min: 0, Max: 100, Current: 50

// 3. Min + Max + Current
// Useful for temperature or custom ranges.
var temp = new BoundedFloat(50f, -50f, 20f);
// Min: -50, Max: 50, Current: 20
```

### 2. Operators & Arithmetic

Operations automatically clamp the result.

```csharp
var hp = new BoundedFloat(100f);

hp -= 25f;       // Current: 75
hp = hp / 2f;    // Current: 37.5
hp += 1000f;     // Current: 100 (Clamped to Max)
hp -= 1000f;     // Current: 0   (Clamped to Min)
```

### 3. Queries

```csharp
if (hp.IsEmpty()) {
    // Current == Min
    Die();
}

if (hp.IsFull()) {
    // Current == Max
    ShowFullHealthEffect();
}

// Get percentage (0.0 to 1.0)
float healthBarFill = hp.GetRatio();
```

### 4. Implicit Conversion

You can treat the struct as its underlying primitive type for reading.

```csharp
float currentHp = hp; // Implicitly returns hp.Current

if (hp > 50f) {
    // Works because of implicit conversion + float comparison
}
```

### 5. String Formatting

```csharp
Console.WriteLine(hp);            // Output: "100/100"
Console.WriteLine($"{hp:R}");     // Output: "100.0%" (Ratio format)
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
