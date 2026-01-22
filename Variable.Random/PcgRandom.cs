namespace Variable.Random;

/// <summary>
///     A deterministic, zero-allocation random number generator using the PCG-XSH-RR algorithm.
///     (Permuted Congruential Generator - Xorshift High (Random Rotation)).
/// </summary>
/// <remarks>
///     <para>
///         This struct holds the state of the generator (128 bits: 64-bit state + 64-bit stream/increment).
///         It is blittable, serializable, and suitable for Unity ECS/Jobs.
///     </para>
///     <para>
///         PCG is much statistically better than System.Random and faster.
///         It is also fully deterministic: the same seed always produces the same sequence.
///     </para>
/// </remarks>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct PcgRandom : IEquatable<PcgRandom>
{
    /// <summary>The internal state of the generator.</summary>
    public ulong State;

    /// <summary>The increment/stream selector. Must be odd.</summary>
    public ulong Increment;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PcgRandom" /> struct.
    /// </summary>
    /// <param name="seed">The seed for the generator.</param>
    /// <param name="sequence">
    ///     The sequence/stream identifier. Different sequences produce independent streams of random numbers.
    ///     Default is 0.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PcgRandom(ulong seed, ulong sequence = 0)
    {
        RandomLogic.Initialize(seed, sequence, out State, out Increment);
    }

    /// <summary>
    ///     Creates a new <see cref="PcgRandom" /> initialized with the current time ticks.
    /// </summary>
    /// <param name="sequence">The sequence/stream identifier. Default is 0.</param>
    public static PcgRandom New(ulong sequence = 0)
    {
        return new PcgRandom((ulong)DateTime.Now.Ticks, sequence);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is PcgRandom other && Equals(other);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(PcgRandom other)
    {
        return State == other.State && Increment == other.Increment;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(State, Increment);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"PcgRandom(State={State}, Inc={Increment})";
    }

    /// <summary>
    ///     Determines whether two PcgRandom instances are equal.
    /// </summary>
    public static bool operator ==(PcgRandom left, PcgRandom right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Determines whether two PcgRandom instances are not equal.
    /// </summary>
    public static bool operator !=(PcgRandom left, PcgRandom right)
    {
        return !left.Equals(right);
    }
}
