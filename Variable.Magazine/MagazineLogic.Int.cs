using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Magazine;

public static partial class MagazineLogic
{
    /// <summary>
    /// Reloads the clip from the reserve.
    /// Transfers as much as possible from reserve to current, up to capacity.
    /// </summary>
    /// <param name="current">Reference to current ammo in clip.</param>
    /// <param name="capacity">Maximum ammo in clip.</param>
    /// <param name="reserve">Reference to reserve ammo.</param>
    /// <returns>The amount reloaded.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Reload(ref int current, int capacity, ref int reserve)
    {
        int missing = capacity - current;
        if (missing <= 0 || reserve <= 0) return 0;

        int toReload = reserve < missing ? reserve : missing;
        current += toReload;
        reserve -= toReload;

        return toReload;
    }

    /// <summary>
    /// Reloads the BoundedInt clip from the reserve.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Reload(ref BoundedInt clip, ref int reserve)
    {
        return Reload(ref clip.Current, clip.Max, ref reserve);
    }
}
