namespace Variable.Input;

public static partial class ComboLogic
{
    /// <summary>
    ///     Attempts to advance the state based on the next input in the buffer.
    ///     All parameters are primitives or primitive arrays.
    /// </summary>
    /// <param name="state">Current combo state</param>
    /// <param name="buffer">Input ring buffer</param>
    /// <param name="nodes">Array of combo nodes</param>
    /// <param name="edges">Array of combo edges</param>
    /// <param name="newActionID">Output action ID if transition succeeds</param>
    /// <returns>True if valid transition occurred</returns>
    public static bool TryAdvanceState(
        ref ComboState state,
        ref InputRingBuffer buffer,
        ComboNode[] nodes,
        ComboEdge[] edges,
        out int newActionID)
    {
        newActionID = -1;

        if (state.IsActionBusy) return false;

        if (!PeekInput(ref buffer, out var nextInput)) return false;

        if (state.CurrentNodeIndex < 0 || state.CurrentNodeIndex >= nodes.Length)
        {
            state.CurrentNodeIndex = 0;
        }

        var currentNode = nodes[state.CurrentNodeIndex];

        var targetNodeIndex = -1;
        var start = currentNode.EdgeStartIndex;
        var end = start + currentNode.EdgeCount;

        for (var i = start; i < end; i++)
        {
            if (edges[i].InputTrigger == nextInput)
            {
                targetNodeIndex = edges[i].TargetNodeIndex;
                break;
            }
        }

        if (targetNodeIndex != -1)
        {
            TryDequeueInput(ref buffer, out _);

            state.CurrentNodeIndex = targetNodeIndex;
            state.IsActionBusy = true;
            newActionID = nodes[targetNodeIndex].ActionID;
            return true;
        }

        TryDequeueInput(ref buffer, out _);
        state.CurrentNodeIndex = 0;
        newActionID = nodes[0].ActionID;
        return false;
    }

    /// <summary>
    ///     Signals that the current action has finished, allowing state transitions.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SignalActionFinished(ref ComboState state)
    {
        state.IsActionBusy = false;
    }
}
