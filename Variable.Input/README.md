# Variable.Input

**Deterministic Finite Automaton (DFA) Combo System** — AAA-grade, zero-allocation, DOTS-ready input buffer and graph traversal.

---

## Architecture

This package implements the **Data-Logic-Extension** pattern for a Combo Tree system:

### Layer A: Data (Structs)
- `InputId` — Static constants for input IDs (users define their own)
- `InputRingBuffer` — Fixed-size circular queue for buffered inputs (stores int IDs)
- `ComboNode` — Graph node with action ID and edge references
- `ComboEdge` — Graph edge with input trigger (int ID) and target
- `ComboGraph` — Complete graph (CSR format: nodes + edges)
- `ComboState` — Runtime traversal state

### Layer B: Logic (Static Methods)
- `ComboLogic` — Pure, stateless, primitive-only operations
  - Ring buffer operations (enqueue, dequeue, peek)
  - Graph traversal (advance state, signal finish)

### Layer C: Extensions (Sugar)
- `InputRingBufferExtensions` — Convenience methods for buffer
- `ComboStateExtensions` — Convenience methods for state

---

## Features

✅ **Zero Allocation** — No `new`, no `List<T>`, no garbage collection  
✅ **Array-Based** — CSR (Compressed Sparse Row) graph format  
✅ **DOTS Compatible** — Can swap arrays for `BlobArray<T>` in Unity DOTS  
✅ **Cache Coherent** — Sequential memory access for CPU efficiency  
✅ **Dial-Up Buffering** — 8-input ring buffer for rapid inputs  
✅ **Deterministic** — Same inputs = same output, every time  
✅ **Universal** — Integer IDs allow users to define any input (no enum limitation)

---

## Example Usage

```csharp
// 1. Define your input IDs (outside the DLL)
public static class MyInputs
{
    public const int LMB = 100;      // Left Mouse Button
    public const int RMB = 101;      // Right Mouse Button
    public const int Space = 102;    // Space (Dash)
    public const int Shift = 103;    // Shift (Sprint)
}

// 2. Define the graph
var graph = new ComboGraph
{
    Nodes = new[]
    {
        new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 2 },    // Idle
        new ComboNode { ActionID = 100, EdgeStartIndex = 2, EdgeCount = 1 },  // Light Attack
        new ComboNode { ActionID = 200, EdgeStartIndex = 3, EdgeCount = 0 }   // Heavy Attack
    },
    Edges = new[]
    {
        new ComboEdge { InputTrigger = MyInputs.LMB, TargetNodeIndex = 1 },
        new ComboEdge { InputTrigger = MyInputs.RMB, TargetNodeIndex = 2 },
        new ComboEdge { InputTrigger = MyInputs.Space, TargetNodeIndex = 0 }  // Dash cancel
    }
};

// 3. Initialize runtime state
var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
var buffer = new InputRingBuffer();

// 4. Register inputs (integer IDs)
buffer.RegisterInput(MyInputs.LMB);

// 5. Update state
if (state.TryUpdate(ref buffer, graph, out int actionID))
{
    Debug.Log($"Execute Action: {actionID}");
    // Play animation, trigger effect, etc.
}

// 6. Signal completion
state.SignalActionFinished();
```

---

## Why Integer IDs?

**Problem**: Using `enum` locks users into predefined input types. The DLL becomes inflexible.

**Solution**: Use `int` IDs. Users define their own constants:
- `100-199` = Mouse inputs
- `200-299` = Keyboard inputs  
- `300-399` = Gamepad inputs
- `400+` = Custom/network inputs

**Example**:
```csharp
public static class FPSInputs
{
    public const int LMB = 100;
    public const int RMB = 101;
    public const int Reload = 200;
    public const int Crouch = 201;
}

public static class FightingGameInputs
{
    public const int LightPunch = 1;
    public const int HeavyPunch = 2;
    public const int LightKick = 3;
    public const int HeavyKick = 4;
}
```

---

## Graph Construction

The graph uses **CSR format**:
- `ComboNode.EdgeStartIndex` — Index into the `Edges` array
- `ComboNode.EdgeCount` — Number of edges for this node
- Edges are stored sequentially in `ComboGraph.Edges`

This enables linear scans for edge matching (fastest for N < 10 edges per node).

---

## Performance

- **Ring Buffer**: O(1) enqueue/dequeue
- **Graph Traversal**: O(E) where E = edges per node (typically < 5)
- **Memory**: Fixed structs, no heap allocations
- **Thread Safe**: Can be used in Burst-compiled jobs (Unity DOTS)

---

## Rules Enforced

1. **Data Layer** — Only structs, no logic
2. **Logic Layer** — Only primitives/arrays, no structs in parameters
3. **Extension Layer** — Sugar that decomposes structs before calling logic

---

## Testing

Run tests with:
```bash
dotnet test Variable.Input.Tests
```

Tests cover:
- Ring buffer FIFO ordering
- Wrap-around behavior
- Graph traversal (valid/invalid transitions)
- Combo chains
- Action busy state handling

All tests use integer IDs (1=Light, 2=Heavy for simplicity).

---

## Why This Design?

> "You don't need 1.5 years. You just need to structure your data correctly."

This architecture:
- Separates **data** from **logic** from **sugar**
- Enables **Unity DOTS migration** without rewriting core logic
- Guarantees **zero GC pressure** in gameplay loops
- Maintains **testability** (logic is pure functions)
- **Universal** — No enum coupling, users define their own input space

**Forged into steel.** 🔥
