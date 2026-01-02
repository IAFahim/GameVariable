using Xunit;

namespace Variable.Input.Tests;

/// <summary>
///     Tests for safety and robustness against corrupt graph data.
/// </summary>
public class ComboSafetyTests
{
    [Fact]
    public void Safety_InvalidTargetNode_ResetsGracefully()
    {
        // Create graph with edge pointing to non-existent node
        var nodes = new ComboNode[2];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 1 };
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 1, EdgeCount = 0 };

        var edges = new ComboEdge[1];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 999 }; // Invalid!

        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        // Should NOT crash, should reset to neutral
        var result = ComboLogic.TryAdvanceState(ref state, ref buffer, nodes, edges, out var actionID);

        Assert.False(result);
        Assert.Equal(0, actionID);
        Assert.Equal(0, state.CurrentNodeIndex);
    }

    [Fact]
    public void Safety_NegativeTargetNode_ResetsGracefully()
    {
        var nodes = new ComboNode[2];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 1 };
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 1, EdgeCount = 0 };

        var edges = new ComboEdge[1];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = -5 }; // Negative!

        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        var result = ComboLogic.TryAdvanceState(ref state, ref buffer, nodes, edges, out var actionID);

        Assert.False(result);
        Assert.Equal(0, state.CurrentNodeIndex);
    }

    [Fact]
    public void Safety_EdgeCountExceedsArrayBounds_ClampsGracefully()
    {
        var nodes = new ComboNode[2];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 999 }; // Way too many!
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 1, EdgeCount = 0 };

        var edges = new ComboEdge[2];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 1 };
        edges[1] = new ComboEdge { InputTrigger = 2, TargetNodeIndex = 1 };

        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        // Should NOT crash, should scan only available edges
        var result = ComboLogic.TryAdvanceState(ref state, ref buffer, nodes, edges, out var actionID);

        Assert.True(result);
        Assert.Equal(100, actionID);
        Assert.Equal(1, state.CurrentNodeIndex);
    }

    [Fact]
    public void Safety_CurrentNodeIndexCorrupt_AutoRecovers()
    {
        var nodes = new ComboNode[2];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 1 };
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 1, EdgeCount = 0 };

        var edges = new ComboEdge[1];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 1 };

        // State starts corrupted
        var state = new ComboState { CurrentNodeIndex = 777, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        // Should auto-recover to node 0
        var result = ComboLogic.TryAdvanceState(ref state, ref buffer, nodes, edges, out var actionID);

        // After recovery, it's at node 0, input 1 takes it to node 1
        Assert.True(result);
        Assert.Equal(100, actionID);
        Assert.Equal(1, state.CurrentNodeIndex);
    }

    [Fact]
    public void Safety_EmptyGraph_DoesNotCrash()
    {
        var nodes = Array.Empty<ComboNode>();
        var edges = Array.Empty<ComboEdge>();

        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        // Should handle gracefully - returns false immediately for empty graph
        var result = ComboLogic.TryAdvanceState(ref state, ref buffer, nodes, edges, out var actionID);

        Assert.False(result);
        Assert.Equal(-1, actionID); // No action executed
        Assert.Equal(1, buffer.Count); // Input not consumed
    }

    [Fact]
    public void Safety_SpanAPI_SameProtections()
    {
        // Ensure Span API has same safety (it's the source of truth)
        var nodes = new ComboNode[2];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 1 };
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 1, EdgeCount = 0 };

        var edges = new ComboEdge[1];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 999 }; // Bad target

        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        var result = ComboLogic.TryAdvanceState(
            ref state,
            ref buffer,
            nodes.AsSpan(),
            edges.AsSpan(),
            out var actionID);

        Assert.False(result);
        Assert.Equal(0, state.CurrentNodeIndex);
    }

    [Fact]
    public void Safety_EdgeStartIndexBeyondArrayBounds_Clamps()
    {
        var nodes = new ComboNode[1];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 100, EdgeCount = 5 }; // Start way beyond

        var edges = Array.Empty<ComboEdge>();

        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        // Should not crash
        var result = ComboLogic.TryAdvanceState(ref state, ref buffer, nodes, edges, out var actionID);

        Assert.False(result); // No valid edges, resets
        Assert.Equal(0, state.CurrentNodeIndex);
    }
}