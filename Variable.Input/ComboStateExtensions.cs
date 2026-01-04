using System.Runtime.InteropServices;

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
        out int actionId)
    {
        var inputs = MemoryMarshal.CreateSpan(ref buffer.Input0, 8);
        return ComboLogic.TryAdvanceState(
            ref state.CurrentNodeIndex,
            ref state.IsActionBusy,
            ref buffer.Head,
            ref buffer.Count,
            inputs,
            graph.Nodes,
            graph.Edges,
            out actionId);
    }

    /// <summary>
    ///     Attempts to update the combo state based on buffered inputs (Span overload).
    /// </summary>
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
    public static void SignalActionFinished(ref this ComboState state)
    {
        ComboLogic.SignalActionFinished(ref state.IsActionBusy);
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