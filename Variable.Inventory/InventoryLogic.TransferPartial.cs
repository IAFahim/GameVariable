namespace Variable.Inventory;

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
        out float actualTransferred, float tolerance = MathConstants.Tolerance)
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
}