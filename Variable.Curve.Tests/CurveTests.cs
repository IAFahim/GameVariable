namespace Variable.Curve.Tests;

using Xunit;
using Variable.Curve;

public class CurveTests
{
    [Fact]
    public void Initialization_SetsValuesCorrectly()
    {
        var curve = new CurveFloat(2f, 0f, 100f, EaseType.Linear);

        Assert.Equal(0f, curve.CurrentTime);
        Assert.Equal(2f, curve.Duration);
        Assert.Equal(0f, curve.StartValue);
        Assert.Equal(100f, curve.EndValue);
        Assert.Equal(EaseType.Linear, curve.Type);
    }

    [Fact]
    public void Tick_AdvancesTime()
    {
        var curve = new CurveFloat(2f, 0f, 100f);
        curve.Tick(0.5f);

        Assert.Equal(0.5f, curve.CurrentTime);
    }

    [Fact]
    public void Tick_ClampsToDuration()
    {
        var curve = new CurveFloat(2f, 0f, 100f);
        curve.Tick(3f);

        Assert.Equal(2f, curve.CurrentTime);
        Assert.True(curve.IsFinished());
    }

    [Fact]
    public void GetValue_Linear_InterpolatesCorrectly()
    {
        var curve = new CurveFloat(2f, 0f, 100f, EaseType.Linear);

        curve.Tick(1f); // 50%
        Assert.Equal(50f, curve.GetValue(), 0.001f);

        curve.Tick(1f); // 100%
        Assert.Equal(100f, curve.GetValue(), 0.001f);
    }

    [Fact]
    public void GetValue_InQuad_InterpolatesCorrectly()
    {
        var curve = new CurveFloat(2f, 0f, 100f, EaseType.InQuad);

        curve.Tick(1f); // 50% time -> 0.5 * 0.5 = 0.25 eased -> 25 value
        Assert.Equal(25f, curve.GetValue(), 0.001f);
    }

    [Fact]
    public void GetRatio_ReturnsNormalizedTime()
    {
        var curve = new CurveFloat(10f, 0f, 100f);

        curve.Tick(2f);
        Assert.Equal(0.2f, curve.GetRatio(), 0.001f);
    }

    [Fact]
    public void Reset_SetsTimeBackToZero()
    {
        var curve = new CurveFloat(2f, 0f, 100f);
        curve.Tick(2f);
        Assert.True(curve.IsFinished());

        curve.Reset();
        Assert.Equal(0f, curve.CurrentTime);
        Assert.False(curve.IsFinished());
        Assert.Equal(0f, curve.GetValue());
    }

    [Fact]
    public void GetValue_HandlesZeroDuration()
    {
        var curve = new CurveFloat(0f, 0f, 100f);

        // Should immediately be at end value
        Assert.Equal(100f, curve.GetValue());
        Assert.True(curve.IsFinished());
    }
}
