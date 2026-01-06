namespace Variable.Inventory;

/// <summary>
///     Transfer operations for moving items between inventories.
///     These methods coordinate modifications across multiple inventory states.
/// </summary>
public static partial class InventoryLogic
{
    /// <summary>
    ///     Attempts to transfer as much of the requested amount as possible from a source to a destination.
    /// </summary>
    /// <param name="srcQty">The reference to the source quantity variable.</param>
    /// <param name="dstQty">The reference to the destination quantity variable.</param>
    /// <param name="dstMax">The maximum capacity of the destination.</param>
    /// <param name="requestedAmount">The amount requested to transfer.</param>
    /// <param name="actualTransferred">The actual amount transferred.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns>True if any amount was transferred; otherwise, false.</returns>
    public static bool TryTransferPartial(ref float srcQty, ref float dstQty, float dstMax, float requestedAmount,
        out float actualTransferred, float tolerance = MathConstants.DefaultTolerance)
    {
        if (srcQty <= tolerance || requestedAmount <= 0)
        {
            actualTransferred = 0f;
            return false;
        }

        var available = srcQty < requestedAmount ? srcQty : requestedAmount;

        var space = dstMax - dstQty;
        if (space < 0f) space = 0f;

        var actualMove = available < space ? available : space;

        if (actualMove > tolerance)
        {
            srcQty -= actualMove;
            dstQty += actualMove;

            if (srcQty < tolerance) srcQty = 0f;
            if (dstMax - dstQty < tolerance) dstQty = dstMax;

            actualTransferred = actualMove;
            return true;
        }

        actualTransferred = 0f;
        return false;
    }

    /// <summary>
    ///     Attempts to transfer as much of the requested amount as possible from a source to a destination, considering weight
    ///     limits.
    /// </summary>
    /// <param name="srcQty">The reference to the source quantity variable.</param>
    /// <param name="dstQty">The reference to the destination quantity variable.</param>
    /// <param name="dstCurWeight">The reference to the destination current weight variable.</param>
    /// <param name="dstMaxWeight">The maximum weight capacity of the destination.</param>
    /// <param name="dstMaxQty">The maximum quantity capacity of the destination.</param>
    /// <param name="unitWeight">The weight of a single unit of the item.</param>
    /// <param name="requestedAmount">The amount requested to transfer.</param>
    /// <param name="actualTransferred">The actual amount transferred.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns>True if any amount was transferred; otherwise, false.</returns>
    public static bool TryTransferPartialWithWeight(
        ref float srcQty,
        ref float dstQty,
        ref float dstCurWeight,
        float dstMaxWeight,
        float dstMaxQty, float unitWeight,
        float requestedAmount,
        out float actualTransferred,
        float tolerance = MathConstants.DefaultTolerance)
    {
        if (srcQty <= tolerance || requestedAmount <= 0)
        {
            actualTransferred = 0f;
            return false;
        }

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

            actualTransferred = actualMove;
            return true;
        }

        actualTransferred = 0f;
        return false;
    }

    /// <summary>
    ///     Attempts to transfer an exact amount of items from a source to a destination.
    /// </summary>
    /// <param name="srcQty">The reference to the source quantity variable.</param>
    /// <param name="dstQty">The reference to the destination quantity variable.</param>
    /// <param name="dstMax">The maximum capacity of the destination.</param>
    /// <param name="amount">The amount to transfer.</param>
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns>
    ///     <c>true</c> if the transfer was successful; otherwise, <c>false</c> if source has insufficient items or
    ///     destination is full.
    /// </returns>
    public static bool TryTransferExact(ref float srcQty, ref float dstQty, float dstMax, float amount,
        float tolerance = MathConstants.DefaultTolerance)
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
    /// <param name="tolerance">The tolerance for floating point comparisons.</param>
    /// <returns><c>true</c> if the transfer was successful; otherwise, <c>false</c> if constraints are not met.</returns>
    public static bool TryTransferExactWithWeight(
        ref float srcQty,
        ref float dstQty,
        ref float dstCurWeight,
        float dstMaxWeight,
        float dstMaxQty, float itemUnitWeight,
        float amount, float tolerance = MathConstants.DefaultTolerance)
    {
        if (amount <= tolerance) return true;

        if (srcQty < amount - tolerance) return false;

        if (dstQty + amount > dstMaxQty + tolerance) return false;

        var weightToAdd = amount * itemUnitWeight;
        if (dstCurWeight + weightToAdd > dstMaxWeight + tolerance) return false;

        srcQty -= amount;
        dstQty += amount;
        dstCurWeight += weightToAdd;

        if (srcQty < tolerance) srcQty = 0f;
        if (dstMaxQty - dstQty < tolerance) dstQty = dstMaxQty;
        if (dstMaxWeight - dstCurWeight < tolerance) dstCurWeight = dstMaxWeight;

        return true;
    }
}