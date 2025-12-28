using System.Runtime.CompilerServices;

namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref float current, float value, float max)
    {
        if (value < 0f) value = 0f;
        if (value > max) value = max;

        if (value < TOLERANCE) value = 0f;
        if (max - value < TOLERANCE) value = max;

        current = value;
    }

    /// <summary>
    ///     Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Set(ref double current, double value, double max)
    {
        if (value < 0d) value = 0d;
        if (value > max) value = max;

        if (value < TOLERANCE_DOUBLE) value = 0d;
        if (max - value < TOLERANCE_DOUBLE) value = max;

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