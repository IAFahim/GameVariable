namespace Variable.Input;

/// <summary>
///     Represents a node in the combo graph.
///     Each node has an action ID and edges to other nodes.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ComboNode
{
    public int ActionID;
    public int EdgeStartIndex;
    public int EdgeCount;
}
