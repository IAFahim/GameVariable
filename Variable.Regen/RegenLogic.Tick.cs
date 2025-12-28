using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Regen;

public static partial class RegenLogic
{
    /// <summary>
    /// Updates a value based on a rate and time delta.
    /// Handles both regeneration (positive rate) and decay (negative rate).
    /// Clamps the result between 0 and max.
    /// </summary>
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Tick(ref BoundedFloat bounded, float rate, float deltaTime)
    {
        if (rate == 0f || deltaTime == 0f) return;

        bounded.Current += rate * deltaTime;
        bounded.Normalize();
    }
}
