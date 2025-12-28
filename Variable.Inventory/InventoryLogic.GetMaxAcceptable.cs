namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Calculates the maximum amount of an item that can be accepted, considering both quantity and weight limits.
    /// </summary>
    /// <param name="currentQty">The current quantity of items.</param>
    /// <param name="maxQty">The maximum quantity capacity.</param>
    /// <param name="currentWeight">The current total weight.</param>
    /// <param name="maxWeight">The maximum weight capacity.</param>
    /// <param name="unitWeight">The weight of a single unit of the item.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns>The maximum amount that can be accepted.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMaxAcceptable(
        float currentQty,
        float maxQty,
        float currentWeight,
        float maxWeight,
        float unitWeight,
        float tolerance = MathConstants.Tolerance)
    {
        var spaceByQty = currentQty >= maxQty ? 0f : maxQty - currentQty;

        if (unitWeight <= tolerance) return spaceByQty;

        var remainingWeight = currentWeight >= maxWeight ? 0f : maxWeight - currentWeight;
        var spaceByWeight = remainingWeight / unitWeight;

        return spaceByQty < spaceByWeight ? spaceByQty : spaceByWeight;
    }
}