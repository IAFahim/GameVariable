using System.Runtime.CompilerServices;

namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Attempts to add an exact amount of items to the inventory.
    /// </summary>
    /// <param name="current">The reference to the current quantity variable.</param>
    /// <param name="amount">The amount to add.</param>
    /// <param name="max">The maximum capacity.</param>
    /// <returns><c>true</c> if the amount was added successfully; otherwise, <c>false</c> if it would exceed capacity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryAddExact(ref float current, float amount, float max, float tolerance = 0.001f)
    {
        if (amount <= tolerance) return true;
        if (current + amount > max + tolerance) return false;

        current += amount;
        if (max - current < tolerance) current = max;
        return true;
    }
}