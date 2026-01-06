namespace Variable.Input;

/// <summary>
///     The complete combo graph containing nodes and edges.
///     Pure unmanaged struct using unsafe pointers for Burst compatibility.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public unsafe struct ComboGraph
{
    private ComboNode* _nodes;
    private ComboEdge* _edges;

    /// <summary>
    ///     Initializes a new instance from externally allocated memory.
    /// </summary>
    /// <param name="nodes">Pointer to nodes array.</param>
    /// <param name="nodeCount">Number of nodes.</param>
    /// <param name="edges">Pointer to edges array.</param>
    /// <param name="edgeCount">Number of edges.</param>
    public ComboGraph(ComboNode* nodes, int nodeCount, ComboEdge* edges, int edgeCount)
    {
        if (nodeCount < 0)
        {
            _nodes = null;
            NodeCount = 0;
            _edges = null;
            EdgeCount = 0;
            return;
        }

        if (edgeCount < 0)
        {
            _nodes = null;
            NodeCount = 0;
            _edges = null;
            EdgeCount = 0;
            return;
        }

        _nodes = nodes;
        NodeCount = nodeCount;
        _edges = edges;
        EdgeCount = edgeCount;
    }


    /// <summary>
    ///     Number of nodes.
    /// </summary>
    public int NodeCount
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    /// <summary>
    ///     Number of edges.
    /// </summary>
    public int EdgeCount
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    /// <summary>
    ///     Gets the nodes as a ReadOnlySpan (zero allocation).
    /// </summary>
    public ReadOnlySpan<ComboNode> NodesSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _nodes == null || NodeCount == 0
            ? ReadOnlySpan<ComboNode>.Empty
            : new ReadOnlySpan<ComboNode>(_nodes, NodeCount);
    }

    /// <summary>
    ///     Gets the edges as a ReadOnlySpan (zero allocation).
    /// </summary>
    public ReadOnlySpan<ComboEdge> EdgesSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _edges == null || EdgeCount == 0
            ? ReadOnlySpan<ComboEdge>.Empty
            : new ReadOnlySpan<ComboEdge>(_edges, EdgeCount);
    }

    /// <summary>
    ///     Gets a reference to a node by index.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref ComboNode GetNodeRef(int index)
    {
        if (index < 0 || index >= NodeCount || _nodes == null)
            throw new IndexOutOfRangeException($"Node index {index} out of range [0, {NodeCount})");
        return ref _nodes[index];
    }

    /// <summary>
    ///     Gets a reference to an edge by index.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref ComboEdge GetEdgeRef(int index)
    {
        if (index < 0 || index >= EdgeCount || _edges == null)
            throw new IndexOutOfRangeException($"Edge index {index} out of range [0, {EdgeCount})");
        return ref _edges[index];
    }
}