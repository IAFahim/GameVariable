using System.Runtime.CompilerServices;
using Variable.Core;
using Variable.Timer;

namespace Variable.Tween;

/// <summary>
///     Extension methods for <see cref="TweenFloat" />.
/// </summary>
public static class TweenExtensions
{
    /// <summary>
    ///     Advances the tween by delta time and updates the current value.
    /// </summary>
    /// <param name="tween">The tween to advance.</param>
    /// <param name="dt">The time elapsed since the last tick.</param>
    /// <returns>True if the tween is complete; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Tick(ref this TweenFloat tween, float dt)
    {
        // Handle zero/small duration - snap to end
        if (tween.Timer.Duration < MathConstants.Tolerance)
        {
            tween.Current = tween.End;
            return true;
        }

        // 1. Tick the timer
        TimerExtensions.Tick(ref tween.Timer, dt);

        // 2. Calculate ratio
        // Using TimerExtensions.GetRatio to handle safe division
        var ratio = (float)TimerExtensions.GetRatio(tween.Timer);

        // 3. Evaluate logic
        TweenLogic.Evaluate(tween.Start, tween.End, ratio, tween.Easing, out tween.Current);

        return TimerExtensions.IsFull(tween.Timer);
    }

    /// <summary>
    ///     Resets the tween to its initial state.
    /// </summary>
    /// <param name="tween">The tween to reset.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Reset(ref this TweenFloat tween)
    {
        TimerExtensions.Reset(ref tween.Timer);
        tween.Current = tween.Start;
    }

    /// <summary>
    ///     Determines whether the tween has completed.
    /// </summary>
    /// <param name="tween">The tween to check.</param>
    /// <returns>True if the tween is complete; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsComplete(this TweenFloat tween)
    {
        return TimerExtensions.IsFull(tween.Timer);
    }
}
