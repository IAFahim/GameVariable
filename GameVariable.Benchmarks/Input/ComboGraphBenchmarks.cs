using BenchmarkDotNet.Attributes;
using Variable.Input;

namespace GameVariable.Benchmarks.Input;

[MemoryDiagnoser]
[ShortRunJob]
public class ComboStateBenchmarks
{
    [Benchmark]
    public ComboState Construction()
    {
        return new ComboState();
    }

    [Benchmark]
    public bool Equals_SameState()
    {
        var state1 = new ComboState { CurrentNodeIndex = 5, IsActionBusy = true };
        var state2 = new ComboState { CurrentNodeIndex = 5, IsActionBusy = true };
        return state1.Equals(state2);
    }

    [Benchmark]
    public int GetHashCode_State()
    {
        var state = new ComboState { CurrentNodeIndex = 5, IsActionBusy = true };
        return state.GetHashCode();
    }
}

[MemoryDiagnoser]
[ShortRunJob]
public class ComboNodeBenchmarks
{
    [Benchmark]
    public ComboNode Construction()
    {
        return new ComboNode { ActionID = 100, EdgeStartIndex = 0, EdgeCount = 2 };
    }
}

[MemoryDiagnoser]
[ShortRunJob]
public class ComboEdgeBenchmarks
{
    [Benchmark]
    public ComboEdge Construction()
    {
        return new ComboEdge { InputTrigger = 1, TargetNodeIndex = 2 };
    }

    [Benchmark]
    public bool Equals_SameEdge()
    {
        var edge1 = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 2 };
        var edge2 = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 2 };
        return edge1.Equals(edge2);
    }

    [Benchmark]
    public int GetHashCode_Edge()
    {
        var edge = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 2 };
        return edge.GetHashCode();
    }
}

[MemoryDiagnoser]
[ShortRunJob]
public class ComboLogicBenchmarks
{
    private ComboNode[] _nodes = null!;
    private ComboEdge[] _edges = null!;
    private int[] _buffer = null!;
    private int _currentNodeIndex;
    private bool _isActionBusy;
    private int _bufferHead;
    private int _bufferCount;

    [GlobalSetup]
    public void Setup()
    {
        // Create a simple combo graph:
        // Node 0 (Neutral) --Input 1--> Node 1 (LightAttack)
        // Node 1 (LightAttack) --Input 2--> Node 2 (LightCombo)
        _nodes = new ComboNode[]
        {
            new() { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 1 },
            new() { ActionID = 100, EdgeStartIndex = 1, EdgeCount = 1 },
            new() { ActionID = 101, EdgeStartIndex = 2, EdgeCount = 0 }
        };

        _edges = new ComboEdge[]
        {
            new() { InputTrigger = 1, TargetNodeIndex = 1 },
            new() { InputTrigger = 2, TargetNodeIndex = 2 }
        };

        _buffer = new int[8];
        _currentNodeIndex = 0;
        _isActionBusy = false;
        _bufferHead = 0;
        _bufferCount = 0;
    }

    [Benchmark]
    public bool TryEnqueueInput_HasSpace()
    {
        _bufferCount = 0;
        _bufferHead = 0;
        return ComboLogic.TryEnqueueInput(ref _bufferHead, ref _bufferCount, _buffer, 1);
    }

    [Benchmark]
    public bool TryDequeueInput_HasItems()
    {
        _bufferCount = 1;
        _bufferHead = 0;
        _buffer[0] = 1;
        return ComboLogic.TryDequeueInput(ref _bufferHead, ref _bufferCount, _buffer, out _);
    }

    [Benchmark]
    public bool PeekInput_HasItems()
    {
        _bufferCount = 1;
        _bufferHead = 0;
        _buffer[0] = 1;
        return ComboLogic.PeekInput(in _bufferHead, in _bufferCount, _buffer, out _);
    }

    [Benchmark]
    public bool TryAdvanceState_ValidTransition()
    {
        // Setup: Start at node 0, input 1 in buffer, not busy
        _currentNodeIndex = 0;
        _isActionBusy = false;
        _bufferHead = 0;
        _bufferCount = 1;
        _buffer[0] = 1;

        return ComboLogic.TryAdvanceState(
            ref _currentNodeIndex,
            ref _isActionBusy,
            ref _bufferHead,
            ref _bufferCount,
            _buffer,
            _nodes,
            _edges,
            out _);
    }

