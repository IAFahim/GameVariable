using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Clears the inventory, setting the current quantity to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(ref float current) => current = 0f;

        /// <summary>
        /// Clears the inventory, setting the current quantity to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(ref double current) => current = 0d;

        /// <summary>
        /// Clears the inventory, setting the current quantity to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(ref int current) => current = 0;

        /// <summary>
        /// Clears the inventory, setting the current quantity to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(ref long current) => current = 0;

        /// <summary>
        /// Clears the inventory, setting the current quantity to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(ref byte current) => current = 0;
    }
}
