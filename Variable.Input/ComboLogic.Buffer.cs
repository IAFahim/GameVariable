namespace Variable.Input;

/// <summary>
///     Core logic for the combo system. Stateless, primitive-only operations.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static partial class ComboLogic
{
    /// <summary>
    ///     Attempts to add an input ID to the ring buffer.
    /// </summary>
    /// <param name="tail">Reference to the tail index.</param>
    /// <param name="count">Reference to the count.</param>
    /// <param name="buffer">The buffer span (must be length 8).</param>
    /// <param name="inputId">The input ID to enqueue.</param>
    /// <returns>True if the input was successfully added; false if the buffer is full.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryEnqueueInput(ref int tail, ref int count, Span<int> buffer, in int inputId)
    {
        if (count >= InputRingBuffer.CAPACITY) return false;

        buffer[tail] = inputId;

        tail = (tail + 1) % InputRingBuffer.CAPACITY;
        count++;
        return true;
    }

    /// <summary>
    ///     Attempts to remove and return the oldest input ID from the ring buffer.
    /// </summary>
    /// <param name="head">Reference to the head index.</param>
    /// <param name="count">Reference to the count.</param>
    /// <param name="buffer">The buffer span.</param>
    /// <param name="inputId">The dequeued input ID, or InputId.None if empty.</param>
    /// <returns>True if an input was dequeued; false if the buffer was empty.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDequeueInput(ref int head, ref int count, Span<int> buffer, out int inputId)
    {
        inputId = InputId.None;
        if (count == 0) return false;

        inputId = buffer[head];

        head = (head + 1) % InputRingBuffer.CAPACITY;
        count--;
        return true;
    }

    /// <summary>
    ///     Returns the oldest input ID from the ring buffer without removing it.
    /// </summary>
    /// <param name="head">The head index.</param>
    /// <param name="count">The count.</param>
    /// <param name="buffer">The buffer span.</param>
    /// <param name="inputId">The peeked input ID, or InputId.None if empty.</param>
    /// <returns>True if an input was found; false if the buffer was empty.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool PeekInput(in int head, in int count, ReadOnlySpan<int> buffer, out int inputId)
    {
        inputId = InputId.None;
        if (count == 0) return false;
        inputId = buffer[head];
        return true;
    }
}