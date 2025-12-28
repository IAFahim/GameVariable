namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Transfers as much of the requested amount as possible from a source to a destination, considering weight limits.
    /// </summary>
    /// <param name="srcQty">The reference to the source quantity variable.</param>
    /// <param name="dstQty">The reference to the destination quantity variable.</param>
    /// <param name="dstCurWeight">The reference to the destination current weight variable.</param>
    /// <param name="dstMaxWeight">The maximum weight capacity of the destination.</param>
    /// <param name="dstMaxQty">The maximum quantity capacity of the destination.</param>
    /// <param name="unitWeight">The weight of a single unit of the item.</param>
    /// <param name="requestedAmount">The amount requested to transfer.</param>
    /// <returns>The actual amount transferred.</returns>
    public static float TransferPartialWithWeight(
        ref float srcQty,
        ref float dstQty,
        ref float dstCurWeight,
        float dstMaxWeight,
        float dstMaxQty, float unitWeight,
        float requestedAmount, float tolerance = 0.001f)
    {
        if (srcQty <= tolerance || requestedAmount <= 0) return 0f;

        var limitSrc = srcQty < requestedAmount ? srcQty : requestedAmount;

        var slotSpace = dstMaxQty - dstQty;
        if (slotSpace < 0f) slotSpace = 0f;

        var weightLimit = float.MaxValue;
        if (unitWeight > tolerance)
        {
            var weightSpace = dstMaxWeight - dstCurWeight;
            if (weightSpace < 0f) weightSpace = 0f;
            weightLimit = weightSpace / unitWeight;
        }

        var actualMove = limitSrc;
        if (slotSpace < actualMove) actualMove = slotSpace;
        if (weightLimit < actualMove) actualMove = weightLimit;

        if (actualMove > tolerance)
        {
            srcQty -= actualMove;
            dstQty += actualMove;
            dstCurWeight += actualMove * unitWeight;

            if (srcQty < tolerance) srcQty = 0f;
            if (dstMaxQty - dstQty < tolerance) dstQty = dstMaxQty;
            if (dstMaxWeight - dstCurWeight < tolerance) dstCurWeight = dstMaxWeight;

            return actualMove;
        }

        return 0f;
    }
}