namespace Variable.Input;

/// <summary>
///     The complete combo graph containing nodes and edges.
///     Uses CSR (Compressed Sparse Row) format for cache-friendly traversal.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ComboGraph
{
    public ComboNode[] Nodes;
    public ComboEdge[] Edges;
}
