namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref float current, float value, float max, float tolerance = MathConstants.Tolerance)
    {
        if (max - value < tolerance) value = max;
        else if (value < tolerance) value = 0f;

        current = value;
    }

    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref int current, int value, int max)
    {
        if (value < 0) value = 0;
        if (value > max) value = max;
        current = value;
    }

    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref long current, long value, long max)
    {
        if (value < 0) value = 0;
        if (value > max) value = max;
        current = value;
    }

    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref byte current, byte value, byte max)
    {
        if (value > max) value = max;
        current = value;
    }
}