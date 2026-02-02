using System;
using Xunit;
using Variable.Grid;

namespace Variable.Grid.Tests;

public class GridTests
{
    [Fact]
    public void Constructor_Valid_CreatesGrid()
    {
        var grid = new Grid2D<int>(10, 5);
        Assert.Equal(10, grid.Width);
        Assert.Equal(5, grid.Height);
        Assert.Equal(50, grid.Length);
        Assert.NotNull(grid.Data);
        Assert.Equal(50, grid.Data.Length);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    [InlineData(-1, 10)]
    [InlineData(10, -1)]
    public void Constructor_Invalid_Throws(int w, int h)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Grid2D<int>(w, h));
    }

    [Fact]
    public void Indexer_SetGet_Works()
    {
        var grid = new Grid2D<int>(5, 5);
        grid[2, 3] = 42;
        Assert.Equal(42, grid[2, 3]);

        // Check flat index too
        int flatIndex = 3 * 5 + 2; // y * width + x
        Assert.Equal(42, grid[flatIndex]);
    }

    [Fact]
    public void Indexer_OutOfBounds_Throws()
    {
        var grid = new Grid2D<int>(5, 5);
        Assert.Throws<IndexOutOfRangeException>(() => grid[5, 0]); // x too big
        Assert.Throws<IndexOutOfRangeException>(() => grid[0, 5]); // y too big
        Assert.Throws<IndexOutOfRangeException>(() => grid[-1, 0]);
        // Note: C# array might throw IndexOutOfRangeException for negative indices on the underlying array if accessed directly,
        // but our strict check in the indexer handles high bounds.
        // For negative numbers, (uint) cast trick handles it.
    }

    [Fact]
    public void Coordinate_RoundTrip()
    {
        var grid = new Grid2D<int>(10, 10);
        int x = 3;
        int y = 7;

        int index = grid.GetIndex(x, y);
        var coords = grid.GetCoordinates(index);

        Assert.Equal(x, coords.x);
        Assert.Equal(y, coords.y);
    }

    [Fact]
    public void Fill_Works()
    {
        var grid = new Grid2D<int>(3, 3);
        grid.Fill(99);

        for(int i=0; i<grid.Length; i++)
        {
            Assert.Equal(99, grid[i]);
        }
    }

    [Fact]
    public void Clear_Works()
    {
        var grid = new Grid2D<int>(3, 3);
        grid.Fill(99);
        grid.Clear();

        for(int i=0; i<grid.Length; i++)
        {
            Assert.Equal(0, grid[i]);
        }
    }

    [Fact]
    public void GetRow_Works()
    {
        var grid = new Grid2D<int>(3, 3);
        // Row 0: 0, 1, 2
        // Row 1: 3, 4, 5
        // Row 2: 6, 7, 8
        for(int i=0; i<grid.Length; i++) grid[i] = i;

        var row1 = grid.GetRow(1);
        Assert.Equal(3, row1.Length);
        Assert.Equal(3, row1[0]);
        Assert.Equal(4, row1[1]);
        Assert.Equal(5, row1[2]);
    }

    [Fact]
    public void CopyColumn_Works()
    {
        var grid = new Grid2D<int>(3, 3);
        // Row 0: 0, 1, 2
        // Row 1: 3, 4, 5
        // Row 2: 6, 7, 8
        for(int i=0; i<grid.Length; i++) grid[i] = i;

        Span<int> col1 = stackalloc int[3];
        grid.CopyColumn(1, col1); // Should be 1, 4, 7

        Assert.Equal(1, col1[0]);
        Assert.Equal(4, col1[1]);
        Assert.Equal(7, col1[2]);
    }

    [Fact]
    public void UnsafeGet_Works()
    {
        var grid = new Grid2D<int>(3, 3);
        grid[1, 1] = 42;

        ref int val = ref grid.UnsafeGet(1, 1);
        Assert.Equal(42, val);

        val = 99; // modify via ref
        Assert.Equal(99, grid[1, 1]);
    }

    [Fact]
    public void UnsafeGet_Flat_Works()
    {
        var grid = new Grid2D<int>(3, 3);
        grid[4] = 42; // index 4 is (1, 1)

        ref int val = ref grid.UnsafeGet(4);
        Assert.Equal(42, val);

        val = 100;
        Assert.Equal(100, grid[4]);
    }

    [Fact]
    public void AsSpan_Works()
    {
        var grid = new Grid2D<int>(3, 3);
        grid.Fill(7);

        var span = grid.AsSpan();
        Assert.Equal(9, span.Length);
        Assert.Equal(7, span[0]);
        Assert.Equal(7, span[8]);

        span[0] = 123;
        Assert.Equal(123, grid[0]);
    }
}
