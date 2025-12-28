namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Attempts to transfer an exact amount of items from a source to a destination, considering weight limits.
    /// </summary>
    /// <param name="srcQty">The reference to the source quantity variable.</param>
    /// <param name="dstQty">The reference to the destination quantity variable.</param>
    /// <param name="dstCurWeight">The reference to the destination current weight variable.</param>
    /// <param name="dstMaxWeight">The maximum weight capacity of the destination.</param>
    /// <param name="dstMaxQty">The maximum quantity capacity of the destination.</param>
    /// <param name="itemUnitWeight">The weight of a single unit of the item.</param>
    /// <param name="amount">The amount to transfer.</param>
    /// <returns><c>true</c> if the transfer was successful; otherwise, <c>false</c> if constraints are not met.</returns>
    public static bool TryTransferExactWithWeight(
        ref float srcQty,
        ref float dstQty,
        ref float dstCurWeight,
        float dstMaxWeight,
        float dstMaxQty, float itemUnitWeight,
        float amount)
    {
        if (amount <= TOLERANCE) return true;

        if (srcQty < amount - TOLERANCE) return false;

        if (dstQty + amount > dstMaxQty + TOLERANCE) return false;

        var weightToAdd = amount * itemUnitWeight;
        if (dstCurWeight + weightToAdd > dstMaxWeight + TOLERANCE) return false;

        srcQty -= amount;
        dstQty += amount;
        dstCurWeight += weightToAdd;

        if (srcQty < TOLERANCE) srcQty = 0f;
        if (dstMaxQty - dstQty < TOLERANCE) dstQty = dstMaxQty;
        if (dstMaxWeight - dstCurWeight < TOLERANCE) dstCurWeight = dstMaxWeight;

        return true;
    }
}