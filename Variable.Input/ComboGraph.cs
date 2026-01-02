namespace Variable.Input;

/// <summary>
///     The complete combo graph containing nodes and edges.
///     Uses CSR (Compressed Sparse Row) format for cache-friendly traversal.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ComboGraph
{
    /// <summary>
    ///     The array of all nodes in the graph.
    /// </summary>
    public ComboNode[] Nodes;

    /// <summary>
    ///     The flattened array of all edges in the graph.
    /// </summary>
    public ComboEdge[] Edges;
}