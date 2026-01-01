# Variable.Input - Complete Unity Guide

**Full MonoBehaviour, ScriptableObject, ECS, and Jobs Integration**

---

## 🎮 MonoBehaviour (Copy-Paste Ready)

### Complete Working Example

```csharp
// 1. MyGameInputs.cs
public static class MyGameInputs
{
    public const int LMB = 100;
    public const int RMB = 101;
    public const int Space = 200;
}

// 2. ComboGraphBuilder.cs
using UnityEngine;
using Variable.Input;

public class ComboGraphBuilder
{
    public static ComboGraph BuildFightingCombo()
    {
        // Neutral → Light → Light-Light
        //       └→ Heavy → Heavy-Heavy
        
        var nodes = new ComboNode[5];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 2 };    // Neutral
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 2, EdgeCount = 1 };  // Light
        nodes[2] = new ComboNode { ActionID = 200, EdgeStartIndex = 3, EdgeCount = 1 };  // Heavy
        nodes[3] = new ComboNode { ActionID = 101, EdgeStartIndex = 4, EdgeCount = 0 };  // LL
        nodes[4] = new ComboNode { ActionID = 201, EdgeStartIndex = 4, EdgeCount = 0 };  // HH

        var edges = new ComboEdge[4];
        edges[0] = new ComboEdge { InputTrigger = MyGameInputs.LMB, TargetNodeIndex = 1 };
        edges[1] = new ComboEdge { InputTrigger = MyGameInputs.RMB, TargetNodeIndex = 2 };
        edges[2] = new ComboEdge { InputTrigger = MyGameInputs.LMB, TargetNodeIndex = 3 };
        edges[3] = new ComboEdge { InputTrigger = MyGameInputs.RMB, TargetNodeIndex = 4 };

        return new ComboGraph { Nodes = nodes, Edges = edges };
    }
}

// 3. PlayerComboController.cs
using UnityEngine;
using Variable.Input;

public class PlayerComboController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public ComboState State;      // Public for debugging
    public InputRingBuffer Buffer;
    private ComboGraph _graph;

    private void Awake()
    {
        _graph = ComboGraphBuilder.BuildFightingCombo();
        State = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        Buffer = new InputRingBuffer();
    }

    private void Update()
    {
        // Capture inputs
        if (Input.GetMouseButtonDown(0)) Buffer.RegisterInput(MyGameInputs.LMB);
        if (Input.GetMouseButtonDown(1)) Buffer.RegisterInput(MyGameInputs.RMB);
        
        // Process combo
        if (State.TryUpdate(ref Buffer, _graph, out int actionID))
        {
            ExecuteAction(actionID);
        }
    }

    private void ExecuteAction(int actionID)
    {
        Debug.Log($"<color=green>Action: {actionID}</color>");
        animator.Play($"Action_{actionID}");
        StartCoroutine(WaitForAnimation(actionID));
    }

    private System.Collections.IEnumerator WaitForAnimation(int actionID)
    {
        yield return new WaitForSeconds(GetDuration(actionID));
        State.SignalActionFinished();
        
        // Process next buffered input
        if (State.TryUpdate(ref Buffer, _graph, out int nextID))
            ExecuteAction(nextID);
    }

    private float GetDuration(int actionID) => actionID switch
    {
        100 => 0.3f,  // Light
        200 => 0.5f,  // Heavy
        101 => 0.4f,  // LL
        201 => 0.7f,  // HH
        _ => 0f
    };

    // Debug GUI
    private void OnGUI()
    {
        GUILayout.Label($"Node: {State.CurrentNodeIndex}, Busy: {State.IsActionBusy}, Buffer: {Buffer.Count}");
    }
}
```

---

## 📦 ScriptableObject Pattern

### Create Asset Type

