using System;
using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Reservoir;

/// <summary>
/// Represents a reservoir system with a limited volume (BoundedFloat) and a reserve (float).
/// Generic concept for Energy, Fuel, etc.
/// </summary>
[Serializable]
public struct ReservoirFloat
{
    public BoundedFloat Volume;
    public float Reserve;

    public ReservoirFloat(float capacity, float currentVolume, float reserve)
    {
        Volume = new BoundedFloat(capacity, currentVolume);
        Reserve = reserve;
    }

    public ReservoirFloat(BoundedFloat volume, float reserve)
    {
        Volume = volume;
        Reserve = reserve;
    }

    /// <summary>
    /// Refills the volume from the reserve.
    /// </summary>
    /// <returns>The amount refilled.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Refill()
    {
        return ReservoirLogic.Refill(ref Volume, ref Reserve);
    }

    public static implicit operator float(ReservoirFloat res) => res.Volume.Current;
}
