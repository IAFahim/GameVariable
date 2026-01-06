namespace Variable.Input;

/// <summary>
///     Ring buffer for storing input sequences with 16-slot capacity.
///     Uses explicit fields instead of arrays for full serialization compatibility.
///     Stores input IDs as integers for universal extensibility.
/// </summary>
/// <remarks>
///     Use this variant for games requiring larger combo buffers (e.g., fighting games with 10+ hit combos).
///     For most games, the standard 8-slot <see cref="InputRingBuffer"/> is sufficient.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct InputRingBuffer16 : IEquatable<InputRingBuffer16>
{
    /// <summary>
    ///     The maximum number of inputs the buffer can hold.
    /// </summary>
    public const int CAPACITY = 16;

    // Input slots 0-15
    public int Input0;
    public int Input1;
    public int Input2;
    public int Input3;
    public int Input4;
    public int Input5;
    public int Input6;
    public int Input7;
    public int Input8;
    public int Input9;
    public int Input10;
    public int Input11;
    public int Input12;
    public int Input13;
    public int Input14;
    public int Input15;

    /// <summary>The index of the head (read) position.</summary>
    public int Head;

    /// <summary>The index of the tail (write) position.</summary>
    public int Tail;

    /// <summary>The current number of items in the buffer.</summary>
    public int Count;

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is InputRingBuffer16 other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(InputRingBuffer16 other)
    {
        return Head == other.Head && Tail == other.Tail && Count == other.Count &&
               Input0 == other.Input0 && Input1 == other.Input1 && Input2 == other.Input2 &&
               Input3 == other.Input3 && Input4 == other.Input4 && Input5 == other.Input5 &&
               Input6 == other.Input6 && Input7 == other.Input7 && Input8 == other.Input8 &&
               Input9 == other.Input9 && Input10 == other.Input10 && Input11 == other.Input11 &&
               Input12 == other.Input12 && Input13 == other.Input13 && Input14 == other.Input14 &&
               Input15 == other.Input15;
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
        hash.Add(Input8);
        hash.Add(Input9);
        hash.Add(Input10);
        hash.Add(Input11);
        hash.Add(Input12);
        hash.Add(Input13);
        hash.Add(Input14);
        hash.Add(Input15);
        return hash.ToHashCode();
    }

    /// <summary>Determines whether two buffers are equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(InputRingBuffer16 left, InputRingBuffer16 right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two buffers are not equal.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(InputRingBuffer16 left, InputRingBuffer16 right)
    {
        return !left.Equals(right);
    }
}
