namespace Variable.Input.Tests;

/// <summary>
///     Integration test demonstrating a realistic fighting game combo scenario.
/// </summary>
public class ComboIntegrationTests
{
    /// <summary>
    ///     Creates a fighting game style combo tree:
    ///     Neutral (0)
    ///     ├─ L → Light Attack (100)
    ///     │     ├─ L → Light-Light Combo (101)
    ///     │     └─ R → Light-Heavy Combo (102)
    ///     └─ R → Heavy Attack (200)
    ///     └─ R → Heavy-Heavy Finisher (201)
    /// </summary>
    private static ComboGraph CreateFightingGameGraph()
    {
        var nodes = new ComboNode[6];
        nodes[0] = new ComboNode { ActionID = 0, EdgeStartIndex = 0, EdgeCount = 2 }; // Neutral
        nodes[1] = new ComboNode { ActionID = 100, EdgeStartIndex = 2, EdgeCount = 2 }; // Light
        nodes[2] = new ComboNode { ActionID = 200, EdgeStartIndex = 4, EdgeCount = 1 }; // Heavy
        nodes[3] = new ComboNode { ActionID = 101, EdgeStartIndex = 5, EdgeCount = 0 }; // LL
        nodes[4] = new ComboNode { ActionID = 102, EdgeStartIndex = 5, EdgeCount = 0 }; // LR
        nodes[5] = new ComboNode { ActionID = 201, EdgeStartIndex = 5, EdgeCount = 0 }; // RR

        var edges = new ComboEdge[5];
        edges[0] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 1 }; // Neutral → L
        edges[1] = new ComboEdge { InputTrigger = 2, TargetNodeIndex = 2 }; // Neutral → R
        edges[2] = new ComboEdge { InputTrigger = 1, TargetNodeIndex = 3 }; // Light → LL
        edges[3] = new ComboEdge { InputTrigger = 2, TargetNodeIndex = 4 }; // Light → LR
        edges[4] = new ComboEdge { InputTrigger = 2, TargetNodeIndex = 5 }; // Heavy → RR

        return new ComboGraph { Nodes = nodes, Edges = edges };
    }

    [Fact]
    public void FightingGame_SimpleLightAttack()
    {
        var graph = CreateFightingGameGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        // Player presses L
        buffer.RegisterInput(1);

        // Frame update
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID));
        Assert.Equal(100, actionID); // Light Attack
        Assert.Equal(1, state.CurrentNodeIndex);
    }

    [Fact]
    public void FightingGame_LightLightCombo()
    {
        var graph = CreateFightingGameGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        // Player rapidly presses L, L
        buffer.RegisterInput(1);
        buffer.RegisterInput(1);

        // First hit
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID1));
        Assert.Equal(100, actionID1);

        // Animation finishes
        state.SignalActionFinished();

        // Second hit (buffered input processed)
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID2));
        Assert.Equal(101, actionID2); // LL Combo
    }

    [Fact]
    public void FightingGame_LightHeavyCombo()
    {
        var graph = CreateFightingGameGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        // Player presses L, then R for a mix-up
        buffer.RegisterInput(1);
        buffer.RegisterInput(2);

        // Light attack
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID1));
        Assert.Equal(100, actionID1);

        state.SignalActionFinished();

        // Heavy finisher
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID2));
        Assert.Equal(102, actionID2); // LR Combo
    }

    [Fact]
    public void FightingGame_HeavyHeavyFinisher()
    {
        var graph = CreateFightingGameGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        // Player charges with R, R
        buffer.RegisterInput(2);
        buffer.RegisterInput(2);

        // Heavy attack
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID1));
        Assert.Equal(200, actionID1);

        state.SignalActionFinished();

        // Heavy finisher
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID2));
        Assert.Equal(201, actionID2); // RR Finisher
    }

    [Fact]
    public void FightingGame_InvalidInputResetsCombo()
    {
        var graph = CreateFightingGameGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        // Start light combo
        buffer.RegisterInput(1);
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID1));
        Assert.Equal(100, actionID1);

        state.SignalActionFinished();

        // Player mistimes and presses nothing, then presses L again
        // Heavy-Heavy edge only accepts R from Heavy node
        buffer.RegisterInput(1);
        buffer.RegisterInput(1);

        // LL combo continues
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID2));
        Assert.Equal(101, actionID2);

        state.SignalActionFinished();

        // At dead end (node 3), any input resets
        buffer.RegisterInput(1);
        Assert.False(state.TryUpdate(ref buffer, graph, out var actionID3));
        Assert.Equal(0, actionID3); // Reset to Neutral
        Assert.Equal(0, state.CurrentNodeIndex);
    }

    [Fact]
    public void FightingGame_BufferingMultipleInputs()
    {
        var graph = CreateFightingGameGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        // Player "dial-up" inputs rapidly (frame-perfect execution)
        buffer.RegisterInput(1);
        buffer.RegisterInput(2);
        buffer.RegisterInput(2);
        buffer.RegisterInput(2);

        // First input consumed
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID1));
        Assert.Equal(100, actionID1);
        Assert.Equal(3, buffer.Count); // 3 remaining

        state.SignalActionFinished();

        // Second input consumed (LR combo)
        Assert.True(state.TryUpdate(ref buffer, graph, out var actionID2));
        Assert.Equal(102, actionID2);
        Assert.Equal(2, buffer.Count); // 2 remaining

        state.SignalActionFinished();

        // Dead end - next inputs reset to neutral
        Assert.False(state.TryUpdate(ref buffer, graph, out _));
        Assert.Equal(0, state.CurrentNodeIndex);
        Assert.Equal(1, buffer.Count); // 1 remaining (consumed bad input)
    }

    [Fact]
    public void FightingGame_ActionBusy_BlocksTransition()
    {
        var graph = CreateFightingGameGraph();
        var state = new ComboState { CurrentNodeIndex = 0, IsActionBusy = false };
        var buffer = new InputRingBuffer();

        // Start combo
        buffer.RegisterInput(1);
        Assert.True(state.TryUpdate(ref buffer, graph, out _));
        Assert.True(state.IsActionBusy);

        // Player presses button again before animation finishes
        buffer.RegisterInput(1);

        // Should NOT process because busy
        Assert.False(state.TryUpdate(ref buffer, graph, out var actionID));
        Assert.Equal(-1, actionID);
        Assert.Equal(1, buffer.Count); // Input still buffered
    }
}