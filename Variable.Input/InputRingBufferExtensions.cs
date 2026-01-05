namespace Variable.Input;

/// <summary>
///     Extension methods for InputRingBuffer. Sugar layer over logic.
/// </summary>
public static class InputRingBufferExtensions
{
    /// <summary>
    ///     Attempts to register an input into the buffer.
    /// </summary>
    /// <param name="buffer">The buffer to register input into.</param>
    /// <param name="inputId">The input ID to register.</param>
    /// <returns>True if the input was registered; otherwise, false.</returns>
    public static bool RegisterInput(ref this InputRingBuffer buffer, int inputId)
    {
        var inputs = MemoryMarshal.CreateSpan(ref buffer.Input0, 8);
        return ComboLogic.TryEnqueueInput(ref buffer.Tail, ref buffer.Count, inputs, inputId);
    }

    /// <summary>
    ///     Peeks at the next input without consuming it.
    /// </summary>
    /// <param name="buffer">The buffer to peek from.</param>
    /// <param name="inputId">The peeked input ID.</param>
    /// <returns>True if an input was peeked; otherwise, false.</returns>
    public static bool Peek(ref this InputRingBuffer buffer, out int inputId)
    {
        var inputs = MemoryMarshal.CreateReadOnlySpan(ref buffer.Input0, 8);
        return ComboLogic.PeekInput(buffer.Head, buffer.Count, inputs, out inputId);
    }

    /// <summary>
    ///     Attempts to remove and return the oldest input ID from the ring buffer.
    /// </summary>
    /// <param name="buffer">The buffer to dequeue from.</param>
    /// <param name="inputId">The dequeued input ID.</param>
    /// <returns>True if an input was dequeued; otherwise, false.</returns>
    public static bool TryDequeue(ref this InputRingBuffer buffer, out int inputId)
    {
        var inputs = MemoryMarshal.CreateSpan(ref buffer.Input0, 8);
        return ComboLogic.TryDequeueInput(ref buffer.Head, ref buffer.Count, inputs, out inputId);
    }

    /// <summary>
    ///     Clears all inputs from the buffer.
    /// </summary>
    /// <param name="buffer">The buffer to clear.</param>
    public static void Clear(ref this InputRingBuffer buffer)
    {
        buffer.Head = 0;
        buffer.Tail = 0;
        buffer.Count = 0;
        buffer.Input0 = InputId.None;
        buffer.Input1 = InputId.None;
        buffer.Input2 = InputId.None;
        buffer.Input3 = InputId.None;
        buffer.Input4 = InputId.None;
        buffer.Input5 = InputId.None;
        buffer.Input6 = InputId.None;
        buffer.Input7 = InputId.None;
    }
}