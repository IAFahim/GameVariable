namespace Variable.Bounded;

/// <summary>
///     Pure primitive logic for bounding values.
///     All methods operate on primitives only - NO STRUCTS.
/// </summary>
public static class BoundedLogic
{
    /// <summary>
    ///     Sets a float value, clamping it to the specified bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref float current, float min, float max, float value)
    {
        current = value > max ? max : value < min ? min : value;
    }

    /// <summary>
    ///     Normalizes (clamps) a float value to the specified bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref float current, float min, float max)
    {
        if (current > max) current = max;
        else if (current < min) current = min;
    }

    /// <summary>
    ///     Calculates the range between min and max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRange(float min, float max)
    {
        return max - min;
    }

    /// <summary>
    ///     Calculates the amount remaining until max.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetRemaining(float current, float max)
    {
        return max - current;
    }

    /// <summary>
    ///     Sets an int value, clamping it to the specified bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref int current, int min, int max, int value)
    {
        current = value > max ? max : value < min ? min : value;
    }

    /// <summary>
    ///     Normalizes (clamps) an int value to the specified bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref int current, int min, int max)
    {
        if (current > max) current = max;
        else if (current < min) current = min;
    }

    /// <summary>
    ///     Calculates the range between min and max (int version).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRange(int min, int max)
    {
        return max - min;
    }

    /// <summary>
    ///     Calculates the amount remaining until max (int version).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRemaining(int current, int max)
    {
        return max - current;
    }

    /// <summary>
    ///     Sets a byte value, clamping it to the specified bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref byte current, byte max, byte value)
    {
        current = value > max ? max : value;
    }

    /// <summary>
    ///     Normalizes (clamps) a byte value to the specified max (min is always 0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Normalize(ref byte current, byte max)
    {
        if (current > max) current = max;
    }

    /// <summary>
    ///     Calculates the range for byte (which is just max since min is always 0).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetRange(byte max)
    {
        return max;
    }

    /// <summary>
    ///     Calculates the amount remaining until max (byte version).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetRemaining(byte current, byte max)
    {
        return (byte)(max - current);
    }
}