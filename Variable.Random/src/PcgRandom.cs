namespace Variable.Random;

/// <summary>
///     A high-performance, deterministic pseudo-random number generator (PRNG).
///     Implements the PCG-XSH-RR 32/64 algorithm.
/// </summary>
/// <remarks>
///     This struct is 16 bytes (128 bits) in size.
///     It is fully deterministic, meaning the same seed and sequence will always produce the same output.
///     Ideal for procedural generation, simulation, and networking.
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct PcgRandom : IEquatable<PcgRandom>
{
    private ulong _state;
    private readonly ulong _inc;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PcgRandom"/> struct.
    /// </summary>
    /// <param name="seed">The initial seed value.</param>
    /// <param name="sequence">The sequence ID (stream) to use. Different sequences produce independent streams of random numbers.</param>
    public PcgRandom(ulong seed, ulong sequence = 0)
    {
        _state = 0;
        _inc = (sequence << 1) | 1u;
        Next();
        _state += seed;
        Next();
    }

    /// <summary>
    ///     Initializes a new instance with a seed based on the current time ticks.
    /// </summary>
    public PcgRandom() : this((ulong)DateTime.Now.Ticks)
    {
    }

    /// <summary>
    ///     Generates the next random unsigned 32-bit integer.
    /// </summary>
    /// <returns>A random <see cref="uint"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Next()
    {
        ulong oldState = _state;
        // LCG step
        _state = oldState * 6364136223846793005ul + _inc;

        // XSH-RR step
        uint xorshifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
        int rot = (int)(oldState >> 59);
        return (xorshifted >> rot) | (xorshifted << ((-rot) & 31));
    }

    /// <summary>
    ///     Generates the next random float in the range [0, 1).
    /// </summary>
    /// <returns>A random <see cref="float"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float NextFloat()
    {
        // 2^-32
        return Next() * 2.3283064365386963E-10f;
    }

    /// <summary>
    ///     Generates a random integer in the range [min, max).
    /// </summary>
    /// <param name="min">The inclusive lower bound.</param>
    /// <param name="max">The exclusive upper bound.</param>
    /// <returns>A random integer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int NextInt(int min, int max)
    {
        if (min >= max)
        {
             return min;
        }

        uint range = (uint)(max - min);
        return min + (int)(Next(range));
    }

    /// <summary>
    ///     Generates a random unsigned integer in the range [0, max).
    /// </summary>
    /// <param name="max">The exclusive upper bound.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Next(uint max)
    {
        if (max == 0) return 0;

        // Lemire's method for unbiased bounded random numbers
        // https://arxiv.org/abs/1805.10941
        ulong x = Next();
        ulong m = x * (ulong)max;
        uint l = (uint)m;

        if (l < max)
        {
            uint t = (0u - max) % max;
            while (l < t)
            {
                x = Next();
                m = x * (ulong)max;
                l = (uint)m;
            }
        }
        return (uint)(m >> 32);
    }

    /// <summary>
    ///     Generates a random boolean value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool NextBool()
    {
        return (Next() & 1) == 0;
    }

    /// <summary>
    ///     Returns a random float in the range [min, max).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float NextFloat(float min, float max)
    {
        return min + NextFloat() * (max - min);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is PcgRandom other && Equals(other);
    }

    /// <inheritdoc/>
    public bool Equals(PcgRandom other)
    {
        return _state == other._state && _inc == other._inc;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(_state, _inc);
    }

    /// <summary>
    ///     Determines whether two <see cref="PcgRandom"/> instances are equal.
    /// </summary>
    public static bool operator ==(PcgRandom left, PcgRandom right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="PcgRandom"/> instances are not equal.
    /// </summary>
    public static bool operator !=(PcgRandom left, PcgRandom right) => !left.Equals(right);
}
