using Variable.Grid;

namespace Variable.Grid.Tests;

public class GridTests
{
    [Fact]
    public void Constructor_InitializesCorrectly()
    {
        var grid = new Grid2D<int>(10, 5);
        Assert.Equal(10, grid.Width);
        Assert.Equal(5, grid.Height);
        Assert.Equal(50, grid.Cells.Length);
    }

    [Fact]
    public void SetAndGet_Works()
    {
        var grid = new Grid2D<int>(3, 3);
        grid.Set(1, 1, 42);

        Assert.Equal(42, grid.Get(1, 1));
        Assert.Equal(42, grid.Cells[4]); // 1*3 + 1 = 4
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(2, 0, 2)]
    [InlineData(0, 1, 3)]
    [InlineData(2, 2, 8)]
    public void ToIndex_CalculatesCorrectly(int x, int y, int expectedIndex)
    {
        var grid = new Grid2D<int>(3, 3);
        int index = grid.ToIndex(x, y);
        Assert.Equal(expectedIndex, index);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(2, 2, 8)]
    [InlineData(0, 1, 3)]
    [InlineData(1, 1, 4)]
    public void ToXY_CalculatesCorrectly(int expectedX, int expectedY, int index)
    {
        var grid = new Grid2D<int>(3, 3);
        grid.ToXY(index, out int x, out int y);
        Assert.Equal(expectedX, x);
        Assert.Equal(expectedY, y);
    }

    [Fact]
    public void TryGet_ReturnsFalse_WhenOutOfBounds()
    {
        var grid = new Grid2D<int>(3, 3);

        bool result = grid.TryGet(-1, 0, out int val);
        Assert.False(result);

        result = grid.TryGet(3, 0, out val);
        Assert.False(result);
    }

    [Fact]
    public void GetNeighbors_Returns8_ForCenterCell()
    {
        var grid = new Grid2D<int>(10, 10);
        Span<int> buffer = stackalloc int[8];

        // (5,5) has 8 neighbors
        grid.GetNeighbors(5, 5, buffer, out int count);

        Assert.Equal(8, count);
    }

    [Fact]
    public void GetNeighbors_Returns3_ForCornerCell()
    {
        var grid = new Grid2D<int>(10, 10);
        Span<int> buffer = stackalloc int[8];

        // (0,0) has neighbors: (1,0), (0,1), (1,1) -> 3
        grid.GetNeighbors(0, 0, buffer, out int count);

        Assert.Equal(3, count);

        // Verify indices
        // (1,0) -> 1
        // (0,1) -> 10
        // (1,1) -> 11
        // Order depends on loop.

        var list = new System.Collections.Generic.List<int>();
        for(int i=0; i<count; i++) list.Add(buffer[i]);

        Assert.Contains(1, list);
        Assert.Contains(10, list);
        Assert.Contains(11, list);
    }
}
