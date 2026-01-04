# Variable.Input - Combo System

**A zero-allocation, Burst-compatible combo/skill system for Unity.**

---

## 🧠 The Mental Model: It's Just a Tree of Moves!

Forget the technical terms. Think of your combo system as a **tree of moves**:

```
         [Idle]
        /      \
    LMB/        \RMB
      /          \
 [Light]       [Heavy]
    |             |
   LMB           RMB
    |             |
[Light-Light] [Heavy-Heavy]
```

**That's it!** Each box is a "node" (a move), and each arrow is an "edge" (a button press that connects moves).

---

## 📖 Plain English Glossary

| Term | What It Actually Means |
|------|------------------------|
| **Node** | A single move/attack/ability (like "Light Punch" or "Heavy Kick") |
| **Edge** | "If I press THIS button, go to THAT move" |
| **ActionID** | A number you assign to identify the move (e.g., `100` = "Light Attack") |
| **InputTrigger** | Which button activates this transition (e.g., `LMB = 1`) |
| **TargetNodeIndex** | Which move to go to next (index in your nodes array) |
| **EdgeStartIndex** | Where this node's edges begin in the edges array |
| **EdgeCount** | How many edges (button options) this node has |

---

## 🎮 Example 1: Visual GameObject Tree (Beginner-Friendly)

**The easiest way to understand combos: Make each move a GameObject!**

### Setup in Hierarchy

```
Player
└── ComboTree
    ├── Node_Idle          (Index 0)
    ├── Node_LightAttack   (Index 1)
    ├── Node_HeavyAttack   (Index 2)
    ├── Node_LightLight    (Index 3)
    └── Node_HeavyHeavy    (Index 4)
```

### Step 1: Define Your Button IDs

```csharp
// Inputs.cs - Give each button a unique number
public static class Inputs
{
    public const int None = 0;
    public const int LMB = 1;   // Left Mouse Button
    public const int RMB = 2;   // Right Mouse Button
    public const int Space = 3;
}
```

### Step 2: Create a Simple Transition Component

```csharp
// ComboTransition.cs - Attach to each "move" GameObject
using UnityEngine;
using Variable.Input;

/// <summary>
/// Defines what buttons can be pressed from this move, and where they lead.
/// </summary>
[System.Serializable]
public class Transition
{
    [Tooltip("Which button press triggers this transition?")]
    public int InputButton;
    
    [Tooltip("Which GameObject/Move do we go to?")]
    public GameObject TargetNode;
}

public class ComboTransition : MonoBehaviour
{
    [Tooltip("Unique ID for this move (used by animation system)")]
    public int ActionID;
    
    [Tooltip("All possible moves you can chain into from here")]
    public Transition[] NextMoves;
}
```

### Step 3: The Visual Controller

