namespace Variable.Random;

/// <summary>
///     Pure data struct holding the state for the random number generator.
///     Uses Xoshiro128** algorithm state (4 x 32-bit integers).
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct RandomState
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
}
