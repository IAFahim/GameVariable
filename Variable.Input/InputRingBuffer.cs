namespace Variable.Input;

/// <summary>
///     Ring buffer for storing input sequences. Fixed capacity, zero allocation.
///     Uses explicit fields instead of arrays for full serialization compatibility.
///     Stores input IDs as integers for universal extensibility.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct InputRingBuffer : IEquatable<InputRingBuffer>
{
    /// <summary>
    ///     The maximum number of inputs the buffer can hold.
    /// </summary>
    public const int CAPACITY = InputBufferConfig.StandardCapacity;

    /// <summary>Slot 0 for input storage.</summary>
    public int Input0;

    /// <summary>Slot 1 for input storage.</summary>
    public int Input1;

    /// <summary>Slot 2 for input storage.</summary>
    public int Input2;

    /// <summary>Slot 3 for input storage.</summary>
    public int Input3;

    /// <summary>Slot 4 for input storage.</summary>
    public int Input4;

    /// <summary>Slot 5 for input storage.</summary>
    public int Input5;

    /// <summary>Slot 6 for input storage.</summary>
    public int Input6;

    /// <summary>Slot 7 for input storage.</summary>
    public int Input7;

    /// <summary>The index of the head (read) position.</summary>
    public int Head;

    /// <summary>The index of the tail (write) position.</summary>
    public int Tail;

    /// <summary>The current number of items in the buffer.</summary>
    public int Count;

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is InputRingBuffer other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(InputRingBuffer other)
    {
        return Head == other.Head && Tail == other.Tail && Count == other.Count &&
               Input0 == other.Input0 && Input1 == other.Input1 && Input2 == other.Input2 &&
               Input3 == other.Input3 && Input4 == other.Input4 && Input5 == other.Input5 &&
               Input6 == other.Input6 && Input7 == other.Input7;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Head);
        hash.Add(Tail);
        hash.Add(Count);
        hash.Add(Input0);
        hash.Add(Input1);
        hash.Add(Input2);
        hash.Add(Input3);
        hash.Add(Input4);
        hash.Add(Input5);
        hash.Add(Input6);
        hash.Add(Input7);
        return hash.ToHashCode();
    }

    /// <summary>Determines whether two buffers are equal.</summary>
    /// <param name="left">The first buffer to compare.</param>
    /// <param name="right">The second buffer to compare.</param>
    /// <returns>True if the buffers are equal; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(InputRingBuffer left, InputRingBuffer right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two buffers are not equal.</summary>
    /// <param name="left">The first buffer to compare.</param>
    /// <param name="right">The second buffer to compare.</param>
    /// <returns>True if the buffers are not equal; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(InputRingBuffer left, InputRingBuffer right)
    {
        return !left.Equals(right);
    }
}