namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Attempts to remove as much of the specified amount as possible from the inventory.
    /// </summary>
    /// <param name="current">The reference to the current quantity variable.</param>
    /// <param name="amount">The amount to attempt to remove.</param>
    /// <param name="actualRemoved">The actual amount removed from the inventory.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns>True if any amount was removed; otherwise, false.</returns>
    public static bool TryRemovePartial(ref float current, float amount, out float actualRemoved,
        float tolerance = MathConstants.Tolerance)
    {
        if (amount <= tolerance || current <= tolerance)
        {
            actualRemoved = 0f;
            return false;
        }

        var toRemove = current < amount ? current : amount;

        current -= toRemove;
        if (current < tolerance) current = 0f;

        actualRemoved = toRemove;
        return true;
    }
}