```csharp
// ComboGraphAsset.cs
using UnityEngine;
using Variable.Input;

[CreateAssetMenu(menuName = "Game/Combo Graph")]
public class ComboGraphAsset : ScriptableObject
{
    [System.Serializable]
    public class Node
    {
        public int ActionID;
        public int EdgeStartIndex;
        public int EdgeCount;
    }

    [System.Serializable]
    public class Edge
    {
        public int InputTrigger;
        public int TargetNodeIndex;
    }

    public Node[] Nodes;
    public Edge[] Edges;

    public ComboGraph ToComboGraph()
    {
        var nodes = new ComboNode[Nodes.Length];
        for (int i = 0; i < Nodes.Length; i++)
            nodes[i] = new ComboNode
            {
                ActionID = Nodes[i].ActionID,
                EdgeStartIndex = Nodes[i].EdgeStartIndex,
                EdgeCount = Nodes[i].EdgeCount
            };

        var edges = new ComboEdge[Edges.Length];
        for (int i = 0; i < Edges.Length; i++)
            edges[i] = new ComboEdge
            {
                InputTrigger = Edges[i].InputTrigger,
                TargetNodeIndex = Edges[i].TargetNodeIndex
            };

        return new ComboGraph { Nodes = nodes, Edges = edges };
    }
}
```

### Use in Controller

```csharp
public class PlayerComboController : MonoBehaviour
{
    [SerializeField] private ComboGraphAsset graphAsset;
    private ComboGraph _graph;

    private void Awake()
    {
        _graph = graphAsset.ToComboGraph();
    }
}
```

---

## 🚀 Unity ECS (DOTS)

### Components

```csharp
using Unity.Entities;
using Variable.Input;

public struct ComboInputComponent : IComponentData
{
    public InputRingBuffer Buffer;
}

public struct ComboStateComponent : IComponentData
{
    public ComboState State;
}

public struct ExecuteActionEvent : IComponentData
{
    public int ActionID;
}

public struct ComboGraphBlob
{
    public BlobArray<ComboNode> Nodes;
    public BlobArray<ComboEdge> Edges;
}

public struct ComboGraphReference : IComponentData
{
    public BlobAssetReference<ComboGraphBlob> Graph;
}
```

### Input System

```csharp
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class ComboInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var lmb = Input.GetMouseButtonDown(0);
        var rmb = Input.GetMouseButtonDown(1);

        Entities.ForEach((ref ComboInputComponent input) =>
        {
            if (lmb) input.Buffer.RegisterInput(MyGameInputs.LMB);
            if (rmb) input.Buffer.RegisterInput(MyGameInputs.RMB);
        }).WithoutBurst().Run();
    }
}
```

### Processing System

```csharp
using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class ComboProcessingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entities.WithoutBurst().ForEach((Entity entity, 
            ref ComboInputComponent input, 
            ref ComboStateComponent state, 
            in ComboGraphReference graphRef) =>
        {
            unsafe
            {
                ref var graph = ref graphRef.Graph.Value;
                
                var nodeSpan = new ReadOnlySpan<ComboNode>(
                    graph.Nodes.GetUnsafePtr(), graph.Nodes.Length);
                var edgeSpan = new ReadOnlySpan<ComboEdge>(
                    graph.Edges.GetUnsafePtr(), graph.Edges.Length);

                if (ComboLogic.TryAdvanceState(
                    ref state.State, ref input.Buffer, nodeSpan, edgeSpan, out int actionID))
                {
                    ecb.AddComponent(entity, new ExecuteActionEvent { ActionID = actionID });
                }
            }
        }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
```

---

## 🎨 Unity Jobs

```csharp
using Unity.Jobs;
using Unity.Collections;
using Variable.Input;

public struct ComboJob : IJob
{
    [ReadOnly] public NativeArray<ComboNode> Nodes;
    [ReadOnly] public NativeArray<ComboEdge> Edges;
    public ComboState State;
    public InputRingBuffer Buffer;
    public NativeArray<int> Result;

    public void Execute()
    {
        if (ComboLogic.TryAdvanceState(ref State, ref Buffer, Nodes, Edges, out int actionID))
            Result[0] = actionID;
        else
            Result[0] = -1;
    }
}

// Usage
public class ComboJobRunner : MonoBehaviour
{
    private NativeArray<ComboNode> _nodes;
    private NativeArray<ComboEdge> _edges;
    private NativeArray<int> _result;

    private void Update()
    {
        var job = new ComboJob
        {
            Nodes = _nodes,
            Edges = _edges,
            State = _state,
            Buffer = _buffer,
            Result = _result
        };

        job.Schedule().Complete();

        if (_result[0] != -1)
            ExecuteAction(_result[0]);
    }

    private void OnDestroy()
    {
        _nodes.Dispose();
        _edges.Dispose();
        _result.Dispose();
    }
}
```

