using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Attempts to remove an exact amount of items from the inventory.
        /// </summary>
        /// <param name="current">The reference to the current quantity variable.</param>
        /// <param name="amount">The amount to remove.</param>
        /// <returns><c>true</c> if the amount was removed successfully; otherwise, <c>false</c> if there were insufficient items.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveExact(ref float current, float amount)
        {
            if (amount <= TOLERANCE) return true;
            if (current < amount - TOLERANCE) return false;

            current -= amount;
            if (current < TOLERANCE) current = 0f;
            return true;
        }
    }
}
