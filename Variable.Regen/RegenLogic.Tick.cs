using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Regen
{
    /// <summary>
    /// Provides static methods for regeneration and decay calculations.
    /// </summary>
    /// <remarks>
    /// <para>This class contains the core logic for updating values based on rates.</para>
    /// <para>All methods are stateless and work with ref parameters for zero allocation.</para>
    /// </remarks>
    public static partial class RegenLogic
    {
        /// <summary>
        /// Updates a value based on a rate and time delta.
        /// Handles both regeneration (positive rate) and decay (negative rate).
        /// Clamps the result between 0 and max.
        /// </summary>
        /// <param name="current">The current value to modify.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="rate">The rate of change per second.</param>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Tick(ref float current, float max, float rate, float deltaTime)
        {
            if (rate == 0f || deltaTime == 0f) return;

            current += rate * deltaTime;

            if (current > max) current = max;
            else if (current < 0f) current = 0f;
        }

        /// <summary>
        /// Updates a BoundedFloat based on a rate and time delta.
        /// Handles both regeneration (positive rate) and decay (negative rate).
        /// </summary>
        /// <param name="bounded">The bounded float to modify.</param>
        /// <param name="rate">The rate of change per second.</param>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Tick(ref BoundedFloat bounded, float rate, float deltaTime)
        {
            if (rate == 0f || deltaTime == 0f) return;

            bounded.Current += rate * deltaTime;
            bounded.Normalize();
        }
    }
}