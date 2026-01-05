namespace Variable.Input;

/// <summary>
///     Extension methods for ComboState. Sugar layer over logic.
/// </summary>
public static class ComboStateExtensions
{
    /// <summary>
    ///     Attempts to update the combo state based on buffered inputs.
    /// </summary>
    /// <param name="state">The combo state to update.</param>
    /// <param name="buffer">The input buffer to read from.</param>
    /// <param name="graph">The combo graph to traverse.</param>
    /// <param name="actionId">The resulting action ID if a transition occurs.</param>
    /// <returns>True if the state was updated; otherwise, false.</returns>
    public static bool TryUpdate(
        ref this ComboState state,
        ref InputRingBuffer buffer,
        ComboGraph graph,
        out int actionId)
    {
        var inputs = MemoryMarshal.CreateSpan(ref buffer.Input0, 8);
        return ComboLogic.TryAdvanceState(
            ref state.CurrentNodeIndex,
            ref state.IsActionBusy,
            ref buffer.Head,
            ref buffer.Count,
            inputs,
            graph.NodesSpan,
            graph.EdgesSpan,
            out actionId);
    }


    /// <summary>
    ///     Attempts to update the combo state based on buffered inputs (Span overload).
    /// </summary>
    /// <param name="state">The combo state to update.</param>
    /// <param name="buffer">The input buffer to read from.</param>
    /// <param name="nodes">The span of combo nodes.</param>
    /// <param name="edges">The span of combo edges.</param>
    /// <param name="actionId">The resulting action ID if a transition occurs.</param>
    /// <returns>True if the state was updated; otherwise, false.</returns>
    public static bool TryUpdate(
        ref this ComboState state,
        ref InputRingBuffer buffer,
        ReadOnlySpan<ComboNode> nodes,
        ReadOnlySpan<ComboEdge> edges,
        out int actionId)
    {
        var inputs = MemoryMarshal.CreateSpan(ref buffer.Input0, 8);
        return ComboLogic.TryAdvanceState(
            ref state.CurrentNodeIndex,
            ref state.IsActionBusy,
            ref buffer.Head,
            ref buffer.Count,
            inputs,
            nodes,
            edges,
            out actionId);
    }

    /// <summary>
    ///     Signals that the current action has finished.
    /// </summary>
    /// <param name="state">The combo state to signal.</param>
    public static void SignalActionFinished(ref this ComboState state)
    {
        ComboLogic.SignalActionFinished(ref state.IsActionBusy);
    }

    /// <summary>
    ///     Resets the state to the initial node.
    /// </summary>
    /// <param name="state">The combo state to reset.</param>
    public static void Reset(ref this ComboState state)
    {
        state.CurrentNodeIndex = 0;
        state.IsActionBusy = false;
    }
}