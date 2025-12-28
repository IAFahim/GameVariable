using System.Runtime.CompilerServices;

namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(float current, float max, float amountToAdd, float tolerance = 0.001f)
    {
        return current + amountToAdd <= max + tolerance;
    }

    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(double current, double max, double amountToAdd, double tolerance = 0.001)
    {
        return current + amountToAdd <= max + tolerance;
    }

    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(int current, int max, int amountToAdd)
    {
        return current + amountToAdd <= max;
    }

    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(long current, long max, long amountToAdd)
    {
        return current + amountToAdd <= max;
    }

    /// <summary>
    ///     Checks if the inventory can accept a specific amount of items without exceeding its capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanAccept(byte current, byte max, byte amountToAdd)
    {
        return current + amountToAdd <= max;
    }
}