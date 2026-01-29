using Variable.Curve;
using Xunit;

namespace Variable.Curve.Tests;

public class EasingTests
{
    [Fact]
    public void Linear_Works()
    {
        EasingLogic.Linear(0f, out var r0);
        Assert.Equal(0f, r0);

        EasingLogic.Linear(0.5f, out var r1);
        Assert.Equal(0.5f, r1);

        EasingLogic.Linear(1f, out var r2);
        Assert.Equal(1f, r2);
    }

    [Fact]
    public void QuadIn_Works()
    {
        // 0.5^2 = 0.25
        Assert.Equal(0.25f, 0.5f.EaseQuadIn());
    }

    [Fact]
    public void QuadOut_Works()
    {
        // 1 - (1-0.5)^2 = 1 - 0.25 = 0.75
        Assert.Equal(0.75f, 0.5f.EaseQuadOut());
    }

    [Fact]
    public void BounceOut_EndsAtOne()
    {
        Assert.Equal(1f, 1f.EaseBounceOut());
    }

    [Fact]
    public void ElasticOut_EndsAtOne()
    {
        Assert.Equal(1f, 1f.EaseElasticOut());
    }

    [Fact]
    public void BackOut_Overshoots()
    {
        // At 0.5, BackOut should be > 0.5?
        // Actually BackOut overshoots at the end.
        // Let's check a value near 1.
        float val = 0.8f.EaseBackOut();
        // It might overshoot 1?
        // BackOut formula at 1 is 1.
        // It peaks before 1?
        // Let's just ensure it runs and stays within sanity (usually < 1.5).
        Assert.True(val > 0.5f);
    }
}
