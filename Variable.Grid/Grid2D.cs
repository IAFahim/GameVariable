namespace Variable.Grid;

/// <summary>
///     A zero-allocation, struct-based 2D grid.
///     Wraps a 1D array to provide 2D spatial semantics.
/// </summary>
/// <typeparam name="T">The type of data stored in the grid cells.</typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Grid2D<T>
{
    /// <summary>
    ///     The flattened 1D array containing the grid data.
    /// </summary>
    public T[] Cells;

    /// <summary>
    ///     The width (number of columns) of the grid.
    /// </summary>
    public int Width;

    /// <summary>
    ///     The height (number of rows) of the grid.
    /// </summary>
    public int Height;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Grid2D{T}"/> struct.
    /// </summary>
    /// <param name="width">The width of the grid.</param>
    /// <param name="height">The height of the grid.</param>
    public Grid2D(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new T[width * height];
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Grid2D{T}"/> struct with existing data.
    /// </summary>
    /// <param name="width">The width of the grid.</param>
    /// <param name="height">The height of the grid.</param>
    /// <param name="cells">The existing array to wrap.</param>
    public Grid2D(int width, int height, T[] cells)
    {
        if (cells.Length < width * height)
            throw new ArgumentException("Array length is smaller than width * height.");

        Width = width;
        Height = height;
        Cells = cells;
    }
}
