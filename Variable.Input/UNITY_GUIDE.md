# Variable.Input - Advanced Unity Patterns

**ECS, Jobs, Animation Events, and Production Patterns**

> 📖 **New to this library?** Start with [README.md](README.md) for beginner-friendly examples.

---

## 🎬 Animation Event Integration

The most reliable way to chain combos is using Animation Events.

### Setup

1. Open your attack animation in Unity
2. Add an Animation Event at the end (or at the "cancellable" point)
3. Set function name to `OnComboWindowOpen`

```csharp
// ComboAnimationHandler.cs
using UnityEngine;
using Variable.Input;

public class ComboAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private ComboGraph _graph;
    private ComboState _state;
    private InputRingBuffer _buffer;
    
    void Update()
    {
        // Buffer inputs at any time
        if (Input.GetMouseButtonDown(0)) _buffer.RegisterInput(Inputs.LMB);
        if (Input.GetMouseButtonDown(1)) _buffer.RegisterInput(Inputs.RMB);
        
        // Only process when NOT busy (animation handles timing)
        if (!_state.IsActionBusy && _state.TryUpdate(ref _buffer, _graph, out int actionID))
        {
            PlayAction(actionID);
        }
    }
    
    void PlayAction(int actionID)
    {
        _state.IsActionBusy = true;
        animator.CrossFade($"Attack_{actionID}", 0.1f);
    }
    
    /// <summary>
    /// Called from Animation Event when attack can be cancelled into next move.
    /// </summary>
    public void OnComboWindowOpen()
    {
        _state.SignalActionFinished();
        
        // Immediately check for buffered input
        if (_state.TryUpdate(ref _buffer, _graph, out int nextAction))
        {
            PlayAction(nextAction);
        }
    }
    
    /// <summary>
    /// Called from Animation Event when attack fully completes.
    /// </summary>
    public void OnAttackComplete()
    {
        _state.SignalActionFinished();
        _state.Reset(); // Return to idle if no input buffered
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

public struct ComboActionEvent : IComponentData
{
    public int ActionID;
}

// Blob for shared graph data (memory efficient)
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
        bool lmb = Input.GetMouseButtonDown(0);
        bool rmb = Input.GetMouseButtonDown(1);
        
        if (!lmb && !rmb) return;
        
        Entities.ForEach((ref ComboInputComponent input) =>
        {
            if (lmb) input.Buffer.RegisterInput(Inputs.LMB);
            if (rmb) input.Buffer.RegisterInput(Inputs.RMB);
        }).WithoutBurst().Run();
    }
}
```

### Processing System

```csharp
using Unity.Entities;
using Unity.Collections;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class ComboProcessingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        
        Entities.ForEach((
            Entity entity,
            ref ComboInputComponent input,
            ref ComboStateComponent state,
            in ComboGraphReference graphRef) =>
        {
            ref var graph = ref graphRef.Graph.Value;
            
            // Convert BlobArrays to ReadOnlySpan
            ReadOnlySpan<ComboNode> nodes;
            ReadOnlySpan<ComboEdge> edges;
            unsafe
            {
                nodes = new ReadOnlySpan<ComboNode>(
                    graph.Nodes.GetUnsafePtr(), graph.Nodes.Length);
                edges = new ReadOnlySpan<ComboEdge>(
                    graph.Edges.GetUnsafePtr(), graph.Edges.Length);
            }
            
            if (ComboLogic.TryAdvanceState(
                ref state.State, 
                ref input.Buffer, 
                nodes, 
                edges, 
                out int actionID))
            {
                ecb.AddComponent(entity, new ComboActionEvent { ActionID = actionID });
            }
        }).WithoutBurst().Run();
        
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
```

### Creating BlobAssets

```csharp
using Unity.Entities;
using Unity.Collections;
using Variable.Input;

public static class ComboGraphBlobBuilder
{
    public static BlobAssetReference<ComboGraphBlob> Build(ComboGraph source)
    {
        var builder = new BlobBuilder(Allocator.Temp);
        
        ref var root = ref builder.ConstructRoot<ComboGraphBlob>();
        
        // Copy nodes
        var nodesArray = builder.Allocate(ref root.Nodes, source.Nodes.Length);
        for (int i = 0; i < source.Nodes.Length; i++)
            nodesArray[i] = source.Nodes[i];
        
        // Copy edges
        var edgesArray = builder.Allocate(ref root.Edges, source.Edges.Length);
        for (int i = 0; i < source.Edges.Length; i++)
            edgesArray[i] = source.Edges[i];
        
        var result = builder.CreateBlobAssetReference<ComboGraphBlob>(Allocator.Persistent);
        builder.Dispose();
        
        return result;
    }
}
```