    [Benchmark]
    public bool TryAdvanceState_Busy_NoTransition()
    {
        // Setup: Start at node 0, input in buffer, but busy (should not transition)
        _currentNodeIndex = 0;
        _isActionBusy = true;
        _bufferHead = 0;
        _bufferCount = 1;
        _buffer[0] = 1;

        return ComboLogic.TryAdvanceState(
            ref _currentNodeIndex,
            ref _isActionBusy,
            ref _bufferHead,
            ref _bufferCount,
            _buffer,
            _nodes,
            _edges,
            out _);
    }

    [Benchmark]
    public bool TryAdvanceState_NoInput_NoTransition()
    {
        // Setup: Start at node 0, no input in buffer, not busy
        _currentNodeIndex = 0;
        _isActionBusy = false;
        _bufferHead = 0;
        _bufferCount = 0;

        return ComboLogic.TryAdvanceState(
            ref _currentNodeIndex,
            ref _isActionBusy,
            ref _bufferHead,
            ref _bufferCount,
            _buffer,
            _nodes,
            _edges,
            out _);
    }

    [Benchmark]
    public void SignalActionFinished()
    {
        _isActionBusy = true;
        ComboLogic.SignalActionFinished(ref _isActionBusy);
    }
}

[MemoryDiagnoser]
[ShortRunJob]
public class ComboGraphBuilderBenchmarks
{
    [Benchmark]
    public int AddNode_Single()
    {
        var builder = new ComboGraphBuilder();
        return builder.AddNode(100);
    }

    [Benchmark]
    public int AddNode_10Nodes()
    {
        var builder = new ComboGraphBuilder();
        int lastIndex = 0;
        for (int i = 0; i < 10; i++)
        {
            lastIndex = builder.AddNode(i * 100);
        }
        return lastIndex;
    }

    [Benchmark]
    public void AddEdge_Single()
    {
        var builder = new ComboGraphBuilder();
        int node0 = builder.AddNode(0);
        int node1 = builder.AddNode(100);
        builder.AddEdge(node0, node1, 1);
    }

    [Benchmark]
    public (ComboNode[] nodes, ComboEdge[] edges) Build_SimpleGraph_3Nodes_2Edges()
    {
        // Create a simple graph: Neutral -> LightAttack -> HeavyAttack
        var builder = new ComboGraphBuilder();
        int neutral = builder.AddNode(0);
        int lightAttack = builder.AddNode(100);
        int heavyAttack = builder.AddNode(200);

        builder.AddEdge(neutral, lightAttack, 1);
        builder.AddEdge(lightAttack, heavyAttack, 2);

        return builder.Build();
    }

    [Benchmark]
    public (ComboNode[] nodes, ComboEdge[] edges) Build_ComplexGraph_10Nodes_20Edges()
    {
        // Create a more complex combo graph
        var builder = new ComboGraphBuilder();

        // Create 10 nodes
        var nodeIndices = new int[10];
        for (int i = 0; i < 10; i++)
        {
            nodeIndices[i] = builder.AddNode(i * 100);
        }

        // Create 2 edges from each node (total 20 edges)
        for (int i = 0; i < 10; i++)
        {
            int nextNode1 = (i + 1) % 10;
            int nextNode2 = (i + 2) % 10;
            builder.AddEdge(nodeIndices[i], nodeIndices[nextNode1], i * 2 + 1);
            builder.AddEdge(nodeIndices[i], nodeIndices[nextNode2], i * 2 + 2);
        }

        return builder.Build();
    }

    [Benchmark]
    public (ComboNode[] nodes, ComboEdge[] edges) Build_LinearGraph_50Nodes_49Edges()
    {
        // Create a long linear combo chain
        var builder = new ComboGraphBuilder();

        var nodeIndices = new int[50];
        for (int i = 0; i < 50; i++)
        {
            nodeIndices[i] = builder.AddNode(i * 100);
        }

        // Create linear chain
        for (int i = 0; i < 49; i++)
        {
            builder.AddEdge(nodeIndices[i], nodeIndices[i + 1], i + 1);
        }

        return builder.Build();
    }
}
