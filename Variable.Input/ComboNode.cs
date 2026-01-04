namespace Variable.Input;

/// <summary>
///     Represents a node in the combo graph.
///     Each node has an action ID and edges to other nodes.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public partial struct ComboNode
{
    /// <summary>
    ///     The ID of the action associated with this node.
    /// </summary>
    public int ActionID;

    /// <summary>
    ///     The starting index in the global edge array for this node's outgoing edges.
    /// </summary>
    public int EdgeStartIndex;

    /// <summary>
    ///     The number of outgoing edges for this node.
    /// </summary>
    public int EdgeCount;
}