---

## 🎨 Unity Jobs

### Single Combo Job

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
    public NativeReference<int> Result;
    
    public void Execute()
    {
        if (ComboLogic.TryAdvanceState(ref State, ref Buffer, Nodes, Edges, out int actionID))
            Result.Value = actionID;
        else
            Result.Value = -1;
    }
}
```

### Batch Processing (AI Enemies)

```csharp
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Variable.Input;

[BurstCompile]
public struct BatchComboJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<ComboNode> Nodes;
    [ReadOnly] public NativeArray<ComboEdge> Edges;
    public NativeArray<ComboState> States;
    public NativeArray<InputRingBuffer> Buffers;
    public NativeArray<int> Results;
    
    public void Execute(int index)
    {
        var state = States[index];
        var buffer = Buffers[index];
        
        if (ComboLogic.TryAdvanceState(ref state, ref buffer, Nodes, Edges, out int actionID))
            Results[index] = actionID;
        else
            Results[index] = -1;
        
        States[index] = state;
        Buffers[index] = buffer;
    }
}

// Usage
public class AIComboProcessor : MonoBehaviour
{
    private NativeArray<ComboNode> _nodes;
    private NativeArray<ComboEdge> _edges;
    private NativeArray<ComboState> _aiStates;
    private NativeArray<InputRingBuffer> _aiBuffers;
    private NativeArray<int> _results;
    
    private const int AI_COUNT = 100;
    
    void Start()
    {
        var graph = DirectComboBuilder.Build();
        
        _nodes = new NativeArray<ComboNode>(graph.Nodes, Allocator.Persistent);
        _edges = new NativeArray<ComboEdge>(graph.Edges, Allocator.Persistent);
        _aiStates = new NativeArray<ComboState>(AI_COUNT, Allocator.Persistent);
        _aiBuffers = new NativeArray<InputRingBuffer>(AI_COUNT, Allocator.Persistent);
        _results = new NativeArray<int>(AI_COUNT, Allocator.Persistent);
    }
    
    void Update()
    {
        // AI decision system would populate buffers here
        
        var job = new BatchComboJob
        {
            Nodes = _nodes,
            Edges = _edges,
            States = _aiStates,
            Buffers = _aiBuffers,
            Results = _results
        };
        
        job.Schedule(AI_COUNT, 32).Complete();
        
        // Process results
        for (int i = 0; i < AI_COUNT; i++)
        {
            if (_results[i] != -1)
            {
                // Trigger AI animation/action
            }
        }
    }
    
    void OnDestroy()
    {
        _nodes.Dispose();
        _edges.Dispose();
        _aiStates.Dispose();
        _aiBuffers.Dispose();
        _results.Dispose();
    }
}
```

---

## 🛠️ Editor Tools

### Graph Visualizer

```csharp
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Variable.Input;

public class ComboGraphWindow : EditorWindow
{
    private ComboAsset _asset;
    
    [MenuItem("Tools/Combo Graph Viewer")]
    static void Open() => GetWindow<ComboGraphWindow>("Combo Graph");
    
