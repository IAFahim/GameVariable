using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Transfers as much of the requested amount as possible from a source to a destination, considering weight limits.
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
            float requestedAmount)
        {
            if (srcQty <= TOLERANCE || requestedAmount <= 0) return 0f;

            float limitSrc = (srcQty < requestedAmount) ? srcQty : requestedAmount;

            float slotSpace = dstMaxQty - dstQty;
            if (slotSpace < 0f) slotSpace = 0f;

            float weightLimit = float.MaxValue;
            if (unitWeight > TOLERANCE)
            {
                float weightSpace = dstMaxWeight - dstCurWeight;
                if (weightSpace < 0f) weightSpace = 0f;
                weightLimit = weightSpace / unitWeight;
            }

            float actualMove = limitSrc;
            if (slotSpace < actualMove) actualMove = slotSpace;
            if (weightLimit < actualMove) actualMove = weightLimit;

            if (actualMove > TOLERANCE)
            {
                srcQty -= actualMove;
                dstQty += actualMove;
                dstCurWeight += (actualMove * unitWeight);

                if (srcQty < TOLERANCE) srcQty = 0f;
                if (dstMaxQty - dstQty < TOLERANCE) dstQty = dstMaxQty;
                if (dstMaxWeight - dstCurWeight < TOLERANCE) dstCurWeight = dstMaxWeight;

                return actualMove;
            }

            return 0f;
        }
    }
}
