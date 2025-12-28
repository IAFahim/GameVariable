using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Removes as much of the specified amount as possible from the inventory.
        /// </summary>
        /// <param name="current">The reference to the current quantity variable.</param>
        /// <param name="amount">The amount to attempt to remove.</param>
        /// <returns>The actual amount removed from the inventory.</returns>
        public static float RemovePartial(ref float current, float amount)
        {
            if (amount <= TOLERANCE || current <= TOLERANCE) return 0f;

            float toRemove = (current < amount) ? current : amount;

            current -= toRemove;
            if (current < TOLERANCE) current = 0f;

            return toRemove;
        }
    }
}