```csharp
// VisualComboController.cs - Reads your GameObject tree and builds the graph
using UnityEngine;
using System.Collections.Generic;
using Variable.Input;

public class VisualComboController : MonoBehaviour
{
    [Header("=== SETUP ===")]
    [Tooltip("Drag all your move GameObjects here (in order!)")]
    public ComboTransition[] AllMoves;
    
    [Header("=== DEBUG (Read-Only) ===")]
    public int CurrentMoveIndex;
    public bool IsAttacking;
    public int BufferedInputs;
    
    // Internal
    private ComboGraphData _graph;
    private ComboState _state;
    private InputRingBuffer _buffer;
    private Dictionary<GameObject, int> _nodeToIndex;
    
    void Awake()
    {
        BuildGraphFromGameObjects();
    }
    
    void Update()
    {
        // 1. Capture button presses
        if (Input.GetMouseButtonDown(0)) _buffer.RegisterInput(Inputs.LMB);
        if (Input.GetMouseButtonDown(1)) _buffer.RegisterInput(Inputs.RMB);
        if (Input.GetKeyDown(KeyCode.Space)) _buffer.RegisterInput(Inputs.Space);
        
        // 2. Try to advance combo
        if (_state.TryUpdate(ref _buffer, _graph, out int actionID))
        {
            ExecuteMove(actionID);
        }
        
        // 3. Update debug view
        CurrentMoveIndex = _state.CurrentNodeIndex;
        IsAttacking = _state.IsActionBusy;
        BufferedInputs = _buffer.Count;
        
        // 4. Visual: Enable only the active node
        UpdateVisuals();
    }
    
    void BuildGraphFromGameObjects()
    {
        // Map each GameObject to an index
        _nodeToIndex = new Dictionary<GameObject, int>();
        for (int i = 0; i < AllMoves.Length; i++)
        {
            _nodeToIndex[AllMoves[i].gameObject] = i;
        }
        
        // Count total edges
        int totalEdges = 0;
        foreach (var move in AllMoves)
        {
            totalEdges += move.NextMoves?.Length ?? 0;
        }
        
        // Build arrays
        var nodes = new ComboNode[AllMoves.Length];
        var edges = new ComboEdge[totalEdges];
        int edgeIndex = 0;
        
        for (int i = 0; i < AllMoves.Length; i++)
        {
            var move = AllMoves[i];
            int edgeCount = move.NextMoves?.Length ?? 0;
            
            nodes[i] = new ComboNode
            {
                ActionID = move.ActionID,
                EdgeStartIndex = edgeIndex,
                EdgeCount = edgeCount
            };
            
            // Add edges
            if (move.NextMoves != null)
            {
                foreach (var transition in move.NextMoves)
                {
                    if (transition.TargetNode != null && 
                        _nodeToIndex.TryGetValue(transition.TargetNode, out int targetIdx))
                    {
                        edges[edgeIndex++] = new ComboEdge
                        {
                            InputTrigger = transition.InputButton,
                            TargetNodeIndex = targetIdx
                        };
                    }
                }
            }
        }
        
        _graph = new ComboGraphData { Nodes = nodes, Edges = edges };
    }
    
    void ExecuteMove(int actionID)
    {
        Debug.Log($"<color=cyan>Executing Move: {actionID}</color>");
        
        // TODO: Play animation, spawn VFX, deal damage, etc.
        // For demo, just wait a bit
        Invoke(nameof(OnMoveComplete), 0.5f);
    }
    
    void OnMoveComplete()
    {
        _state.SignalActionFinished();
        
        // Check if there's a buffered input waiting
        if (_state.TryUpdate(ref _buffer, _graph, out int nextAction))
        {
            ExecuteMove(nextAction);
        }
    }
    
    void UpdateVisuals()
    {
        // Enable only the active move's GameObject
        for (int i = 0; i < AllMoves.Length; i++)
        {
            AllMoves[i].gameObject.SetActive(i == _state.CurrentNodeIndex);
        }
    }
    
    /// <summary>
    /// Call this from Animation Events when your attack animation ends.
    /// </summary>
    public void OnAnimationEnd()
    {
        OnMoveComplete();
    }
}
```

### Step 4: Set Up in Unity Inspector

1. Create empty GameObjects for each move under `ComboTree`
2. Add `ComboTransition` to each
3. Set `ActionID` (e.g., Idle=0, Light=100, Heavy=200)
4. Drag target GameObjects into `NextMoves`
5. Add `VisualComboController` to Player
6. Drag all moves into `AllMoves` array **in order**

**Result:** You can now design combos by just dragging GameObjects in the Inspector!

---

## 🎮 Example 2: ScriptableObject Designer Tool

**For designers who want to create combo assets without code.**

### The Combo Asset

