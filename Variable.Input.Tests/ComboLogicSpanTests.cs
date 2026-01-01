namespace Variable.Input.Tests;

/// <summary>
///     Tests for Span-based API (ECS compatibility).
/// </summary>
public class ComboLogicSpanTests
{
    private static (ComboNode[] nodes, ComboEdge[] edges) CreateSimpleGraph()
    {
        var nodes = new ComboNode[3];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 2 };
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 2, EdgeCount = 0 };
        nodes[2] = new ComboNode { ActionID = 200, EdgeStartIndex = 2, EdgeCount = 0 };

        var edges = new ComboEdge[2];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 1 };
        edges[1] = new ComboEdge { InputTrigger = 2, TargetNodeIndex = 2 };

        return (nodes, edges);
    }

    [Fact]
    public void SpanAPI_WithManagedArray_Works()
    {
        var (nodes, edges) = CreateSimpleGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        // Use Span overload
        var result = ComboLogic.TryAdvanceState(
            ref state,
            ref buffer,
            nodes.AsSpan(),
            edges.AsSpan(),
            out var actionID);

        Assert.True(result);
        Assert.Equal(100, actionID);
        Assert.Equal(1, state.CurrentNodeIndex);
    }

    [Fact]
    public void SpanAPI_WithStackalloc_Works()
    {
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        // Create graph on stack
        Span<ComboNode> nodes = stackalloc ComboNode[3];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 2 };
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 2, EdgeCount = 0 };
        nodes[2] = new ComboNode { ActionID = 200, EdgeStartIndex = 2, EdgeCount = 0 };

        Span<ComboEdge> edges = stackalloc ComboEdge[2];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 1 };
        edges[1] = new ComboEdge { InputTrigger = 2, TargetNodeIndex = 2 };

        var result = ComboLogic.TryAdvanceState(
            ref state,
            ref buffer,
            nodes,
            edges,
            out var actionID);

        Assert.True(result);
        Assert.Equal(100, actionID);
    }

    [Fact]
    public void SpanAPI_WithSlice_Works()
    {
        var (allNodes, allEdges) = CreateSimpleGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(2);

        // Take slice of arrays (useful for partial graphs)
        var nodeSlice = allNodes.AsSpan().Slice(0, 3);
        var edgeSlice = allEdges.AsSpan().Slice(0, 2);

        var result = ComboLogic.TryAdvanceState(
            ref state,
            ref buffer,
            nodeSlice,
            edgeSlice,
            out var actionID);

        Assert.True(result);
        Assert.Equal(200, actionID);
    }

    [Fact]
    public void SpanAPI_ComboChain_Works()
    {
        var nodes = new ComboNode[4];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 1 };
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 1, EdgeCount = 1 };
        nodes[2] = new ComboNode { ActionID = 101, EdgeStartIndex = 2, EdgeCount = 1 };
        nodes[3] = new ComboNode { ActionID = 102, EdgeStartIndex = 3, EdgeCount = 0 };

        var edges = new ComboEdge[3];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 1 };
        edges[1] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 2 };
        edges[2] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 3 };

        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);
        buffer.RegisterInput(1);
        buffer.RegisterInput(1);

        // First hit
        Assert.True(ComboLogic.TryAdvanceState(ref state, ref buffer, nodes.AsSpan(), edges.AsSpan(), out var act1));
        Assert.Equal(100, act1);
        ComboLogic.SignalActionFinished(ref state);

        // Second hit
        Assert.True(ComboLogic.TryAdvanceState(ref state, ref buffer, nodes.AsSpan(), edges.AsSpan(), out var act2));
        Assert.Equal(101, act2);
        ComboLogic.SignalActionFinished(ref state);

        // Third hit
        Assert.True(ComboLogic.TryAdvanceState(ref state, ref buffer, nodes.AsSpan(), edges.AsSpan(), out var act3));
        Assert.Equal(102, act3);
    }
}