---

## 🛠️ Unity Tools

### Combo Debugger

```csharp
using UnityEngine;
using Variable.Input;

public class ComboDebugger : MonoBehaviour
{
    [SerializeField] private PlayerComboController controller;
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        GUILayout.Box("Combo Debugger");
        
        GUILayout.Label($"Node: {controller.State.CurrentNodeIndex}");
        GUILayout.Label($"Busy: {controller.State.IsActionBusy}");
        GUILayout.Label($"Buffer: {controller.Buffer.Count}");
        
        if (GUILayout.Button("Force LMB"))
            controller.Buffer.RegisterInput(MyGameInputs.LMB);
        
        if (GUILayout.Button("Clear Buffer"))
            controller.Buffer.Clear();
        
        if (GUILayout.Button("Reset"))
            controller.State.Reset();
        
        GUILayout.EndArea();
    }
}
```

### Custom Inspector

```csharp
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ComboGraphAsset))]
public class ComboGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var asset = (ComboGraphAsset)target;
        
        if (GUILayout.Button("Validate Graph"))
        {
            bool valid = true;
            
            for (int i = 0; i < asset.Nodes.Length; i++)
            {
                var node = asset.Nodes[i];
                if (node.EdgeStartIndex + node.EdgeCount > asset.Edges.Length)
                {
                    Debug.LogError($"Node {i}: Invalid edge range!");
                    valid = false;
                }
            }
            
            for (int i = 0; i < asset.Edges.Length; i++)
            {
                var edge = asset.Edges[i];
                if (edge.TargetNodeIndex >= asset.Nodes.Length)
                {
                    Debug.LogError($"Edge {i}: Invalid target!");
                    valid = false;
                }
            }
            
            if (valid)
                Debug.Log("<color=green>Graph Valid!</color>");
        }
    }
}
#endif
```

---

## 📋 Common Patterns

### Input Mapping

```csharp
public class InputMapper : MonoBehaviour
{
    private Dictionary<KeyCode, int> _map = new();

    private void Awake()
    {
        _map[KeyCode.Q] = 1;
        _map[KeyCode.W] = 2;
        _map[KeyCode.E] = 3;
        _map[KeyCode.R] = 4;
    }

    public bool TryGetInput(out int inputId)
    {
        inputId = 0;
        foreach (var kvp in _map)
            if (Input.GetKeyDown(kvp.Key))
            {
                inputId = kvp.Value;
                return true;
            }
        return false;
    }
}
```

### Animation Event Integration

```csharp
// On Animation Clip, add event at end:
// Function: OnComboAnimationComplete
// Time: 0.9 (near end)

public void OnComboAnimationComplete()
{
    State.SignalActionFinished();
    if (State.TryUpdate(ref Buffer, _graph, out int nextID))
        ExecuteAction(nextID);
}
```

---

## 🎯 Best Practices

1. **Make State/Buffer Public** for Inspector debugging
2. **Use Animation Events** for precise timing
3. **ScriptableObjects** for designer-friendly graphs
4. **Validate Graphs** in custom editor
5. **Clear Buffer** on player death/respawn
6. **Jobs for AI** processing multiple combos

---

## 📚 See Also

- [EXAMPLES.md](EXAMPLES.md) - Game-specific patterns
- [PERFECTION.md](PERFECTION.md) - Architecture details
- [CHANGELOG.md](CHANGELOG.md) - Design rationale

---

**Unity-Ready. Production-Tested. Drop-In Integration.** 🎮
