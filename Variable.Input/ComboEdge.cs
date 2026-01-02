namespace Variable.Input;

/// <summary>
///     Represents an edge in the combo graph.
///     Defines input trigger and target node.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ComboEdge
{
    /// <summary>
    ///     The input ID that triggers the transition.
    /// </summary>
    public int InputTrigger;

    /// <summary>
    ///     The index of the target node in the graph's node array.
    /// </summary>
    public int TargetNodeIndex;
}