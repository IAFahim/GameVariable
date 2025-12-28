namespace Variable.Inventory;

public static partial class InventoryLogic
{
    /// <summary>
    ///     Transfers as much of the requested amount as possible from a source to a destination.
    /// </summary>
    /// <param name="srcQty">The reference to the source quantity variable.</param>
    /// <param name="dstQty">The reference to the destination quantity variable.</param>
    /// <param name="dstMax">The maximum capacity of the destination.</param>
    /// <param name="requestedAmount">The amount requested to transfer.</param>
    /// <returns>The actual amount transferred.</returns>
    public static float TransferPartial(ref float srcQty, ref float dstQty, float dstMax, float requestedAmount,
        float tolerance = 0.001f)
    {
        if (srcQty <= tolerance || requestedAmount <= 0) return 0f;

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

            return actualMove;
        }

        return 0f;
    }
}