```csharp
// ComboAsset.cs - Create via Assets > Create > Game > Combo
using UnityEngine;
using System.Collections.Generic;
using Variable.Input;

[CreateAssetMenu(menuName = "Game/Combo", fileName = "NewCombo")]
public class ComboAsset : ScriptableObject
{
    [System.Serializable]
    public class Move
    {
        [Tooltip("Name for readability (not used in code)")]
        public string Name;
        
        [Tooltip("Unique ID for this move")]
        public int ActionID;
        
        [Tooltip("Transitions to other moves")]
        public List<MoveTransition> Transitions = new();
    }
    
    [System.Serializable]
    public class MoveTransition
    {
        [Tooltip("Button that triggers this (use your Inputs constants)")]
        public int Button;
        
        [Tooltip("Index of the target move in the Moves array")]
        public int GoToMoveIndex;
    }
    
    [Header("All Moves (Index 0 is always the starting point)")]
    public List<Move> Moves = new();
    
    /// <summary>
    /// Converts this designer-friendly format to the optimized runtime format.
    /// </summary>
    public ComboGraphData BuildGraph()
    {
        // Count edges
        int totalEdges = 0;
        foreach (var move in Moves)
            totalEdges += move.Transitions.Count;
        
        var nodes = new ComboNode[Moves.Count];
        var edges = new ComboEdge[totalEdges];
        int edgeIdx = 0;
        
        for (int i = 0; i < Moves.Count; i++)
        {
            var move = Moves[i];
            
            nodes[i] = new ComboNode
            {
                ActionID = move.ActionID,
                EdgeStartIndex = edgeIdx,
                EdgeCount = move.Transitions.Count
            };
            
            foreach (var t in move.Transitions)
            {
                edges[edgeIdx++] = new ComboEdge
                {
                    InputTrigger = t.Button,
                    TargetNodeIndex = t.GoToMoveIndex
                };
            }
        }
        
        return new ComboGraphData { Nodes = nodes, Edges = edges };
    }
    
    #if UNITY_EDITOR
    [ContextMenu("Validate")]
    void Validate()
    {
        for (int i = 0; i < Moves.Count; i++)
        {
            foreach (var t in Moves[i].Transitions)
            {
                if (t.GoToMoveIndex < 0 || t.GoToMoveIndex >= Moves.Count)
                {
                    Debug.LogError($"Move '{Moves[i].Name}' has invalid target index: {t.GoToMoveIndex}");
                }
            }
        }
        Debug.Log("<color=green>Validation passed!</color>");
    }
    #endif
}
```

### Using the Asset

```csharp
// ComboPlayer.cs
using UnityEngine;
using Variable.Input;

public class ComboPlayer : MonoBehaviour
{
    [SerializeField] ComboAsset comboAsset;
    [SerializeField] Animator animator;
    
    ComboGraphData _graph;
    ComboState _state;
    InputRingBuffer _buffer;
    
    void Awake()
    {
        _graph = comboAsset.BuildGraph();
    }
    
    void Update()
    {
        // Capture inputs
        if (Input.GetMouseButtonDown(0)) _buffer.RegisterInput(Inputs.LMB);
        if (Input.GetMouseButtonDown(1)) _buffer.RegisterInput(Inputs.RMB);
        
        // Process
        if (_state.TryUpdate(ref _buffer, _graph, out int actionID))
        {
            animator.SetTrigger($"Action_{actionID}");
        }
    }
    
    // Call from Animation Event at end of attack animation
    public void OnAttackEnd() => _state.SignalActionFinished();
}
```

### Sample Asset Configuration

```
Moves:
  [0] Name: "Idle"       ActionID: 0
      Transitions:
        - Button: 1 (LMB)  GoToMoveIndex: 1
        - Button: 2 (RMB)  GoToMoveIndex: 2
        
  [1] Name: "Light"      ActionID: 100
      Transitions:
        - Button: 1 (LMB)  GoToMoveIndex: 3
        
  [2] Name: "Heavy"      ActionID: 200
      Transitions:
        - Button: 2 (RMB)  GoToMoveIndex: 4
        
  [3] Name: "LightLight" ActionID: 101
      Transitions: (empty - combo finisher)
      
  [4] Name: "HeavyHeavy" ActionID: 201
      Transitions: (empty - combo finisher)
```

---

## 🎮 Example 3: Pure Code (Advanced/ECS)

**For programmers who want maximum control and performance.**

### Direct Graph Construction

