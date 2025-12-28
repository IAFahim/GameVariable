using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Reservoir;

/// <summary>
///     Represents a reservoir system with a limited volume (BoundedInt) and a reserve (int).
///     Generic concept for Ammo, Batteries, Mana Pools, etc.
/// </summary>
[Serializable]
public struct ReservoirInt
{
    public BoundedInt Volume;
    public int Reserve;

    public ReservoirInt(int capacity, int currentVolume, int reserve)
    {
        Volume = new BoundedInt(capacity, currentVolume);
        Reserve = reserve;
    }

    public ReservoirInt(BoundedInt volume, int reserve)
    {
        Volume = volume;
        Reserve = reserve;
    }

    /// <summary>
    ///     Refills the volume from the reserve.
    /// </summary>
    /// <returns>The amount refilled.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Refill()
    {
        return ReservoirLogic.Refill(ref Volume, ref Reserve);
    }

    public static implicit operator int(ReservoirInt res)
    {
        return res.Volume.Current;
    }
}