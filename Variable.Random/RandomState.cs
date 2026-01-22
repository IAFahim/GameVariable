namespace Variable.Random;

/// <summary>
///     The state for the Xoshiro128** pseudo-random number generator.
///     A deterministic, zero-allocation RNG suitable for high-performance games.
/// </summary>
/// <remarks>
///     <para>
///         Uses the Xoshiro128** algorithm (xor-shift-rotate) designed by David Blackman and Sebastiano Vigna.
///         It is fast, has a period of 2^128 - 1, and passes BigCrush.
///     </para>
///     <para>
///         This struct is 16 bytes (4 x 32-bit uints).
///     </para>
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct RandomState : IEquatable<RandomState>
{
    /// <summary>Internal state component S0.</summary>
    public uint S0;

    /// <summary>Internal state component S1.</summary>
    public uint S1;

    /// <summary>Internal state component S2.</summary>
    public uint S2;

    /// <summary>Internal state component S3.</summary>
    public uint S3;

    /// <summary>
    ///     Initializes the RNG state with a seed.
    ///     Uses SplitMix64 to generate the initial state from the 32-bit seed.
    /// </summary>
    /// <param name="seed">The initialization seed.</param>
    public RandomState(uint seed)
    {
        RandomLogic.Initialize(seed, out this);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is RandomState other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(RandomState other)
    {
        return S0 == other.S0 && S1 == other.S1 && S2 == other.S2 && S3 == other.S3;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(S0, S1, S2, S3);
    }

    /// <summary>Determines whether two RandomState instances are equal.</summary>
    public static bool operator ==(RandomState left, RandomState right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two RandomState instances are not equal.</summary>
    public static bool operator !=(RandomState left, RandomState right)
    {
        return !left.Equals(right);
    }
}
