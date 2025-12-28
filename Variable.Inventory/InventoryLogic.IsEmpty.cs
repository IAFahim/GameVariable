using System.Runtime.CompilerServices;

namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(float current, float tolerance = 0.001f)
    {
        return current <= tolerance;
    }

    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(double current, double tolerance = 0.001)
    {
        return current <= tolerance;
    }

    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(int current)
    {
        return current <= 0;
    }

    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(long current)
    {
        return current <= 0;
    }

    /// <summary>
    ///     Determines whether the inventory is empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(byte current)
    {
        return current <= 0;
    }
}