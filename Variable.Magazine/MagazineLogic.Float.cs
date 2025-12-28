using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Magazine;

public static partial class MagazineLogic
{
    /// <summary>
    /// Reloads the clip from the reserve (Float version).
    /// Transfers as much as possible from reserve to current, up to capacity.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Reload(ref float current, float capacity, ref float reserve)
    {
        float missing = capacity - current;
        if (missing <= 0.001f || reserve <= 0.001f) return 0f;

        float toReload = reserve < missing ? reserve : missing;
        current += toReload;
        reserve -= toReload;

        // Clamp to handle precision errors
        if (capacity - current < 0.001f) current = capacity;
        if (reserve < 0.001f) reserve = 0f;

        return toReload;
    }

    /// <summary>
    /// Reloads the BoundedFloat clip from the reserve.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Reload(ref BoundedFloat clip, ref float reserve)
    {
        return Reload(ref clip.Current, clip.Max, ref reserve);
    }
}