    void OnGUI()
    {
        _asset = EditorGUILayout.ObjectField("Asset", _asset, typeof(ComboAsset), false) as ComboAsset;
        
        if (_asset == null) return;
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Graph Structure", EditorStyles.boldLabel);
        
        for (int i = 0; i < _asset.Moves.Count; i++)
        {
            var move = _asset.Moves[i];
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.LabelField($"[{i}] {move.Name}", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"ActionID: {move.ActionID}");
            
            foreach (var t in move.Transitions)
            {
                string targetName = t.GoToMoveIndex < _asset.Moves.Count 
                    ? _asset.Moves[t.GoToMoveIndex].Name 
                    : "INVALID";
                EditorGUILayout.LabelField($"  → Button {t.Button} → [{t.GoToMoveIndex}] {targetName}");
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}
#endif
```

### Runtime Debugger

```csharp
using UnityEngine;
using Variable.Input;

public class ComboDebugOverlay : MonoBehaviour
{
    public ComboState State;
    public InputRingBuffer Buffer;
    public string[] MoveNames;
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 250, 200));
        GUILayout.Box("Combo Debug");
        
        string currentMove = State.CurrentNodeIndex < MoveNames.Length 
            ? MoveNames[State.CurrentNodeIndex] 
            : $"Node {State.CurrentNodeIndex}";
        
        GUILayout.Label($"Current: {currentMove}");
        GUILayout.Label($"Busy: {State.IsActionBusy}");
        GUILayout.Label($"Buffer: {Buffer.Count} inputs");
        
        if (GUILayout.Button("Reset"))
        {
            State.Reset();
            Buffer.Clear();
        }
        
        GUILayout.EndArea();
    }
}
```

---

## 📋 Production Patterns

### Input Mapping with New Input System

```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using Variable.Input;

public class NewInputSystemBridge : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    
    private InputAction _attackLight;
    private InputAction _attackHeavy;
    private InputRingBuffer _buffer;
    
    void Awake()
    {
        var combat = inputActions.FindActionMap("Combat");
        _attackLight = combat.FindAction("AttackLight");
        _attackHeavy = combat.FindAction("AttackHeavy");
        
        _attackLight.performed += _ => _buffer.RegisterInput(Inputs.LMB);
        _attackHeavy.performed += _ => _buffer.RegisterInput(Inputs.RMB);
    }
    
    void OnEnable()
    {
        _attackLight.Enable();
        _attackHeavy.Enable();
    }
    
    void OnDisable()
    {
        _attackLight.Disable();
        _attackHeavy.Disable();
    }
}
```

### Timeout/Reset Logic

```csharp
using UnityEngine;
using Variable.Input;

public class ComboWithTimeout : MonoBehaviour
{
    [SerializeField] private float comboTimeout = 1.5f;
    
    private ComboGraph _graph;
    private ComboState _state;
    private InputRingBuffer _buffer;
    private float _lastActionTime;
    
    void Update()
    {
        // Check timeout
        if (!_state.IsActionBusy && 
            _state.CurrentNodeIndex != 0 && 
            Time.time - _lastActionTime > comboTimeout)
        {
            _state.Reset();
            _buffer.Clear();
            Debug.Log("Combo timed out - reset to idle");
        }
        
        // Normal processing
        if (_state.TryUpdate(ref _buffer, _graph, out int actionID))
        {
            _lastActionTime = Time.time;
            ExecuteAction(actionID);
        }
    }
    
    void ExecuteAction(int actionID)
    {
        // ... play animation
    }
}
```

### Multiple Combo Graphs (Weapon Switching)

```csharp
using UnityEngine;
using Variable.Input;

public class WeaponComboController : MonoBehaviour
{
    [SerializeField] private ComboAsset swordCombo;
    [SerializeField] private ComboAsset spearCombo;
    [SerializeField] private ComboAsset fistCombo;
    
    private ComboGraph[] _graphs;
    private ComboState _state;
    private InputRingBuffer _buffer;
    private int _currentWeapon;
    
    void Awake()
    {
        _graphs = new ComboGraph[]
        {
            swordCombo.BuildGraph(),
            spearCombo.BuildGraph(),
            fistCombo.BuildGraph()
        };
    }
    
    public void SwitchWeapon(int weaponIndex)
    {
        _currentWeapon = weaponIndex;
        _state.Reset();
        _buffer.Clear();
    }
    
    void Update()
    {
        // ... input capture
        
        if (_state.TryUpdate(ref _buffer, _graphs[_currentWeapon], out int actionID))
        {
            ExecuteAction(actionID);
        }
    }
}
```

---

## 🎯 Performance Tips

| Tip | Why |
|-----|-----|
| Cache `ComboGraph` at Awake | Avoid rebuilding every frame |
| Use NativeArrays for Jobs | Zero GC, Burst compatible |
| Use BlobAssets for ECS | Shared memory, cache friendly |
| Process in batches | Parallel jobs for many entities |
| Use `ref` parameters | Avoid struct copies |

---

## 📚 Related Docs

- [README.md](README.md) - Beginner guide with visual examples
- API is fully documented with XML comments

---

**Production-Ready. Battle-Tested. Burst-Compatible.** 🎮
