namespace Variable.Input;

/// <summary>
///     Input ID constants and utilities. Users should extend this with their own IDs.
/// </summary>
/// <remarks>
///     <para>Define your game-specific inputs by creating extension constants or a derived class:</para>
///     <example>
///         <code>
/// // Option 1: Extend with constants
/// public static class MyInputIds
/// {
///     public const int Jump = 1;
///     public const int Attack = 2;
///     public const int Dash = 3;
/// }
/// 
/// // Option 2: Use enum (if you don't need extension)
/// public enum GameInput
/// {
///     None = InputId.None,
///     Jump = 1,
///     Attack = 2,
///     Dash = 3
/// }
/// </code>
///     </example>
///     <para>
///         IDs are stored as <see cref="int" /> for flexibility across input systems
///         (keyboard, gamepad, touch, network packets, etc.).
///     </para>
/// </remarks>
public static class InputId
{
    /// <summary>
    ///     Represents no input or an invalid input state.
    /// </summary>
    public const int None = 0;

    /// <summary>
    ///     Suggested starting value for user-defined inputs to avoid conflicts with system values.
    /// </summary>
    public const int UserDefinedStart = 100;
}