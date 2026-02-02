namespace Variable.Grid;

/// <summary>
///     A 2D grid structure backed by a flat 1D array for high performance and zero-allocation indexing.
///     <para>Think of this as an Excel sheet stored as a single long line of cells.</para>
/// </summary>
/// <typeparam name="T">The type of elements in the grid.</typeparam>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Grid2D<T> : IEquatable<Grid2D<T>>
{
    /// <summary>
    ///     The flat array storing the grid data.
    ///     <para>Warning: Modifying this array directly bypasses bounds checking.</para>
    /// </summary>
    public T[] Data;

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
    /// <param name="width">The width of the grid (must be > 0).</param>
    /// <param name="height">The height of the grid (must be > 0).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if width or height is zero or negative.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Grid2D(int width, int height)
    {
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

        Width = width;
        Height = height;
        Data = new T[width * height];
    }

    /// <summary>
    ///     Gets the total number of cells in the grid.
    /// </summary>
    public readonly int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Data.Length;
    }

    /// <summary>
    ///     Accesses the element at the specified 2D coordinates.
    /// </summary>
    /// <param name="x">The X coordinate (column).</param>
    /// <param name="y">The Y coordinate (row).</param>
    /// <returns>A reference to the element at the specified coordinates.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if coordinates are out of bounds.</exception>
    public ref T this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            // Optional: Bounds checking could be omitted in Release for speed if desired,
            // but for safety we keep standard array checks implicitly via flat index.
            // We manually check bounds here to ensure x/y logic is correct.
            if ((uint)x >= (uint)Width || (uint)y >= (uint)Height)
                throw new IndexOutOfRangeException($"Grid coordinates ({x}, {y}) out of bounds ({Width}x{Height}).");

            return ref Data[y * Width + x];
        }
    }

    /// <summary>
    ///     Accesses the element at the specified flat index.
    /// </summary>
    /// <param name="index">The flat index in the underlying array.</param>
    /// <returns>A reference to the element at the specified index.</returns>
    public ref T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Data[index];
    }

    /// <summary>
    ///     Calculates the flat index for the given 2D coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <returns>The flat index.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int GetIndex(int x, int y)
    {
        return y * Width + x;
    }

    /// <summary>
    ///     Calculates the 2D coordinates for the given flat index.
    /// </summary>
    /// <param name="index">The flat index.</param>
    /// <returns>A tuple containing (x, y) coordinates.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly (int x, int y) GetCoordinates(int index)
    {
        int y = index / Width;
        int x = index % Width;
        return (x, y);
    }

    /// <summary>
    ///     Checks if the given coordinates are within the grid bounds.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <returns>True if the coordinates are valid, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsValid(int x, int y)
    {
        return (uint)x < (uint)Width && (uint)y < (uint)Height;
    }

    /// <summary>
    ///     Accesses the element at the specified 2D coordinates without bounds checking.
    ///     <para>Warning: Extreme danger. Use only when you are 100% sure the coordinates are valid.</para>
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <returns>A reference to the element.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T UnsafeGet(int x, int y)
    {
        // y * Width + x might overflow if the grid is massive (over 2 billion elements),
        // but standard array indexing has the same limit.
        return ref Unsafe.Add(ref MemoryMarshal.GetReference(new Span<T>(Data)), y * Width + x);
    }

    /// <summary>
    ///     Accesses the element at the specified flat index without bounds checking.
    /// </summary>
    /// <param name="index">The flat index.</param>
    /// <returns>A reference to the element.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T UnsafeGet(int index)
    {
        return ref Unsafe.Add(ref MemoryMarshal.GetReference(new Span<T>(Data)), index);
    }

    /// <summary>
    ///     Returns the grid data as a Span.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
        return new Span<T>(Data);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Grid2D<T> other)
    {
        // Reference equality for the array is sufficient as this is a "View" into data.
        // Two grids are "Equal" if they point to the same data and have same dimensions.
        return Data == other.Data && Width == other.Width && Height == other.Height;
    }

    /// <inheritdoc />
    public readonly override bool Equals(object? obj)
    {
        return obj is Grid2D<T> other && Equals(other);
    }

    /// <inheritdoc />
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Data, Width, Height);
    }

    /// <summary>
    ///     Determines whether two Grid2D instances are equal.
    /// </summary>
    public static bool operator ==(Grid2D<T> left, Grid2D<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Determines whether two Grid2D instances are not equal.
    /// </summary>
    public static bool operator !=(Grid2D<T> left, Grid2D<T> right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc />
    public readonly override string ToString()
    {
        return $"Grid2D<{typeof(T).Name}>({Width}x{Height})";
    }
}
