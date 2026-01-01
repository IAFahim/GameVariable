namespace Variable.Input;

/// <summary>
///     Extension methods for InputRingBuffer. Sugar layer over logic.
/// </summary>
public static class InputRingBufferExtensions
{
    /// <summary>
    ///     Attempts to register an input into the buffer.
    /// </summary>
    public static bool RegisterInput(ref this InputRingBuffer buffer, int inputId)
    {
        return ComboLogic.TryEnqueueInput(ref buffer, inputId);
    }

    /// <summary>
    ///     Peeks at the next input without consuming it.
    /// </summary>
    public static bool Peek(ref this InputRingBuffer buffer, out int inputId)
    {
        return ComboLogic.PeekInput(ref buffer, out inputId);
    }

    /// <summary>
    ///     Clears all inputs from the buffer.
    /// </summary>
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
