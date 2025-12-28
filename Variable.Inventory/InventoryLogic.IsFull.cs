using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Determines whether the inventory is full based on the current quantity and maximum capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFull(float current, float max) => current >= max - TOLERANCE;

        /// <summary>
        /// Determines whether the inventory is full based on the current quantity and maximum capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFull(double current, double max) => current >= max - TOLERANCE_DOUBLE;

        /// <summary>
        /// Determines whether the inventory is full based on the current quantity and maximum capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFull(int current, int max) => current >= max;

        /// <summary>
        /// Determines whether the inventory is full based on the current quantity and maximum capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFull(long current, long max) => current >= max;

        /// <summary>
        /// Determines whether the inventory is full based on the current quantity and maximum capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFull(byte current, byte max) => current >= max;
    }
}
