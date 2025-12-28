using System.Runtime.CompilerServices;

namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(float current, float required)
    {
        return current >= required - TOLERANCE;
    }

    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(double current, double required)
    {
        return current >= required - TOLERANCE_DOUBLE;
    }

    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(int current, int required)
    {
        return current >= required;
    }

    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(long current, long required)
    {
        return current >= required;
    }

    /// <summary>
    ///     Checks if the inventory has enough items to satisfy a requirement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(byte current, byte required)
    {
        return current >= required;
    }
}