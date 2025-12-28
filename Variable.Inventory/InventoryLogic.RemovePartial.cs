namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Removes as much of the specified amount as possible from the inventory.
    /// </summary>
    /// <param name="current">The reference to the current quantity variable.</param>
    /// <param name="amount">The amount to attempt to remove.</param>
    /// <returns>The actual amount removed from the inventory.</returns>
    public static float RemovePartial(ref float current, float amount, float tolerance = 0.001f)
    {
        if (amount <= tolerance || current <= tolerance) return 0f;

        var toRemove = current < amount ? current : amount;

        current -= toRemove;
        if (current < tolerance) current = 0f;

        return toRemove;
    }
}