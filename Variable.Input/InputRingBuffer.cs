namespace Variable.Input;

/// <summary>
///     Ring buffer for storing input sequences. Fixed capacity, zero allocation.
///     Uses explicit fields instead of arrays for full serialization compatibility.
///     Stores input IDs as integers for universal extensibility.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct InputRingBuffer
{
    public const int CAPACITY = 8;

    public int Input0;
    public int Input1;
    public int Input2;
    public int Input3;
    public int Input4;
    public int Input5;
    public int Input6;
    public int Input7;

    public int Head;
    public int Tail;
    public int Count;
}