```csharp
// DirectComboBuilder.cs
using Variable.Input;

public static class DirectComboBuilder
{
    /// <summary>
    /// Builds: Idle --LMB--> Light --LMB--> LightLight
    ///              --RMB--> Heavy --RMB--> HeavyHeavy
    /// </summary>
    public static ComboGraphData Build()
    {
        // NODES: Each move in your combo tree
        // =====================================
        // Index 0: Idle (starting point, always index 0)
        // Index 1: Light Attack
        // Index 2: Heavy Attack  
        // Index 3: Light-Light (finisher)
        // Index 4: Heavy-Heavy (finisher)
        
        var nodes = new ComboNode[]
        {
            // Node 0: Idle
            // - ActionID: What to do when entering this state (0 = idle/nothing)
            // - EdgeStartIndex: Where do MY edges start in the edges array? (index 0)
            // - EdgeCount: How many buttons can I press from here? (2: LMB or RMB)
            new ComboNode { ActionID = 0,   EdgeStartIndex = 0, EdgeCount = 2 },
            
            // Node 1: Light Attack
            // - ActionID: 100 (you'll check this to play "light attack" animation)
            // - EdgeStartIndex: My edges start at index 2 in edges array
            // - EdgeCount: 1 transition available (LMB to Light-Light)
            new ComboNode { ActionID = 100, EdgeStartIndex = 2, EdgeCount = 1 },
            
            // Node 2: Heavy Attack
            new ComboNode { ActionID = 200, EdgeStartIndex = 3, EdgeCount = 1 },
            
            // Node 3: Light-Light (no further combos)
            new ComboNode { ActionID = 101, EdgeStartIndex = 4, EdgeCount = 0 },
            
            // Node 4: Heavy-Heavy (no further combos)
            new ComboNode { ActionID = 201, EdgeStartIndex = 4, EdgeCount = 0 },
        };
        
        // EDGES: "If button X pressed, go to node Y"
        // ==========================================
        // These are stored FLAT for performance (CSR format)
        // Node 0's edges are at index 0-1
        // Node 1's edges are at index 2
        // Node 2's edges are at index 3
        
        var edges = new ComboEdge[]
        {
            // Edges for Node 0 (Idle) - starts at index 0
            new ComboEdge { InputTrigger = Inputs.LMB, TargetNodeIndex = 1 }, // LMB -> Light
            new ComboEdge { InputTrigger = Inputs.RMB, TargetNodeIndex = 2 }, // RMB -> Heavy
            
            // Edges for Node 1 (Light) - starts at index 2
            new ComboEdge { InputTrigger = Inputs.LMB, TargetNodeIndex = 3 }, // LMB -> Light-Light
            
            // Edges for Node 2 (Heavy) - starts at index 3
            new ComboEdge { InputTrigger = Inputs.RMB, TargetNodeIndex = 4 }, // RMB -> Heavy-Heavy
            
            // Nodes 3 & 4 have no edges (EdgeCount = 0), so nothing here for them
        };
        
        return new ComboGraphData { Nodes = nodes, Edges = edges };
    }
}
```

### Understanding EdgeStartIndex and EdgeCount

```
VISUAL REPRESENTATION:

Nodes Array:
┌─────┬─────────┬────────────────┬───────────┐
│ Idx │ ActionID│ EdgeStartIndex │ EdgeCount │
├─────┼─────────┼────────────────┼───────────┤
│  0  │    0    │       0        │     2     │ ← Idle: edges at [0,1]
│  1  │   100   │       2        │     1     │ ← Light: edge at [2]
│  2  │   200   │       3        │     1     │ ← Heavy: edge at [3]
│  3  │   101   │       4        │     0     │ ← LL: no edges
│  4  │   201   │       4        │     0     │ ← HH: no edges
└─────┴─────────┴────────────────┴───────────┘

Edges Array:
┌─────┬──────────────┬─────────────────┐
│ Idx │ InputTrigger │ TargetNodeIndex │
├─────┼──────────────┼─────────────────┤
│  0  │     LMB      │        1        │ ← Idle's 1st edge
│  1  │     RMB      │        2        │ ← Idle's 2nd edge
│  2  │     LMB      │        3        │ ← Light's edge
│  3  │     RMB      │        4        │ ← Heavy's edge
└─────┴──────────────┴─────────────────┘

When at Node 0 (Idle):
  - EdgeStartIndex = 0, EdgeCount = 2
  - Check edges[0] and edges[1] for matching input
  - If LMB pressed: edges[0] matches → go to node 1
  - If RMB pressed: edges[1] matches → go to node 2
```

