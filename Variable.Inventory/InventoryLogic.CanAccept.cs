using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Checks if the inventory can accept a specific amount of items without exceeding its capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanAccept(float current, float max, float amountToAdd) 
            => (current + amountToAdd) <= max + TOLERANCE;

        /// <summary>
        /// Checks if the inventory can accept a specific amount of items without exceeding its capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanAccept(double current, double max, double amountToAdd) 
            => (current + amountToAdd) <= max + TOLERANCE_DOUBLE;

        /// <summary>
        /// Checks if the inventory can accept a specific amount of items without exceeding its capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanAccept(int current, int max, int amountToAdd) 
            => (current + amountToAdd) <= max;

        /// <summary>
        /// Checks if the inventory can accept a specific amount of items without exceeding its capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanAccept(long current, long max, long amountToAdd) 
            => (current + amountToAdd) <= max;

        /// <summary>
        /// Checks if the inventory can accept a specific amount of items without exceeding its capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanAccept(byte current, byte max, byte amountToAdd) 
            => (current + amountToAdd) <= max;
    }
}
