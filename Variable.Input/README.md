# Variable.Input - Unity Integration Guide

**Complete Unity documentation with MonoBehaviour, ScriptableObject, ECS, and Jobs examples.**

---

## 📚 Documentation

- **[UNITY_GUIDE.md](UNITY_GUIDE.md)** - Complete Unity integration (MonoBehaviour, ECS, Jobs, ScriptableObjects)
- **[EXAMPLES.md](EXAMPLES.md)** - Game patterns (FPS, Fighting, MOBA)
- **[PERFECTION.md](PERFECTION.md)** - Architecture & safety details

---

## ⚡ Quick Start (Copy-Paste Ready)

### 1. Define Inputs
```csharp
public static class MyGameInputs {
    public const int LMB = 100;
    public const int RMB = 101;
}
```

### 2. Build Graph
```csharp
var graph = new ComboGraph {
    Nodes = new[] {
        new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 1 }
    },
    Edges = new[] {
        new ComboEdge { InputTrigger = MyGameInputs.LMB, TargetNodeIndex = 1 }
    }
};
```

### 3. Controller
```csharp
public class PlayerCombo : MonoBehaviour {
    private ComboState _state;
    private InputRingBuffer _buffer;
    private ComboGraph _graph;

    void Update() {
        if (Input.GetMouseButtonDown(0))
            _buffer.RegisterInput(MyGameInputs.LMB);
        
        if (_state.TryUpdate(ref _buffer, _graph, out int actionID))
            ExecuteAction(actionID);
    }
}
```

---

## ✨ Features

✅ **Zero GC** — No allocations in gameplay  
✅ **Crash-Proof** — Safe against corrupt data  
✅ **Unity-Native** — MonoBehaviour, ScriptableObject, Animation Events  
✅ **ECS-Ready** — Full DOTS support with BlobAssets  
✅ **Jobs-Compatible** — NativeArray support out of box  
✅ **Universal Inputs** — Integer IDs, no enum lock-in  

---

**See [UNITY_GUIDE.md](UNITY_GUIDE.md) for complete examples.** 🎮
