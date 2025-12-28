using System.Runtime.CompilerServices;

namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Determines whether the inventory is full based on the current quantity and maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(float current, float max, float tolerance = 0.001f)
    {
        return current >= max - tolerance;
    }

    /// <summary>
    ///     Determines whether the inventory is full based on the current quantity and maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(int current, int max)
    {
        return current >= max;
    }

    /// <summary>
    ///     Determines whether the inventory is full based on the current quantity and maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(long current, long max)
    {
        return current >= max;
    }

    /// <summary>
    ///     Determines whether the inventory is full based on the current quantity and maximum capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFull(byte current, byte max)
    {
        return current >= max;
    }
}