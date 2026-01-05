using System.Runtime.InteropServices;

namespace Variable.Input.Tests;

public unsafe class TestGraph : IDisposable
{
    private IntPtr _edgesPtr;
    private IntPtr _nodesPtr;
    public ComboGraph Graph;

    public TestGraph(ComboNode[] nodes, ComboEdge[] edges)
    {
        var nodeSize = sizeof(ComboNode);
        var edgeSize = sizeof(ComboEdge);

        _nodesPtr = Marshal.AllocHGlobal(nodeSize * nodes.Length);
        _edgesPtr = Marshal.AllocHGlobal(edgeSize * edges.Length);

        // Copy data
        for (var i = 0; i < nodes.Length; i++) ((ComboNode*)_nodesPtr)[i] = nodes[i];

        for (var i = 0; i < edges.Length; i++) ((ComboEdge*)_edgesPtr)[i] = edges[i];

        Graph = new ComboGraph
        {
            Nodes = (ComboNode*)_nodesPtr,
            NodeCount = nodes.Length,
            Edges = (ComboEdge*)_edgesPtr,
            EdgeCount = edges.Length
        };
    }

    public void Dispose()
    {
        if (_nodesPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_nodesPtr);
            _nodesPtr = IntPtr.Zero;
        }

        if (_edgesPtr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_edgesPtr);
            _edgesPtr = IntPtr.Zero;
        }
    }
}