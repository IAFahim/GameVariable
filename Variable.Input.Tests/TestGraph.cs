using System.Runtime.InteropServices;

namespace Variable.Input.Tests;

public class TestGraph : IDisposable
{
    private readonly GCHandle _edgeHandle;
    private readonly ComboEdge[] _edges;
    private readonly GCHandle _nodeHandle;
    private readonly ComboNode[] _nodes;

    public unsafe TestGraph(ComboNode[] nodes, ComboEdge[] edges)
    {
        _nodes = nodes;
        _edges = edges;

        // Pin the arrays to prevent GC from moving them
        _nodeHandle = GCHandle.Alloc(_nodes, GCHandleType.Pinned);
        _edgeHandle = GCHandle.Alloc(_edges, GCHandleType.Pinned);

        // Create graph with pinned pointers
        Graph = new ComboGraph(
            (ComboNode*)_nodeHandle.AddrOfPinnedObject(),
            _nodes.Length,
            (ComboEdge*)_edgeHandle.AddrOfPinnedObject(),
            _edges.Length
        );
    }

    public ComboGraph Graph { get; }

    public void Dispose()
    {
        if (_nodeHandle.IsAllocated)
            _nodeHandle.Free();
        if (_edgeHandle.IsAllocated)
            _edgeHandle.Free();
    }
}