### Mermaid Diagram: CSR Structure

```mermaid
graph LR
    subgraph Nodes["Nodes Array"]
        direction TB
        N0["Idx 0: Idle<br/>Start=0, Count=2"]
        N1["Idx 1: Light<br/>Start=2, Count=1"]
        N2["Idx 2: Heavy<br/>Start=3, Count=1"]
    end

    subgraph Edges["Edges Array"]
        direction TB
        E0["Idx 0: LMB -> Node 1"]
        E1["Idx 1: RMB -> Node 2"]
        E2["Idx 2: LMB -> Node 3"]
        E3["Idx 3: RMB -> Node 4"]
    end

    N0 -.-> E0
    N0 -.-> E1
    N1 -.-> E2
    N2 -.-> E3

    style N0 fill:#2d2d2d,stroke:#fff,stroke-width:2px,color:#fff
    style E0 fill:#1a1a1a,stroke:#666,stroke-width:1px,color:#fff
    style E1 fill:#1a1a1a,stroke:#666,stroke-width:1px,color:#fff
```

### ECS/Jobs Usage

```csharp
// For Unity DOTS - works with NativeArray and BlobAssets
using Unity.Collections;
using Unity.Jobs;
using Variable.Input;

public struct ProcessComboJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<ComboNode> Nodes;
    [ReadOnly] public NativeArray<ComboEdge> Edges;
    public NativeArray<ComboState> States;
    public NativeArray<InputRingBuffer> Buffers;
    public NativeArray<int> ResultActions;
    
    public void Execute(int index)
    {
        var state = States[index];
        var buffer = Buffers[index];
        
        if (ComboLogic.TryAdvanceState(ref state, ref buffer, Nodes, Edges, out int action))
        {
            ResultActions[index] = action;
        }
        else
        {
            ResultActions[index] = -1;
        }
        
        States[index] = state;
        Buffers[index] = buffer;
    }
}
```

---

## 🔧 API Quick Reference

```csharp
// Register a button press
buffer.RegisterInput(Inputs.LMB);

// Try to advance combo (returns true if a new action triggered)
if (state.TryUpdate(ref buffer, graph, out int actionID))
{
    // actionID is the ActionID of the new current node
    PlayAnimation(actionID);
}

// Signal that current action/animation finished
state.SignalActionFinished();

// Reset to idle
state.Reset();

// Clear buffered inputs
buffer.Clear();
```

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| **Zero GC** | No allocations during gameplay |
| **Burst-Compatible** | Works with Unity Jobs and Burst compiler |
| **Input Buffering** | Stores up to 8 inputs for responsive combos |
| **Crash-Proof** | Validates all indices, auto-recovers from bad state |
| **Framework-Agnostic** | Core logic is pure C#, no Unity dependency |

---

## 📁 Files Overview

| File | Purpose |
|------|---------|
| `ComboGraph.cs` | Container for nodes + edges (Unmanaged) |
| `ComboGraphData` | Managed container for serialization |
| `ComboNode.cs` | A single move (ActionID + edge pointers) |
| `ComboEdge.cs` | A transition (button → target node) |
| `ComboState.cs` | Runtime state (current node + busy flag) |
| `InputRingBuffer.cs` | Stores buffered button presses |
| `ComboLogic.*.cs` | Core traversal logic (stateless) |
| `*Extensions.cs` | Convenience methods |

---

## 🎯 Best Practices

1. **Index 0 is always "Idle"** - The starting/reset point
2. **Use meaningful ActionIDs** - e.g., `100-199` for light attacks, `200-299` for heavy
3. **Call `SignalActionFinished()`** from Animation Events for precise timing
4. **Buffer inputs generously** - Players appreciate responsive input
5. **Validate graphs in Editor** - Check for invalid indices before shipping

---

**See [UNITY_GUIDE.md](UNITY_GUIDE.md) for ECS, Jobs, and advanced patterns.** 🎮
