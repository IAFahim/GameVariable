using System;
using System.Runtime.CompilerServices;

namespace Variable.Inventory
{
    public static partial class InventoryLogic
    {
        /// <summary>
        /// Calculates the remaining space in the inventory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetRemainingSpace(float current, float max) 
            => (current >= max) ? 0f : max - current;

        /// <summary>
        /// Calculates the remaining space in the inventory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetRemainingSpace(double current, double max) 
            => (current >= max) ? 0d : max - current;

        /// <summary>
        /// Calculates the remaining space in the inventory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetRemainingSpace(int current, int max) 
            => (current >= max) ? 0 : max - current;

        /// <summary>
        /// Calculates the remaining space in the inventory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetRemainingSpace(long current, long max) 
            => (current >= max) ? 0 : max - current;

        /// <summary>
        /// Calculates the remaining space in the inventory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetRemainingSpace(byte current, byte max) 
            => (byte)((current >= max) ? 0 : max - current);
    }
}
