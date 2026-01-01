namespace Variable.Input;

/// <summary>
///     Represents an edge in the combo graph.
///     Defines input trigger and target node.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ComboEdge
{
    public int InputTrigger;
    public int TargetNodeIndex;
}
