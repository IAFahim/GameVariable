using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    /// <summary>
    /// Provides static logic for inventory management, including capacity checks, 
    /// adding/removing items, and transferring items between containers.
    /// </summary>
    public static class InventoryLogic
    {
        /// <summary>
        /// A small tolerance value used for floating-point comparisons to handle precision errors.
        /// </summary>
        public const float TOLERANCE = 0.001f;

        /// <summary>
        /// Determines whether the inventory is full based on the current quantity and maximum capacity.
        /// </summary>
        /// <param name="current">The current quantity of items in the inventory.</param>
        /// <param name="max">The maximum capacity of the inventory.</param>
        /// <returns><c>true</c> if the inventory is full; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFull(float current, float max) => current >= max - TOLERANCE;

        /// <summary>
        /// Determines whether the inventory is empty.
        /// </summary>
        /// <param name="current">The current quantity of items in the inventory.</param>
        /// <returns><c>true</c> if the inventory is empty; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(float current) => current <= TOLERANCE;

        /// <summary>
        /// Checks if the inventory has enough items to satisfy a requirement.
        /// </summary>
        /// <param name="current">The current quantity of items in the inventory.</param>
        /// <param name="required">The required quantity.</param>
        /// <returns><c>true</c> if the inventory has enough items; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasEnough(float current, float required) => current >= required - TOLERANCE;

        /// <summary>
        /// Checks if the inventory can accept a specific amount of items without exceeding its capacity.
        /// </summary>
        /// <param name="current">The current quantity of items in the inventory.</param>
        /// <param name="max">The maximum capacity of the inventory.</param>
        /// <param name="amountToAdd">The amount of items to add.</param>
        /// <returns><c>true</c> if the items can be accepted; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanAccept(float current, float max, float amountToAdd) 
            => (current + amountToAdd) <= max + TOLERANCE;

        /// <summary>
        /// Calculates the remaining space in the inventory.
        /// </summary>
        /// <param name="current">The current quantity of items in the inventory.</param>
        /// <param name="max">The maximum capacity of the inventory.</param>
        /// <returns>The remaining space available. Returns 0 if the inventory is full or overfilled.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetRemainingSpace(float current, float max) 
            => (current >= max) ? 0f : max - current;
        
        
        /// <summary>
        /// Calculates the maximum amount of an item that can be accepted, considering both quantity and weight limits.
        /// </summary>
        /// <param name="currentQty">The current quantity of items.</param>
        /// <param name="maxQty">The maximum quantity capacity.</param>
        /// <param name="currentWeight">The current total weight.</param>
        /// <param name="maxWeight">The maximum weight capacity.</param>
        /// <param name="unitWeight">The weight of a single unit of the item.</param>
        /// <returns>The maximum amount that can be accepted.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMaxAcceptable(
            float currentQty, 
            float maxQty, 
            float currentWeight, 
            float maxWeight, 
            float unitWeight)
        {
            float spaceByQty = (currentQty >= maxQty) ? 0f : maxQty - currentQty;

            if (unitWeight <= TOLERANCE) return spaceByQty;

            float remainingWeight = (currentWeight >= maxWeight) ? 0f : maxWeight - currentWeight;
            float spaceByWeight = remainingWeight / unitWeight;

            return (spaceByQty < spaceByWeight) ? spaceByQty : spaceByWeight;
        }

        /// <summary>
        /// Sets the inventory quantity to a specific value, clamping it between 0 and the maximum capacity.
        /// </summary>
        /// <param name="current">The reference to the current quantity variable.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="max">The maximum capacity.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set(ref float current, float value, float max)
        {
            if (value < 0f) value = 0f;
            if (value > max) value = max;

            if (value < TOLERANCE) value = 0f;
            if (max - value < TOLERANCE) value = max;

            current = value;
        }

        /// <summary>
        /// Clears the inventory, setting the current quantity to 0.
        /// </summary>
        /// <param name="current">The reference to the current quantity variable.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(ref float current) => current = 0f;

        /// <summary>
        /// Attempts to add an exact amount of items to the inventory.
        /// </summary>
        /// <param name="current">The reference to the current quantity variable.</param>
        /// <param name="amount">The amount to add.</param>
        /// <param name="max">The maximum capacity.</param>
        /// <returns><c>true</c> if the amount was added successfully; otherwise, <c>false</c> if it would exceed capacity.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAddExact(ref float current, float amount, float max)
        {
            if (amount <= TOLERANCE) return true;
            if (current + amount > max + TOLERANCE) return false;

            current += amount;
            if (max - current < TOLERANCE) current = max; 
            return true;
        }
        
        
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

        
        /// <summary>
        /// Attempts to transfer an exact amount of items from a source to a destination, considering weight limits.
        /// </summary>
        /// <param name="srcQty">The reference to the source quantity variable.</param>
        /// <param name="dstQty">The reference to the destination quantity variable.</param>
        /// <param name="dstCurWeight">The reference to the destination current weight variable.</param>
        /// <param name="dstMaxWeight">The maximum weight capacity of the destination.</param>
        /// <param name="dstMaxQty">The maximum quantity capacity of the destination.</param>
        /// <param name="itemUnitWeight">The weight of a single unit of the item.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <returns><c>true</c> if the transfer was successful; otherwise, <c>false</c> if constraints are not met.</returns>
        public static bool TryTransferExactWithWeight(
            ref float srcQty, 
            ref float dstQty, 
            ref float dstCurWeight, 
            float dstMaxWeight, 
            float dstMaxQty, float itemUnitWeight, 
            float amount)
        {
            if (amount <= TOLERANCE) return true;

            if (srcQty < amount - TOLERANCE) return false;

            if (dstQty + amount > dstMaxQty + TOLERANCE) return false;

            float weightToAdd = amount * itemUnitWeight;
            if (dstCurWeight + weightToAdd > dstMaxWeight + TOLERANCE) return false;

            srcQty -= amount;
            dstQty += amount;
            dstCurWeight += weightToAdd;

            if (srcQty < TOLERANCE) srcQty = 0f;
            if (dstMaxQty - dstQty < TOLERANCE) dstQty = dstMaxQty;
            if (dstMaxWeight - dstCurWeight < TOLERANCE) dstCurWeight = dstMaxWeight;

            return true;
        }

        /// <summary>
        /// Transfers as much of the requested amount as possible from a source to a destination.
        /// </summary>
        /// <param name="srcQty">The reference to the source quantity variable.</param>
        /// <param name="dstQty">The reference to the destination quantity variable.</param>
        /// <param name="dstMax">The maximum capacity of the destination.</param>
        /// <param name="requestedAmount">The amount requested to transfer.</param>
        /// <returns>The actual amount transferred.</returns>
        public static float TransferPartial(ref float srcQty, ref float dstQty, float dstMax, float requestedAmount)
        {
            if (srcQty <= TOLERANCE || requestedAmount <= 0) return 0f;

            float available = (srcQty < requestedAmount) ? srcQty : requestedAmount;

            float space = dstMax - dstQty;
            if (space < 0f) space = 0f;
            
            float actualMove = (available < space) ? available : space;

            if (actualMove > TOLERANCE)
            {
                srcQty -= actualMove;
                dstQty += actualMove;

                if (srcQty < TOLERANCE) srcQty = 0f;
                if (dstMax - dstQty < TOLERANCE) dstQty = dstMax;
                
                return actualMove;
            }
            return 0f;
        }

        
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