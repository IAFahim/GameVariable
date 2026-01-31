namespace Variable.Random;

/// <summary>
///     Represents the state of the Xoshiro128** pseudo-random number generator.
///     <para>
///         This is a Pure Data struct (Layer A). It contains no logic, only the 128-bit internal state.
///         Pass this by ref to <see cref="RandomLogic"/> methods to generate numbers.
///     </para>
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct RandomState
{
    /// <summary>Internal state component 0.</summary>
    public uint S0;

    /// <summary>Internal state component 1.</summary>
    public uint S1;

    /// <summary>Internal state component 2.</summary>
    public uint S2;

    /// <summary>Internal state component 3.</summary>
    public uint S3;
}
