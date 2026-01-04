using Xunit;

namespace Variable.Input.Tests;

public class ComboLogicTests
{
    private static TestGraph CreateSimpleGraph()
    {
        // Node 0: Idle (ActionID=0)
        //   -> L goes to Node 1
        //   -> R goes to Node 2
        // Node 1: Light Attack (ActionID=100)
        //   -> L goes to Node 3
        // Node 2: Heavy Attack (ActionID=200)
        //   -> R goes to Node 4
        // Node 3: Light Combo (ActionID=101)
        //   (no edges - dead end)
        // Node 4: Heavy Combo (ActionID=201)
        //   (no edges - dead end)

        var nodes = new ComboNode[5];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 2 };
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 2, EdgeCount = 1 };
        nodes[2] = new ComboNode { ActionID = 200, EdgeStartIndex = 3, EdgeCount = 1 };
        nodes[3] = new ComboNode { ActionID = 101, EdgeStartIndex = 4, EdgeCount = 0 };
        nodes[4] = new ComboNode { ActionID = 201, EdgeStartIndex = 4, EdgeCount = 0 };

        var edges = new ComboEdge[4];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 1 };
        edges[1] = new ComboEdge { InputTrigger = 2, TargetNodeIndex = 2 };
        edges[2] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 3 };
        edges[3] = new ComboEdge { InputTrigger = 2, TargetNodeIndex = 4 };

        return new TestGraph(nodes, edges);
    }

    [Fact]
    public void TryAdvanceState_NoInput_ReturnsFalse()
    {
        using var graphWrapper = CreateSimpleGraph();
        var graph = graphWrapper.Graph;
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        var result = state.TryUpdate(ref buffer, graph, out var actionID);

        Assert.False(result);
        Assert.Equal(-1, actionID);
    }

    [Fact]
    public void TryAdvanceState_ActionBusy_ReturnsFalse()
    {
        using var graphWrapper = CreateSimpleGraph();
        var graph = graphWrapper.Graph;
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = true };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        var result = state.TryUpdate(ref buffer, graph, out var actionID);

        Assert.False(result);
        Assert.Equal(-1, actionID);
        Assert.Equal(1, buffer.Count);
    }

    [Fact]
    public void TryAdvanceState_ValidTransition_Works()
    {
        using var graphWrapper = CreateSimpleGraph();
        var graph = graphWrapper.Graph;
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        var result = state.TryUpdate(ref buffer, graph, out var actionID);

        Assert.True(result);
        Assert.Equal(100, actionID);
        Assert.Equal(1, state.CurrentNodeIndex);
        Assert.True(state.IsActionBusy);
        Assert.Equal(0, buffer.Count);
    }

    [Fact]
    public void TryAdvanceState_InvalidInput_ResetsToIdle()
    {
        using var graphWrapper = CreateSimpleGraph();
        var graph = graphWrapper.Graph;
        var state = new ComboState { CurrentNodeIndex = 1, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(2);

        var result = state.TryUpdate(ref buffer, graph, out var actionID);

        Assert.False(result);
        Assert.Equal(0, actionID);
        Assert.Equal(0, state.CurrentNodeIndex);
        Assert.Equal(0, buffer.Count);
    }

    [Fact]
    public void TryAdvanceState_ComboChain_Works()
    {
        using var graphWrapper = CreateSimpleGraph();
        var graph = graphWrapper.Graph;
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);
        buffer.RegisterInput(1);

        var result1 = state.TryUpdate(ref buffer, graph, out var actionID1);
        Assert.True(result1);
        Assert.Equal(100, actionID1);
        Assert.Equal(1, state.CurrentNodeIndex);

        state.SignalActionFinished();

        var result2 = state.TryUpdate(ref buffer, graph, out var actionID2);
        Assert.True(result2);
        Assert.Equal(101, actionID2);
        Assert.Equal(3, state.CurrentNodeIndex);
        Assert.Equal(0, buffer.Count);
    }

    [Fact]
    public void TryAdvanceState_MultipleInputs_Buffered()
    {
        using var graphWrapper = CreateSimpleGraph();
        var graph = graphWrapper.Graph;
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        buffer.RegisterInput(2);
        buffer.RegisterInput(2);

        var result1 = state.TryUpdate(ref buffer, graph, out var actionID1);
        Assert.True(result1);
        Assert.Equal(200, actionID1);
        Assert.Equal(2, state.CurrentNodeIndex);
        Assert.Equal(1, buffer.Count);

        state.SignalActionFinished();

        var result2 = state.TryUpdate(ref buffer, graph, out var actionID2);
        Assert.True(result2);
        Assert.Equal(201, actionID2);
        Assert.Equal(4, state.CurrentNodeIndex);
        Assert.Equal(0, buffer.Count);
    }

    [Fact]
    public void SignalActionFinished_ClearsFlag()
    {
        var state = new ComboState { CurrentNodeIndex = 1, IsActionBusy = true };

        state.SignalActionFinished();

        Assert.False(state.IsActionBusy);
    }

    [Fact]
    public void Extension_TryUpdate_Works()
    {
        using var graphWrapper = CreateSimpleGraph();
        var graph = graphWrapper.Graph;
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        var result = state.TryUpdate(ref buffer, graph, out var actionID);

        Assert.True(result);
        Assert.Equal(100, actionID);
        Assert.Equal(1, state.CurrentNodeIndex);
    }

    [Fact]
    public void Extension_Reset_Works()
    {
        var state = new ComboState { CurrentNodeIndex = 3, IsActionBusy = true };

        state.Reset();

        Assert.Equal(0, state.CurrentNodeIndex);
        Assert.False(state.IsActionBusy);
    }
}