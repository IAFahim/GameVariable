namespace Variable.Input.Tests;

public class TestGraph : IDisposable
{
    private readonly ComboEdge[] _edges;
    private readonly ComboNode[] _nodes;

    public TestGraph(ComboNode[] nodes, ComboEdge[] edges)
    {
        _nodes = nodes;
        _edges = edges;
    }

    public ComboGraph Graph => new(_nodes.AsSpan(), _edges.AsSpan());

    public void Dispose()
    {
        // No unmanaged memory to free!
    }
}