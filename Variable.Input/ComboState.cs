namespace Variable.Input;

/// <summary>
///     Runtime state for combo system traversal.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ComboState
{
    public int CurrentNodeIndex;
    public bool IsActionBusy;
}
