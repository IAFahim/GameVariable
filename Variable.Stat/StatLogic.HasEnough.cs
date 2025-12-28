namespace Variable.Stat;

public static partial class StatLogic
{
    /// <summary>
    ///     Checks if the current value is greater than or equal to the required amount.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEnough(float current, float amount)
    {
        return current >= amount - MathConstants.Tolerance;
    }
}