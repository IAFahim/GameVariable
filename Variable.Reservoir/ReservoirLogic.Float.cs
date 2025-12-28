using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Reservoir;

public static partial class ReservoirLogic
{
    /// <summary>
    ///     Refills the current volume from the reserve (Float version).
    ///     Transfers as much as possible from reserve to current, up to capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Refill(ref float current, float capacity, ref float reserve)
    {
        var missing = capacity - current;
        if (missing <= 0.001f || reserve <= 0.001f) return 0f;

        var toRefill = reserve < missing ? reserve : missing;
        current += toRefill;
        reserve -= toRefill;

        // Clamp to handle precision errors
        if (capacity - current < 0.001f) current = capacity;
        if (reserve < 0.001f) reserve = 0f;

        return toRefill;
    }

    /// <summary>
    ///     Refills the BoundedFloat volume from the reserve.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Refill(ref BoundedFloat volume, ref float reserve)
    {
        return Refill(ref volume.Current, volume.Max, ref reserve);
    }
}