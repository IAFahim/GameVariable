# 🎮 Variable.Input

**The Combo Tree.** 🌳

**Variable.Input** is a zero-allocation system for handling fighting game combos, skill chains, and input buffers. It uses a graph-based approach to turn "Button Mashes" into "Moves".

---

## 🧠 Mental Model: The Combo Tree

Forget "If Timer < 0.2 and Input == Punch".
Think of a **Tree**.
*   **Root:** Idle.
*   **Branch 1:** Punch (LMB).
*   **Branch 1-A:** Punch 2 (LMB).
*   **Branch 1-B:** Kick (RMB).

You are climbing the tree.
1.  You start at **Idle**.
2.  You press **LMB**. The system checks: "Does Idle have an LMB path?"
3.  Yes -> Move to **Punch 1**.
4.  You press **RMB**. The system checks: "Does Punch 1 have an RMB path?"
5.  Yes -> Move to **Kick**.

---

## 👶 ELI5: "How do I do the Super Move?"

To do a "Punch-Punch-Kick" combo:
1.  Game starts at "Ready".
2.  Player hits Punch. Game moves to "Punch 1 State".
3.  Player hits Punch. Game moves to "Punch 2 State".
4.  Player hits Kick. Game moves to "Super Kick State".

**Variable.Input** remembers where you are in the tree and handles the "Input Buffering" (remembering a button press for a split second so the combo feels smooth).

---

## 📦 Installation

```bash
dotnet add package Variable.Input
```

---

## 🚀 Usage Guide

### 1. Building the Tree (Graph)

You don't need to link pointers manually. Use the `ComboGraphBuilder`.

```csharp
using Variable.Input;

var builder = new ComboGraphBuilder();

// 1. Define Actions (Nodes)
int idle = builder.AddNode(ActionIds.Idle);
int punch1 = builder.AddNode(ActionIds.Punch1);
int punch2 = builder.AddNode(ActionIds.Punch2);
int kick = builder.AddNode(ActionIds.Kick);

// 2. Define Inputs (Edges)
// Idle -> Punch1 (Press A)
builder.AddEdge(idle, punch1, InputIds.AttackA);

// Punch1 -> Punch2 (Press A)
builder.AddEdge(punch1, punch2, InputIds.AttackA);

// Punch2 -> Kick (Press B)
builder.AddEdge(punch2, kick, InputIds.AttackB);

// 3. Bake the Graph
var (nodes, edges) = builder.Build();
var graph = new ComboGraph(nodes, edges);
```

### 2. Runtime Processing

In your Update loop, you just feed inputs into the buffer and ask the graph "Where do I go?"

```csharp
// State = Where am I? (Idle)
var state = new ComboState();

// Buffer = What did I just press?
var buffer = new InputRingBuffer();

void Update()
{
    // 1. Register Inputs
    if (Input.GetButtonDown("Fire1"))
        buffer.RegisterInput(InputIds.AttackA);

    // 2. Try to Advance Combo
    // "Can I move from my current node using the input in the buffer?"
    if (state.TryUpdate(ref buffer, graph, out int newAction))
    {
        // Yes! Play the animation for the new action.
        PlayAnimation(newAction);
    }
}
```

### 3. Finishing Moves

When an animation ends, you need to tell the system "I'm ready for the next move".

```csharp
// Call this via Animation Event
public void OnAnimationFinish()
{
    state.SignalActionFinished();
    
    // Check if player buffered an input while the animation was playing
    if (state.TryUpdate(ref buffer, graph, out int nextAction))
    {
        PlayAnimation(nextAction);
    }
    else
    {
        // No input? Go back to Idle.
        state.Reset();
    }
}
```

---

## 🔧 API Reference

### `ComboGraphBuilder`
- `AddNode(actionId)`: Creates a state.
- `AddEdge(from, to, trigger)`: Connects states.
- `Build()`: Returns optimized arrays.

### `ComboState`
- `TryUpdate(...)`: The brain. Checks buffer against graph.
- `SignalActionFinished()`: Unlocks the state for the next move.
- `Reset()`: Go back to start.

### `InputRingBuffer`
- `RegisterInput(id)`: Adds input to queue.
- `Capacity`: Holds last 8 inputs.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
