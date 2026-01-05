# Unity Hierarchy Combo Authoring

This guide explains how to use the Unity Hierarchy to author `ComboGraph` data while maintaining the strict
Data-Oriented Design (DOD) architecture.

## Concept

Instead of manually editing arrays or code, we use the Unity Scene Hierarchy to represent the Combo Tree.

- **GameObjects** represent **Nodes** (Actions).
- **Child GameObjects** represent **Edges** (Transitions).
- **Components** hold the data (`ActionID`, `InputTrigger`).

At runtime (or build time), we traverse this hierarchy using the `ComboGraphBuilder` to "bake" the flat, optimized
arrays.

## The Components

### 1. `ComboNodeAuthoring`

Place this on a GameObject to define a state/action.

```csharp
using UnityEngine;

public class ComboNodeAuthoring : MonoBehaviour
{
    public int ActionID;
    
    // Optional: Visuals to enable when this node is active
    public GameObject Visuals; 
}
```

### 2. `ComboEdgeAuthoring`

Place this on a child GameObject to define a transition to another node.

```csharp
using UnityEngine;

public class ComboEdgeAuthoring : MonoBehaviour
{
    public int InputTrigger;
    public ComboNodeAuthoring TargetNode; // Reference to the target in the hierarchy
}
```

## The Hierarchy Structure

```text
root (ComboGraphAuthoring)
 ├── Neutral (Node: ActionID 0)
 │    ├── ToLight (Edge: Input 1 -> Target: LightAttack)
 │    └── ToHeavy (Edge: Input 2 -> Target: HeavyAttack)
 │
 ├── LightAttack (Node: ActionID 100)
 │    ├── ToLightCombo (Edge: Input 1 -> Target: LightCombo)
 │    └── ToHeavyCombo (Edge: Input 2 -> Target: HeavyCombo)
 │
 ├── HeavyAttack (Node: ActionID 200)
 │    └── ...
 ...
```

## The Baker (Baking to DOD)

This script sits on the root and converts the hierarchy into the `ComboGraph` struct.

```csharp
using UnityEngine;
using Variable.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices; // Required for Marshal

public class ComboGraphBaker : MonoBehaviour
{
    public ComboNodeAuthoring RootNode;
    
    // The baked graph (Runtime Data)
    // We use a raw pointer wrapper or manage the lifecycle explicitly
    public ComboGraph Graph; 
    
    // Pointers to unmanaged memory
    private IntPtr _nodesPtr;
    private IntPtr _edgesPtr;

    private void Awake()
    {
        Bake();
    }

    private void OnDestroy()
    {
        // CRITICAL: Free unmanaged memory to avoid leaks
        if (_nodesPtr != IntPtr.Zero) Marshal.FreeHGlobal(_nodesPtr);
        if (_edgesPtr != IntPtr.Zero) Marshal.FreeHGlobal(_edgesPtr);
    }

    public unsafe void Bake()
    {
        // 1. Build using the helper (creates temporary managed arrays)
        var builder = new ComboGraphBuilder();
        // ... (Collection logic same as above) ...
        
        // (Assume we collected nodes/edges into the builder)
        // For this example, let's assume we have the result:
        var (managedNodes, managedEdges) = builder.Build();
        
        // 2. Allocate Unmanaged Memory (The "Smart" DOD way)
        // We move data off the GC heap immediately.
        
        int nodeSize = sizeof(ComboNode) * managedNodes.Length;
        int edgeSize = sizeof(ComboEdge) * managedEdges.Length;

        _nodesPtr = Marshal.AllocHGlobal(nodeSize);
        _edgesPtr = Marshal.AllocHGlobal(edgeSize);

        // 3. Copy Data (Blit)
        // Using fixed to get source pointers, then memcpy
        fixed (ComboNode* srcNodes = managedNodes)
        {
            Buffer.MemoryCopy(srcNodes, (void*)_nodesPtr, nodeSize, nodeSize);
        }
        
        fixed (ComboEdge* srcEdges = managedEdges)
        {
            Buffer.MemoryCopy(srcEdges, (void*)_edgesPtr, edgeSize, edgeSize);
        }

        // 4. Assign to Graph
        Graph = new ComboGraph
        {
            Nodes = (ComboNode*)_nodesPtr,
            NodeCount = managedNodes.Length,
            Edges = (ComboEdge*)_edgesPtr,
            EdgeCount = managedEdges.Length
        };
        
        Debug.Log($"Baked ComboGraph: {Graph.NodeCount} Nodes, {Graph.EdgeCount} Edges. (Unmanaged)");
    }
}
```

## Dynamic Updates (Smart Graph)

If you need to support adding moves at runtime (e.g., "On and when gameobject with input added"):

1. **Re-Baking**: Since the graph is a flat array, you cannot "insert" easily. The smartest path is to **Re-Bake**.
2. **Double Buffering**:
    * Allocate *New* Graph.
    * Bake.
    * Swap `Graph` reference.
    * Free *Old* Graph.

```csharp
public void OnNewMoveAdded()
{
    // 1. Bake new data to temp pointers
    // 2. Swap
    var oldNodes = _nodesPtr;
    var oldEdges = _edgesPtr;
    
    Bake(); // Allocates new pointers
    
    // 3. Free old
    if (oldNodes != IntPtr.Zero) Marshal.FreeHGlobal(oldNodes);
    if (oldEdges != IntPtr.Zero) Marshal.FreeHGlobal(oldEdges);
}
```

## Runtime Visualization (The "Enable" Lifecycle)

You mentioned using `OnEnable`/`OnDisable` to represent the active state. In DOD, logic is separated from data, but we
can use a **System** or **Controller** to sync the visual state.

```csharp
public class ComboVisualizer : MonoBehaviour
{
    public ComboGraphBaker Baker;
    public ComboState State; // This is the struct being updated by Logic
    
    private ComboNodeAuthoring[] _visualNodes;

    void Start()
    {
        // Cache the visual components corresponding to the graph indices
        _visualNodes = Baker.GetComponentsInChildren<ComboNodeAuthoring>();
    }

    void Update()
    {
        // Sync Hierarchy Active State with Graph State
        for (int i = 0; i < _visualNodes.Length; i++)
        {
            bool isActive = (i == State.CurrentNodeIndex);
            
            // This triggers OnEnable/OnDisable on the GameObject
            if (_visualNodes[i].gameObject.activeSelf != isActive)
            {
                _visualNodes[i].gameObject.SetActive(isActive);
            }
        }
    }
}
```

### How it works

1. **Input**: Player presses a button.
2. **Logic**: `ComboLogic.TryUpdate` runs (pure C#). It changes `State.CurrentNodeIndex` from `0` to `1`.
3. **Visualizer**: The `ComboVisualizer` sees the index changed.
    * It calls `SetActive(false)` on Node 0 (Neutral). -> **Triggers OnDisable**
    * It calls `SetActive(true)` on Node 1 (Light Attack). -> **Triggers OnEnable**

This gives you the Unity Lifecycle hooks you want for animations/VFX, while keeping the core logic in the
high-performance, burst-compatible struct.
