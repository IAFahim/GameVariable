namespace Variable.Random;

/// <summary>
///     A zero-allocation, deterministic random state using Xoshiro128**.
/// </summary>
/// <remarks>
///     <para>
///         This struct holds the 128-bit state required for the Xoshiro128** algorithm.
///         It is blittable, serializable, and fits in 16 bytes.
///     </para>
///     <para>
///         Use <see cref="RandomExtensions" /> to generate values.
///     </para>
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct RandomState : IEquatable<RandomState>
{
    /// <summary>
    ///     State part 0.
    /// </summary>
    public uint S0;

    /// <summary>
    ///     State part 1.
    /// </summary>
    public uint S1;

    /// <summary>
    ///     State part 2.
    /// </summary>
    public uint S2;

    /// <summary>
    ///     State part 3.
    /// </summary>
    public uint S3;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RandomState" /> struct with a single seed.
    ///     Uses SplitMix64 logic to expand the 32-bit seed into 128 bits of state.
    /// </summary>
    /// <param name="seed">The initial seed.</param>
    public RandomState(uint seed)
    {
        // Seeding using a simple SplitMix-like expansion to ensure non-zero state.
        // We need 4 non-zero values.

        // Simple 32-bit mixing to generate initial state from one seed.
        // Based on SplitMix32/MurmurHash3 mixer style.

        uint z = seed + 0x9E3779B9;

        // S0
        z = (z ^ (z >> 16)) * 0x85ebca6b;
        z = (z ^ (z >> 13)) * 0xc2b2ae35;
        S0 = z ^ (z >> 16);

        // S1
        z += 0x9E3779B9;
        z = (z ^ (z >> 16)) * 0x85ebca6b;
        z = (z ^ (z >> 13)) * 0xc2b2ae35;
        S1 = z ^ (z >> 16);

        // S2
        z += 0x9E3779B9;
        z = (z ^ (z >> 16)) * 0x85ebca6b;
        z = (z ^ (z >> 13)) * 0xc2b2ae35;
        S2 = z ^ (z >> 16);

        // S3
        z += 0x9E3779B9;
        z = (z ^ (z >> 16)) * 0x85ebca6b;
        z = (z ^ (z >> 13)) * 0xc2b2ae35;
        S3 = z ^ (z >> 16);

        // Ensure state is not all zero (Xoshiro fails if all are zero)
        if ((S0 | S1 | S2 | S3) == 0)
        {
            S0 = 1;
            S1 = 0x9E3779B9;
            S2 = 0x85ebca6b;
            S3 = 0xc2b2ae35;
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RandomState" /> struct with explicit state.
    /// </summary>
    public RandomState(uint s0, uint s1, uint s2, uint s3)
    {
        S0 = s0;
        S1 = s1;
        S2 = s2;
        S3 = s3;

        if ((S0 | S1 | S2 | S3) == 0)
        {
            S0 = 1;
        }
    }

    /// <inheritdoc />
    public bool Equals(RandomState other)
    {
        return S0 == other.S0 && S1 == other.S1 && S2 == other.S2 && S3 == other.S3;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is RandomState other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)S0;
            hashCode = (hashCode * 397) ^ (int)S1;
            hashCode = (hashCode * 397) ^ (int)S2;
            hashCode = (hashCode * 397) ^ (int)S3;
            return hashCode;
        }
    }
}
