using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Adds as much of the specified amount as possible to the inventory, up to the maximum capacity.
        /// </summary>
        /// <param name="current">The reference to the current quantity variable.</param>
        /// <param name="amount">The amount to attempt to add.</param>
        /// <param name="max">The maximum capacity.</param>
        /// <param name="overflow">The amount that could not be added.</param>
        /// <returns>The actual amount added to the inventory.</returns>
        public static float AddPartial(ref float current, float amount, float max, out float overflow)
        {
            if (amount <= TOLERANCE) { overflow = 0f; return 0f; }

            float space = max - current;
            if (space < 0f) space = 0f;

            float toAdd = (amount < space) ? amount : space;
            overflow = amount - toAdd;
            
            current += toAdd;
            if (max - current < TOLERANCE) current = max;

            return toAdd;
        }
    }
}
