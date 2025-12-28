using System;
using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Magazine;

/// <summary>
/// Represents a magazine with a clip (BoundedFloat) and a reserve (float).
/// Useful for battery/energy systems.
/// </summary>
[Serializable]
public struct MagazineFloat
{
    public BoundedFloat Clip;
    public float Reserve;

    public MagazineFloat(float capacity, float currentInClip, float reserve)
    {
        Clip = new BoundedFloat(capacity, currentInClip);
        Reserve = reserve;
    }

    public MagazineFloat(BoundedFloat clip, float reserve)
    {
        Clip = clip;
        Reserve = reserve;
    }

    /// <summary>
    /// Reloads the clip from the reserve.
    /// </summary>
    /// <returns>The amount reloaded.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Reload()
    {
        return MagazineLogic.Reload(ref Clip, ref Reserve);
    }

    public static implicit operator float(MagazineFloat mag) => mag.Clip.Current;
}
