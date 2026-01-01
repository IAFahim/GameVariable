namespace Variable.Input;

public static partial class ComboLogic
{
    /// <summary>
    ///     Attempts to advance the state based on the next input in the buffer.
    ///     Array overload - wraps the Span implementation for zero duplication.
    /// </summary>
    /// <param name="state">Current combo state</param>
    /// <param name="buffer">Input ring buffer</param>
    /// <param name="nodes">Array of combo nodes</param>
    /// <param name="edges">Array of combo edges</param>
    /// <param name="newActionID">Output action ID if transition succeeds</param>
    /// <returns>True if valid transition occurred</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryAdvanceState(
        ref ComboState state,
        ref InputRingBuffer buffer,
        ComboNode[] nodes,
        ComboEdge[] edges,
        out int newActionID)
    {
        return TryAdvanceState(
            ref state,
            ref buffer,
            new ReadOnlySpan<ComboNode>(nodes),
            new ReadOnlySpan<ComboEdge>(edges),
            out newActionID);
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