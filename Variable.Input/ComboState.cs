namespace Variable.Input;

/// <summary>
///     Runtime state for combo system traversal.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct ComboState
{
    /// <summary>
    ///     The index of the current active node in the graph.
    /// </summary>
    public int CurrentNodeIndex;

    /// <summary>
    ///     Indicates whether the current action is still executing, preventing transitions.
    /// </summary>
    public bool IsActionBusy;
}