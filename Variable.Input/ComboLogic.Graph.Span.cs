namespace Variable.Input;

/// <summary>
///     Span-based overloads for ECS compatibility.
///     This is the single source of truth for traversal logic.
/// </summary>
public static partial class ComboLogic
{
    /// <summary>
    ///     Attempts to advance the state using Span for zero-copy access.
    ///     Works with T[], NativeArray&lt;T&gt;, BlobArray&lt;T&gt;, and stackalloc.
    ///     Includes safety checks to prevent crashes from corrupt graph data.
    /// </summary>
    /// <param name="currentNodeIndex">Reference to the current node index.</param>
    /// <param name="isActionBusy">Reference to the action busy flag.</param>
    /// <param name="bufferHead">Reference to the buffer head index.</param>
    /// <param name="bufferCount">Reference to the buffer count.</param>
    /// <param name="bufferInputs">Span of buffer inputs.</param>
    /// <param name="nodes">ReadOnlySpan of combo nodes.</param>
    /// <param name="edges">ReadOnlySpan of combo edges.</param>
    /// <param name="newActionID">Output for the new action ID if transition occurs.</param>
    /// <returns>True if state advanced; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryAdvanceState(
        ref int currentNodeIndex,
        ref bool isActionBusy,
        ref int bufferHead,
        ref int bufferCount,
        Span<int> bufferInputs,
        ReadOnlySpan<ComboNode> nodes,
        ReadOnlySpan<ComboEdge> edges,
        out int newActionID)
    {
        newActionID = -1;

        // 0. Safety: Handle empty graph
        if (nodes.Length == 0) return false;

        // 1. Busy Check
        if (isActionBusy) return false;

        // 2. Input Check
        if (!PeekInput(bufferHead, bufferCount, bufferInputs, out var nextInput)) return false;

        // 3. Safety: Ensure current index is valid (recovery from bad state)
        if (currentNodeIndex < 0 || currentNodeIndex >= nodes.Length) currentNodeIndex = 0;

        // 4. Graph Traversal (CSR Look-up)
        var currentNode = nodes[currentNodeIndex];
        var targetNodeIndex = -1;
        var start = currentNode.EdgeStartIndex;
        var end = start + currentNode.EdgeCount;

        // Safety: Clamp edge loop to actual array bounds to prevent crashes on bad data
        if (end > edges.Length) end = edges.Length;

        for (var i = start; i < end; i++)
            if (edges[i].InputTrigger == nextInput)
            {
                targetNodeIndex = edges[i].TargetNodeIndex;
                break;
            }

        // 5. Transition Execution
        if (targetNodeIndex != -1)
            // Safety: Validate target node exists before jumping
            if (targetNodeIndex >= 0 && targetNodeIndex < nodes.Length)
            {
                TryDequeueInput(ref bufferHead, ref bufferCount, bufferInputs, out _);

                currentNodeIndex = targetNodeIndex;
                isActionBusy = true;
                newActionID = nodes[targetNodeIndex].ActionID;
                return true;
            }

        // If target index was invalid, treat as dead end (fall through to reset)
        // 6. Failure / Reset Logic (consumes invalid input)
        TryDequeueInput(ref bufferHead, ref bufferCount, bufferInputs, out _);
        currentNodeIndex = 0;
        newActionID = nodes[0].ActionID;
        return false;
    }

    /// <summary>
    ///     Signals that the current action has finished, allowing state transitions.
    /// </summary>
    /// <param name="isActionBusy">Reference to the action busy flag.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SignalActionFinished(ref bool isActionBusy)
    {
        isActionBusy = false;
    }
}