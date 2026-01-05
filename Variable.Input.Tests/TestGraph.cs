using System.Runtime.InteropServices;
using Variable.Input;

namespace Variable.Input.Tests;

public class TestGraph : IDisposable
{
    private ComboNode[] _nodes;
    private ComboEdge[] _edges;
    
    public ComboGraph Graph => new ComboGraph(_nodes.AsSpan(), _edges.AsSpan());

    public TestGraph(ComboNode[] nodes, ComboEdge[] edges)
    {
        _nodes = nodes;
        _edges = edges;
    }

    public void Dispose()
    {
        // No unmanaged memory to free!
    }
}