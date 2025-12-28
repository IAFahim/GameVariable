using System.Runtime.CompilerServices;
using Variable.Bounded;

namespace Variable.Regen;

/// <summary>
///     A bounded float value that automatically regenerates or decays over time.
/// </summary>
[Serializable]
public struct RegenFloat
{
    public BoundedFloat Value;
    public float Rate; // Units per second

    public RegenFloat(float max, float current, float rate)
    {
        Value = new BoundedFloat(max, current);
        Rate = rate;
    }

    public RegenFloat(BoundedFloat value, float rate)
    {
        Value = value;
        Rate = rate;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Tick(float deltaTime)
    {
        RegenLogic.Tick(ref Value, Rate, deltaTime);
    }

    public static implicit operator float(RegenFloat regen)
    {
        return regen.Value.Current;
    }

    public static implicit operator BoundedFloat(RegenFloat regen)
    {
        return regen.Value;
    }
}