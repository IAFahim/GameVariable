# 🎮 Variable.Input - Combo System

**The "Street Fighter" Engine.** 🥊

**Variable.Input** is a combo system that reads like a story. It turns complex button mashing into a simple tree of moves.

---

## 🧠 Mental Model

### The Combo Tree 🌳
Forget "finite state machines." Think of a **Tree**.
- **Root:** Idle (Standing still).
- **Branch 1:** Light Attack (LMB).
  - **Branch 1-A:** Light-Light (LMB).
  - **Branch 1-B:** Light-Heavy (RMB).
- **Branch 2:** Heavy Attack (RMB).

You are climbing this tree. If you press a button that has a branch, you climb. If you don't (or wait too long), you fall back to the Root (Idle).

### The "Buffer" 🧠
Players are faster than your animations.
- Player presses `Punch` -> `Punch` -> `Kick`.
- Character is still winding up the first Punch.
- **The Buffer** remembers the inputs. As soon as the first Punch finishes, it instantly triggers the second Punch.

---

## 👶 ELI5 (Explain Like I'm 5)

Without Variable.Input:
> **You:** "If player pressed A, and timer < 0.5, and animation is 'Punch1', then play 'Punch2'."
> **You:** "Wait, what if he pressed B? Or pressed A twice really fast?"
> **You:** *Cries in spaghetti code.* 🍝

With Variable.Input:
> **You:** "Here is the map of moves. Go."
> **Computer:** "He pressed A-A-B. Executing Combo #42."

---

## 📦 Installation

```bash
dotnet add package Variable.Input
```

---

## 🎮 Usage Guide

### 1. Define Moves (The Tree)

You can build the tree in code (or use our Unity tools).

```csharp
// Simple Combo: Idle -> Light -> Heavy
var builder = new ComboGraphBuilder();

// 0 is always Idle
builder.AddMove(0, "Idle");
builder.AddMove(100, "Light Attack");
builder.AddMove(200, "Heavy Finisher");

// Connect them
builder.Connect(0, Inputs.LMB, 100);   // Idle + Click -> Light
builder.Connect(100, Inputs.RMB, 200); // Light + R-Click -> Heavy

// Bake into optimized arrays
var (nodes, edges) = builder.Build();
```

### 2. The Loop (Input ➔ Action)

```csharp
// 1. Register Input
if (Input.GetMouseButtonDown(0)) buffer.RegisterInput(Inputs.LMB);

// 2. Update State
// This checks the buffer against the graph
if (state.TryUpdate(ref buffer, graph, out int actionID))
{
    // 3. Do something!
    animator.SetInteger("ActionID", actionID);
    animator.SetTrigger("Attack");
}
```

### 3. Flow Control (Animation Events)

When the animation finishes (or hits a "cancel window"), tell the system.

```csharp
// Call this via Animation Event
public void OnAttackFinished()
{
    // Allows the NEXT move to trigger from the buffer
    state.SignalActionFinished();
    
    // Check buffer immediately (for frame-perfect chains)
    if (state.TryUpdate(ref buffer, graph, out int nextAction))
    {
        PlayAction(nextAction);
    }
}
```

---

## 🔧 API Reference

### `ComboGraphBuilder`
- Helper to create `ComboNode[]` and `ComboEdge[]`.

### `InputRingBuffer`
- Remembers the last N inputs.
- `.RegisterInput(id)`: Adds a press.
- `.Head`: Most recent input.

### `ComboState`
- `.CurrentNodeIndex`: Where we are in the tree.
- `.TryUpdate`: Attempts to transition.
- `.SignalActionFinished`: Unlocks the next move.
- `.Reset`: Go back to Idle.

---

<div align="center">

**Part of the [GameVariable](https://github.com/iafahim/GameVariable) Ecosystem**
*Made with ❤️ for game developers*

</div>
