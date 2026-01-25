namespace Variable.Grid;

/// <summary>
///     Extensions for <see cref="Grid2D{T}"/>.
///     Provides the user-friendly API for the Grid system.
/// </summary>
public static class GridExtensions
{
    /// <summary>
    ///     Sets the value at the specified coordinates.
    /// </summary>
    /// <typeparam name="T">The type of the cell data.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="value">The value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set<T>(ref this Grid2D<T> grid, int x, int y, T value)
    {
        GridLogic.ToIndex(in x, in y, in grid.Width, out var index);
        grid.Cells[index] = value;
    }

    /// <summary>
    ///     Sets the value at the specified 1D index.
    /// </summary>
    /// <typeparam name="T">The type of the cell data.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="index">The 1D index.</param>
    /// <param name="value">The value to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set<T>(ref this Grid2D<T> grid, int index, T value)
    {
        grid.Cells[index] = value;
    }

    /// <summary>
    ///     Gets the value at the specified coordinates.
    /// </summary>
    /// <typeparam name="T">The type of the cell data.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns>The value at the coordinates.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Get<T>(in this Grid2D<T> grid, int x, int y)
    {
        GridLogic.ToIndex(in x, in y, in grid.Width, out var index);
        return grid.Cells[index];
    }

    /// <summary>
    ///     Gets the value at the specified 1D index.
    /// </summary>
    /// <typeparam name="T">The type of the cell data.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="index">The 1D index.</param>
    /// <returns>The value at the index.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Get<T>(in this Grid2D<T> grid, int index)
    {
        return grid.Cells[index];
    }

    /// <summary>
    ///     Tries to get the value at the specified coordinates.
    /// </summary>
    /// <typeparam name="T">The type of the cell data.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="value">The value if found, otherwise default.</param>
    /// <returns>True if the coordinates are valid, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGet<T>(in this Grid2D<T> grid, int x, int y, out T value)
    {
        GridLogic.IsValid(in x, in y, in grid.Width, in grid.Height, out var valid);
        if (valid)
        {
            GridLogic.ToIndex(in x, in y, in grid.Width, out var index);
            value = grid.Cells[index];
            return true;
        }

        value = default!;
        return false;
    }

    /// <summary>
    ///     Calculates the 1D index for the given 2D coordinates.
    /// </summary>
    /// <typeparam name="T">The type of the cell data.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns>The 1D index.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToIndex<T>(in this Grid2D<T> grid, int x, int y)
    {
        GridLogic.ToIndex(in x, in y, in grid.Width, out var index);
        return index;
    }

    /// <summary>
    ///     Calculates the 2D coordinates for the given 1D index.
    /// </summary>
    /// <typeparam name="T">The type of the cell data.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="index">The 1D index.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToXY<T>(in this Grid2D<T> grid, int index, out int x, out int y)
    {
        GridLogic.ToXY(in index, in grid.Width, out x, out y);
    }

    /// <summary>
    ///     Gets the indices of all valid neighbors (Moore Neighborhood).
    /// </summary>
    /// <typeparam name="T">The type of the cell data.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="results">Buffer to store indices (size 8).</param>
    /// <param name="count">Number of neighbors found.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetNeighbors<T>(in this Grid2D<T> grid, int x, int y, in Span<int> results, out int count)
    {
        GridLogic.ToIndex(in x, in y, in grid.Width, out var index);
        GridLogic.GetNeighbors(in index, in grid.Width, in grid.Height, in results, out count);
    }
}
