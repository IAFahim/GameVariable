# Variable.Core

**Variable.Core** provides the fundamental interfaces and types for the **GameVariable** ecosystem. It defines the
contract that other variable types follow, ensuring consistency across your game's data structures.

## Installation

```bash
dotnet add package Variable.Core
```

## Features

* **IBoundedInfo**: The base interface for all variable types.
* **Common Extensions**: Shared utility methods.

## Usage

While rarely used directly in game logic, this package is essential if you are building custom variable types that
integrate with the ecosystem.

```csharp
using Variable.Core;

public struct MyCustomVariable : IBoundedInfo
{
    // Implementation
}
```

---
**Author:** Md Ishtiaq Ahamed Fahim  
**GitHub:** [iafahim/GameVariable](https://github.com/iafahim/GameVariable)
