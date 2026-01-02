namespace Variable.Core;

/// <summary>
///     Represents a time-based value that can be ticked and checked for completion.
///     This interface unifies Timer (count-up) and Cooldown (count-down) behaviors.
/// </summary>
/// <remarks>
///     <para>
///         Implementations of this interface represent temporal states that progress over time
///         and eventually reach a completion state (Timer reaches Duration, Cooldown reaches 0).
///     </para>
///     <para>
///         For Timer: IsComplete returns true when Current >= Duration (counting UP).
///     </para>
///     <para>
///         For Cooldown: IsComplete returns true when Current &lt;= 0 (counting DOWN).
///     </para>
/// </remarks>
public interface ICompletable
{
    /// <summary>
    ///     Gets whether the temporal state has reached completion.
    /// </summary>
    /// <returns>
    ///     <c>true</c> if the timer has completed or the cooldown is ready; otherwise, <c>false</c>.
    /// </returns>
    bool IsComplete { get; }
}
