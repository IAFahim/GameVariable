namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Attempts to transfer an exact amount of items from a source to a destination.
    /// </summary>
    /// <param name="srcQty">The reference to the source quantity variable.</param>
    /// <param name="dstQty">The reference to the destination quantity variable.</param>
    /// <param name="dstMax">The maximum capacity of the destination.</param>
    /// <param name="amount">The amount to transfer.</param>
    /// <returns>
    ///     <c>true</c> if the transfer was successful; otherwise, <c>false</c> if source has insufficient items or
    ///     destination is full.
    /// </returns>
    public static bool TryTransferExact(ref float srcQty, ref float dstQty, float dstMax, float amount,
        float tolerance = 0.001f)
    {
        if (amount <= tolerance) return true;

        if (srcQty < amount - tolerance) return false;
        if (dstQty + amount > dstMax + tolerance) return false;

        srcQty -= amount;
        dstQty += amount;

        if (srcQty < tolerance) srcQty = 0f;
        if (dstMax - dstQty < tolerance) dstQty = dstMax;

        return true;
    }
}