using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Reservoir;

public static partial class ReservoirLogic
{
    /// <summary>
    ///     Refills the current volume from the reserve.
    ///     Transfers as much as possible from reserve to current, up to capacity.
    /// </summary>
    /// <param name="current">Reference to current volume.</param>
    /// <param name="capacity">Maximum volume capacity.</param>
    /// <param name="reserve">Reference to reserve amount.</param>
    /// <returns>The amount refilled.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Refill(ref int current, int capacity, ref int reserve)
    {
        var missing = capacity - current;
        if (missing <= 0 || reserve <= 0) return 0;

        var toRefill = reserve < missing ? reserve : missing;
        current += toRefill;
        reserve -= toRefill;

        return toRefill;
    }

    /// <summary>
    ///     Refills the BoundedInt volume from the reserve.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Refill(ref BoundedInt volume, ref int reserve)
    {
        return Refill(ref volume.Current, volume.Max, ref reserve);
    }
}