namespace Variable.Input;

/// <summary>
///     Extension methods for ComboState. Sugar layer over logic.
/// </summary>
public static class ComboStateExtensions
{
    /// <summary>
    ///     Attempts to update the combo state based on buffered inputs.
    /// </summary>
    public static bool TryUpdate(
        ref this ComboState state,
        ref InputRingBuffer buffer,
        ComboGraph graph,
        out int actionID)
    {
        return ComboLogic.TryAdvanceState(ref state, ref buffer, graph.Nodes, graph.Edges, out actionID);
    }

    /// <summary>
    ///     Signals that the current action has finished.
    /// </summary>
    public static void SignalActionFinished(ref this ComboState state)
    {
        ComboLogic.SignalActionFinished(ref state);
    }

    /// <summary>
    ///     Resets the state to the initial node.
    /// </summary>
    public static void Reset(ref this ComboState state)
    {
        state.CurrentNodeIndex = 0;
        state.IsActionBusy = false;
    }
}