namespace Variable.Input;

/// <summary>
///     Configuration constants for input buffer systems.
/// </summary>
public static class InputBufferConfig
{
    /// <summary>
    ///     The default capacity for standard input ring buffers.
    ///     Sufficient for most action games (2-6 input combos).
    /// </summary>
    public const int StandardCapacity = 8;

    /// <summary>
    ///     The extended capacity for complex combo systems.
    ///     Use for fighting games with 10+ hit combos.
    /// </summary>
    public const int ExtendedCapacity = 16;
}