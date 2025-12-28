using System;
using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Magazine;

/// <summary>
/// Represents a magazine with a clip (BoundedInt) and a reserve (int).
/// Useful for ammo systems.
/// </summary>
[Serializable]
public struct MagazineInt
{
    public BoundedInt Clip;
    public int Reserve;

    public MagazineInt(int capacity, int currentInClip, int reserve)
    {
        Clip = new BoundedInt(capacity, currentInClip);
        Reserve = reserve;
    }

    public MagazineInt(BoundedInt clip, int reserve)
    {
        Clip = clip;
        Reserve = reserve;
    }

    /// <summary>
    /// Reloads the clip from the reserve.
    /// </summary>
    /// <returns>The amount reloaded.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Reload()
    {
        return MagazineLogic.Reload(ref Clip, ref Reserve);
    }

    public static implicit operator int(MagazineInt mag) => mag.Clip.Current;
}
