namespace Variable.Grid;

/// <summary>
///     Extension methods for <see cref="Grid2D{T}"/>.
/// </summary>
public static class GridExtensions
{
    /// <summary>
    ///     Fills the entire grid with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the grid.</typeparam>
    /// <param name="grid">The grid to fill.</param>
    /// <param name="value">The value to fill with.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Fill<T>(this Grid2D<T> grid, T value)
    {
        Array.Fill(grid.Data, value);
    }

    /// <summary>
    ///     Clears the grid (sets all elements to their default value).
    /// </summary>
    /// <typeparam name="T">The type of elements in the grid.</typeparam>
    /// <param name="grid">The grid to clear.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear<T>(this Grid2D<T> grid)
    {
        Array.Clear(grid.Data, 0, grid.Data.Length);
    }

    /// <summary>
    ///     Gets a row as a Span.
    /// </summary>
    /// <typeparam name="T">The type of elements in the grid.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="y">The row index (Y coordinate).</param>
    /// <returns>A Span representing the row.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if y is out of bounds.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> GetRow<T>(this Grid2D<T> grid, int y)
    {
        if ((uint)y >= (uint)grid.Height) throw new ArgumentOutOfRangeException(nameof(y));
        return grid.Data.AsSpan(y * grid.Width, grid.Width);
    }

    /// <summary>
    ///     Copies a column into a destination Span.
    /// </summary>
    /// <typeparam name="T">The type of elements in the grid.</typeparam>
    /// <param name="grid">The grid.</param>
    /// <param name="x">The column index (X coordinate).</param>
    /// <param name="destination">The destination span (must be at least Grid.Height in length).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if x is out of bounds.</exception>
    /// <exception cref="ArgumentException">Thrown if destination is too short.</exception>
    public static void CopyColumn<T>(this Grid2D<T> grid, int x, Span<T> destination)
    {
        if ((uint)x >= (uint)grid.Width) throw new ArgumentOutOfRangeException(nameof(x));
        if (destination.Length < grid.Height) throw new ArgumentException("Destination buffer is too short.", nameof(destination));

        // Manual loop is necessary as columns are not contiguous in memory
        int height = grid.Height;
        int width = grid.Width;
        var data = grid.Data;

        for (int y = 0; y < height; y++)
        {
            destination[y] = data[y * width + x];
        }
    }
}
