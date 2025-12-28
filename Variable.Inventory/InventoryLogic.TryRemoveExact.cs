namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Attempts to remove an exact amount of items from the inventory.
    /// </summary>
    /// <param name="current">The reference to the current quantity variable.</param>
    /// <param name="amount">The amount to remove.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns><c>true</c> if the amount was removed successfully; otherwise, <c>false</c> if there were insufficient items.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryRemoveExact(ref float current, float amount, float tolerance = MathConstants.Tolerance)
    {
        if (amount <= tolerance) return true;
        if (current < amount - tolerance) return false;

        current -= amount;
        if (current < tolerance) current = 0f;
        return true;
    }
}