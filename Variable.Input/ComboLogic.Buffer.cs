namespace Variable.Input;

/// <summary>
///     Core logic for the combo system. Stateless, primitive-only operations.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static partial class ComboLogic
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryEnqueueInput(ref InputRingBuffer buffer, int inputId)
    {
        if (buffer.Count >= InputRingBuffer.CAPACITY) return false;

        SetBufferAt(ref buffer, buffer.Tail, inputId);

        buffer.Tail = (buffer.Tail + 1) % InputRingBuffer.CAPACITY;
        buffer.Count++;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDequeueInput(ref InputRingBuffer buffer, out int inputId)
    {
        inputId = InputId.None;
        if (buffer.Count == 0) return false;

        inputId = GetBufferAt(ref buffer, buffer.Head);

        buffer.Head = (buffer.Head + 1) % InputRingBuffer.CAPACITY;
        buffer.Count--;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool PeekInput(ref InputRingBuffer buffer, out int inputId)
    {
        inputId = InputId.None;
        if (buffer.Count == 0) return false;
        inputId = GetBufferAt(ref buffer, buffer.Head);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetBufferAt(ref InputRingBuffer b, int index, int val)
    {
        switch (index)
        {
            case 0:
                b.Input0 = val;
                break;
            case 1:
                b.Input1 = val;
                break;
            case 2:
                b.Input2 = val;
                break;
            case 3:
                b.Input3 = val;
                break;
            case 4:
                b.Input4 = val;
                break;
            case 5:
                b.Input5 = val;
                break;
            case 6:
                b.Input6 = val;
                break;
            case 7:
                b.Input7 = val;
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetBufferAt(ref InputRingBuffer b, int index)
    {
        switch (index)
        {
            case 0: return b.Input0;
            case 1: return b.Input1;
            case 2: return b.Input2;
            case 3: return b.Input3;
            case 4: return b.Input4;
            case 5: return b.Input5;
            case 6: return b.Input6;
            case 7: return b.Input7;
        }

        return InputId.None;
    }
}
