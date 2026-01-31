namespace Variable.Grid;

/// <summary>
///     Stateless logic for grid mathematics.
/// </summary>
public static class GridLogic
{
    /// <summary>
    ///     Converts a 2D coordinate (x, y) to a 1D index.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="width">The width of the grid.</param>
    /// <param name="index">The calculated 1D index.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToIndex(in int x, in int y, in int width, out int index)
    {
        index = y * width + x;
    }

    /// <summary>
    ///     Converts a 1D index to a 2D coordinate (x, y).
    /// </summary>
    /// <param name="index">The 1D index.</param>
    /// <param name="width">The width of the grid.</param>
    /// <param name="x">The calculated x coordinate.</param>
    /// <param name="y">The calculated y coordinate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToXY(in int index, in int width, out int x, out int y)
    {
        x = index % width;
        y = index / width;
    }

    /// <summary>
    ///     Checks if a coordinate is within the grid bounds.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="width">The width of the grid.</param>
    /// <param name="height">The height of the grid.</param>
    /// <param name="isValid">True if the coordinate is valid, otherwise false.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsValid(in int x, in int y, in int width, in int height, out bool isValid)
    {
        isValid = x >= 0 && x < width && y >= 0 && y < height;
    }

    /// <summary>
    ///     Gets the valid neighbor indices for a given cell (Moore Neighborhood - 8 directions).
    /// </summary>
    /// <param name="index">The central cell index.</param>
    /// <param name="width">The grid width.</param>
    /// <param name="height">The grid height.</param>
    /// <param name="results">A buffer to store the neighbor indices (must be at least size 8).</param>
    /// <param name="count">The number of valid neighbors found.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetNeighbors(in int index, in int width, in int height, in Span<int> results, out int count)
    {
        count = 0;
        ToXY(in index, in width, out var x, out var y);

        // Directions: Top-Left, Top, Top-Right, Left, Right, Bot-Left, Bot, Bot-Right
        // Optimized: Loop from y-1 to y+1, x-1 to x+1
        for (var ny = y - 1; ny <= y + 1; ny++)
        {
            for (var nx = x - 1; nx <= x + 1; nx++)
            {
                if (nx == x && ny == y) continue;

                IsValid(in nx, in ny, in width, in height, out var valid);
                if (valid)
                {
                    ToIndex(in nx, in ny, in width, out var idx);
                    results[count++] = idx;
                }
            }
        }
    }
}
