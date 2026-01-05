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
    private int _nodeCount;
    private int _edgeCount;

    /// <summary>
    ///     Initializes a new instance from externally allocated memory.
    /// </summary>
    /// <param name="nodes">Pointer to nodes array.</param>
    /// <param name="nodeCount">Number of nodes.</param>
    /// <param name="edges">Pointer to edges array.</param>
    /// <param name="edgeCount">Number of edges.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ComboGraph(ComboNode* nodes, int nodeCount, ComboEdge* edges, int edgeCount)
    {
        _nodes = nodes;
        _nodeCount = nodeCount;
        _edges = edges;
        _edgeCount = edgeCount;
    }

    /// <summary>
    ///     Creates a ComboGraph from a Span (typically from managed arrays).
    /// </summary>
    /// <param name="nodes">Span of nodes.</param>
    /// <param name="edges">Span of edges.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ComboGraph(Span<ComboNode> nodes, Span<ComboEdge> edges)
    {
        fixed (ComboNode* nodePtr = nodes)
        fixed (ComboEdge* edgePtr = edges)
        {
            _nodes = nodePtr;
            _nodeCount = nodes.Length;
            _edges = edgePtr;
            _edgeCount = edges.Length;
        }
    }

    /// <summary>
    ///     Number of nodes.
    /// </summary>
    public int NodeCount
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _nodeCount;
    }

    /// <summary>
    ///     Number of edges.
    /// </summary>
    public int EdgeCount
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _edgeCount;
    }

    /// <summary>
    ///     Gets the nodes as a ReadOnlySpan (zero allocation).
    /// </summary>
    public ReadOnlySpan<ComboNode> NodesSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _nodes == null || _nodeCount == 0 
            ? ReadOnlySpan<ComboNode>.Empty 
            : new ReadOnlySpan<ComboNode>(_nodes, _nodeCount);
    }

    /// <summary>
    ///     Gets the edges as a ReadOnlySpan (zero allocation).
    /// </summary>
    public ReadOnlySpan<ComboEdge> EdgesSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _edges == null || _edgeCount == 0 
            ? ReadOnlySpan<ComboEdge>.Empty 
            : new ReadOnlySpan<ComboEdge>(_edges, _edgeCount);
    }

    /// <summary>
    ///     Gets a reference to a node by index.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref ComboNode GetNodeRef(int index)
    {
        if (index < 0 || index >= _nodeCount || _nodes == null)
            throw new IndexOutOfRangeException($"Node index {index} out of range [0, {_nodeCount})");
        return ref _nodes[index];
    }

    /// <summary>
    ///     Gets a reference to an edge by index.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref ComboEdge GetEdgeRef(int index)
    {
        if (index < 0 || index >= _edgeCount || _edges == null)
            throw new IndexOutOfRangeException($"Edge index {index} out of range [0, {_edgeCount})");
        return ref _edges[index];
    }
}