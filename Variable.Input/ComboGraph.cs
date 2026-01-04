namespace Variable.Input;

/// <summary>
///     The complete combo graph containing nodes and edges.
///     Unmanaged, Burst-compatible structure.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct ComboGraph
{
    /// <summary>
    ///     Pointer to the nodes array.
    /// </summary>
    public ComboNode* Nodes;

    /// <summary>
    ///     Number of nodes.
    /// </summary>
    public int NodeCount;

    /// <summary>
    ///     Pointer to the edges array.
    /// </summary>
    public ComboEdge* Edges;

    /// <summary>
    ///     Number of edges.
    /// </summary>
    public int EdgeCount;

    /// <summary>
    ///     Gets the nodes as a ReadOnlySpan.
    /// </summary>
    public ReadOnlySpan<ComboNode> NodesSpan => new(Nodes, NodeCount);

    /// <summary>
    ///     Gets the edges as a ReadOnlySpan.
    /// </summary>
    public ReadOnlySpan<ComboEdge> EdgesSpan => new(Edges, EdgeCount);
}