# 🎮 Variable.Input

**Hadouken!** A deterministic Input Buffer and Combo System for fighting games and action RPGs.

`Variable.Input` isn't just an input reader; it's a full-blown Combo Graph system. It handles input buffering (remembering a button press for a few frames), sequence recognition (A -> A -> B), and complex branching combos.

[![NuGet](https://img.shields.io/nuget/v/Variable.Input?color=blue&label=NuGet)](https://www.nuget.org/packages/Variable.Input)

## 📦 Installation

```bash
dotnet add package Variable.Input
```

## 🔥 Features

* **🕸️ Combo Graphs**: Define complex movesets as a graph of nodes and transitions.
* **🧠 Input Buffering**: Stores inputs in a Ring Buffer so players don't have to be frame-perfect.
* **⚡ Zero Allocation**: Designed for high-performance loops (Unity DOTS/ECS compatible).
* **🔮 Deterministic**: Great for rollback netcode or replay systems.

## 📚 Guides

* [**Unity Integration Guide**](./UNITY_GUIDE.md): How to use this with Unity's Input System.
* [**Visualizing Combos**](./DIAGRAM.md): Learn how the graph structure works.

## 🛠️ Usage Guide

### 1. The Input Ring Buffer
This stores the history of player inputs. It's a circular buffer of fixed size (power of 2).

```csharp
using Variable.Input;

// Create a buffer with capacity 16
var buffer = new InputRingBuffer16();

// Add an input (e.g., when player presses 'X')
buffer.Add(new InputTriggerId(1), Time.time);

// Check if input happened recently (within 0.2s)
if (buffer.HasInput(new InputTriggerId(1), 0.2f, Time.time))
{
    Attack();
}
```

### 2. Building a Combo Graph
Define how inputs transition between states.

```csharp
var builder = new ComboGraphBuilder();

// Define States
var idle = builder.CreateNode(0);
var punch1 = builder.CreateNode(1);
var punch2 = builder.CreateNode(2);
var kick = builder.CreateNode(3);

// Define Transitions (Edges)
// From Idle -> Punch1 on 'X' button
builder.Connect(idle, punch1, new InputTriggerId('X'));

// From Punch1 -> Punch2 on 'X' button
builder.Connect(punch1, punch2, new InputTriggerId('X'));

// From Punch1 -> Kick on 'Y' button (Branching!)
builder.Connect(punch1, kick, new InputTriggerId('Y'));

var graph = builder.Build();
```

### 3. Running the Combo System
You need a `ComboState` to track where the player is in the graph.

```csharp
var state = new ComboState();

// In your Update loop:
// 1. Add inputs to buffer
if (Input.GetKeyDown(KeyCode.X))
    buffer.Add(new InputTriggerId('X'), time);

// 2. Tick the graph
// This checks the buffer for valid inputs to transition from current node
ComboState nextState = graph.Tick(state, ref buffer, time);

if (nextState.NodeId != state.NodeId)
{
    // State changed! Play animation for new node.
    PlayAnimation(nextState.NodeId);
    state = nextState;
}
```

## 🧩 Terminology

* **Node**: An animation state (e.g., "Idle", "Jab", "Uppercut").
* **Edge**: A valid transition triggered by an input.
* **Trigger**: The specific input ID (byte) that causes a transition.
* **Buffer**: A short-term memory of inputs to make controls feel responsive.

## 🤝 Contributing
Found a bug? PRs are welcome!
See the [Contributing Guide](../CONTRIBUTING.md) for details.

---
<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Zero-allocation, high-performance game logic.*

</div>
