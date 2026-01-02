namespace Variable.Input;

/// <summary>
///     Ring buffer for storing input sequences. Fixed capacity, zero allocation.
///     Uses explicit fields instead of arrays for full serialization compatibility.
///     Stores input IDs as integers for universal extensibility.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct InputRingBuffer
{
    /// <summary>
    ///     The maximum number of inputs the buffer can hold.
    /// </summary>
    public const int CAPACITY = 8;

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
}