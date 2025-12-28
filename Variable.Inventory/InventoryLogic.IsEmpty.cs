using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Determines whether the inventory is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(float current) => current <= TOLERANCE;

        /// <summary>
        /// Determines whether the inventory is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(double current) => current <= TOLERANCE_DOUBLE;

        /// <summary>
        /// Determines whether the inventory is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(int current) => current <= 0;

        /// <summary>
        /// Determines whether the inventory is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(long current) => current <= 0;

        /// <summary>
        /// Determines whether the inventory is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(byte current) => current <= 0;
    }
}
