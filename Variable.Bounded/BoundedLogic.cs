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
        current = value > max ? max : (value < min ? min : value);
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
    ///     Sets an int value, clamping it to the specified bounds.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref int current, int min, int max, int value)
    {
        current = value > max ? max : (value < min ? min : value);
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
}
