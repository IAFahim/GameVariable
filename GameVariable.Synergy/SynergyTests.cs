using Variable.Bounded;
using Variable.Regen;
using Xunit;

namespace GameVariable.Synergy;

public class SynergyTests
{
    [Fact]
    public void RegenFloat_Updates_BoundedFloat_Correctly()
    {
        // Integration test between Variable.Regen and Variable.Bounded
        // RegenFloat wraps BoundedFloat.

        // Setup
        float max = 100f;
        float current = 50f;
        float rate = 10f;
        var regen = new RegenFloat(max, current, rate);

        // Tick
        float dt = 1.0f;
        RegenExtensions.Tick(ref regen, dt);

        // Verify BoundedFloat updated
        Assert.Equal(60f, regen.Value.Current);

        // Verify clamping via BoundedFloat logic (implicitly tested via RegenLogic)
        RegenExtensions.Tick(ref regen, 10.0f); // Should hit max
        Assert.Equal(100f, regen.Value.Current);
    }

    [Fact]
    public void RegenFloat_Decay_Updates_BoundedFloat_Correctly()
    {
        // Setup
        float max = 100f;
        float current = 50f;
        float rate = -10f;
        var regen = new RegenFloat(max, current, rate);

        // Tick
        float dt = 1.0f;
        RegenExtensions.Tick(ref regen, dt);

        // Verify BoundedFloat updated
        Assert.Equal(40f, regen.Value.Current);

        // Verify clamping via BoundedFloat logic
        RegenExtensions.Tick(ref regen, 10.0f); // Should hit min 0
        Assert.Equal(0f, regen.Value.Current);
    }
}
