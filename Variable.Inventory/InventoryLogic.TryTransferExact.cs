using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Attempts to transfer an exact amount of items from a source to a destination.
        /// </summary>
        /// <param name="srcQty">The reference to the source quantity variable.</param>
        /// <param name="dstQty">The reference to the destination quantity variable.</param>
        /// <param name="dstMax">The maximum capacity of the destination.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <returns><c>true</c> if the transfer was successful; otherwise, <c>false</c> if source has insufficient items or destination is full.</returns>
        public static bool TryTransferExact(ref float srcQty, ref float dstQty, float dstMax, float amount)
        {
            if (amount <= TOLERANCE) return true;

            if (srcQty < amount - TOLERANCE) return false;
            if (dstQty + amount > dstMax + TOLERANCE) return false;

            srcQty -= amount;
            dstQty += amount;

            if (srcQty < TOLERANCE) srcQty = 0f;
            if (dstMax - dstQty < TOLERANCE) dstQty = dstMax;

            return true;
        }
    }
}
