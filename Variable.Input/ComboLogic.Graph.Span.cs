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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryAdvanceState(
        ref ComboState state,
        ref InputRingBuffer buffer,
        ReadOnlySpan<ComboNode> nodes,
        ReadOnlySpan<ComboEdge> edges,
        out int newActionID)
    {
        newActionID = -1;

        // 0. Safety: Handle empty graph
        if (nodes.Length == 0) return false;

        // 1. Busy Check
        if (state.IsActionBusy) return false;

        // 2. Input Check
        if (!PeekInput(ref buffer, out var nextInput)) return false;

        // 3. Safety: Ensure current index is valid (recovery from bad state)
        if (state.CurrentNodeIndex < 0 || state.CurrentNodeIndex >= nodes.Length)
        {
            state.CurrentNodeIndex = 0;
        }

        // 4. Graph Traversal (CSR Look-up)
        var currentNode = nodes[state.CurrentNodeIndex];
        var targetNodeIndex = -1;
        var start = currentNode.EdgeStartIndex;
        var end = start + currentNode.EdgeCount;

        // Safety: Clamp edge loop to actual array bounds to prevent crashes on bad data
        if (end > edges.Length) end = edges.Length;

        for (var i = start; i < end; i++)
        {
            if (edges[i].InputTrigger == nextInput)
            {
                targetNodeIndex = edges[i].TargetNodeIndex;
                break;
            }
        }

        // 5. Transition Execution
        if (targetNodeIndex != -1)
        {
            // Safety: Validate target node exists before jumping
            if (targetNodeIndex >= 0 && targetNodeIndex < nodes.Length)
            {
                TryDequeueInput(ref buffer, out _);

                state.CurrentNodeIndex = targetNodeIndex;
                state.IsActionBusy = true;
                newActionID = nodes[targetNodeIndex].ActionID;
                return true;
            }
            // If target index was invalid, treat as dead end (fall through to reset)
        }

        // 6. Failure / Reset Logic (consumes invalid input)
        TryDequeueInput(ref buffer, out _);
        state.CurrentNodeIndex = 0;
        newActionID = nodes[0].ActionID;
        return false;
    }
}
