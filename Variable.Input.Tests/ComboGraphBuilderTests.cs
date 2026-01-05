using Xunit;

namespace Variable.Input.Tests;

public class ComboGraphBuilderTests
{
    [Fact]
    public void Builder_CreatesValidGraphArrays()
    {
        var builder = new ComboGraphBuilder();

        // Create simple graph: 0 -> 1 (Input 1)
        var node0 = builder.AddNode(0);
        var node1 = builder.AddNode(100);

        builder.AddEdge(node0, node1, 1);

        var (nodes, edges) = builder.Build();

        // Verify Nodes
        Assert.Equal(2, nodes.Length);

        // Node 0
        Assert.Equal(0, nodes[0].ActionID);
        Assert.Equal(0, nodes[0].EdgeStartIndex);
        Assert.Equal(1, nodes[0].EdgeCount);

        // Node 1
        Assert.Equal(100, nodes[1].ActionID);
        Assert.Equal(1, nodes[1].EdgeStartIndex); // Starts after Node 0's edges
        Assert.Equal(0, nodes[1].EdgeCount);

        // Verify Edges
        Assert.Single(edges);
        Assert.Equal(1, edges[0].InputTrigger);
        Assert.Equal(1, edges[0].TargetNodeIndex);
    }

    [Fact]
    public void Builder_HandlesMultipleEdgesCorrectly()
    {
        var builder = new ComboGraphBuilder();

        // 0 -> 1 (Input 1)
        // 0 -> 2 (Input 2)
        var n0 = builder.AddNode(0);
        var n1 = builder.AddNode(100);
        var n2 = builder.AddNode(200);

        builder.AddEdge(n0, n1, 1);
        builder.AddEdge(n0, n2, 2);

        var (nodes, edges) = builder.Build();

        Assert.Equal(3, nodes.Length);
        Assert.Equal(2, edges.Length);

        // Node 0 has 2 edges
        Assert.Equal(0, nodes[0].EdgeStartIndex);
        Assert.Equal(2, nodes[0].EdgeCount);

        // Edges are contiguous
        Assert.Equal(1, edges[0].InputTrigger);
        Assert.Equal(1, edges[0].TargetNodeIndex);

        Assert.Equal(2, edges[1].InputTrigger);
        Assert.Equal(2, edges[1].TargetNodeIndex);
    }

    [Fact]
    public void Builder_HandlesDisjointedCreationOrder()
    {
        var builder = new ComboGraphBuilder();

        var n0 = builder.AddNode(0);
        var n1 = builder.AddNode(100);

        // Add edge to n0
        builder.AddEdge(n0, n1, 1);

        // Add another node n2
        var n2 = builder.AddNode(200);

        // Add another edge to n0 (should still be grouped with n0's edges in final array)
        builder.AddEdge(n0, n2, 2);

        var (nodes, edges) = builder.Build();

        // Node 0 should have 2 edges, contiguous in the final array
        Assert.Equal(2, nodes[0].EdgeCount);
        Assert.Equal(0, nodes[0].EdgeStartIndex);

        // Check edges
        Assert.Equal(1, edges[0].InputTrigger);
        Assert.Equal(1, edges[0].TargetNodeIndex);

        Assert.Equal(2, edges[1].InputTrigger);
        Assert.Equal(2, edges[1].TargetNodeIndex);
    }
}