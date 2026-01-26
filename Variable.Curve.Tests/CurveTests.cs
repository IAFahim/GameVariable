namespace Variable.Curve.Tests;

public class CurveTests
{
    [Fact]
    public void Linear_ReturnsLine()
    {
        Assert.Equal(0f, 0f.Ease(EaseType.Linear));
        Assert.Equal(0.5f, 0.5f.Ease(EaseType.Linear));
        Assert.Equal(1f, 1f.Ease(EaseType.Linear));
    }

    [Fact]
    public void Clamping_Works()
    {
        // Should clamp < 0 to 0
        Assert.Equal(0f, (-1f).Ease(EaseType.Linear));

        // Should clamp > 1 to 1
        Assert.Equal(1f, 2f.Ease(EaseType.Linear));
    }

    [Fact]
    public void EaseLerp_Works()
    {
        // 0.5 linear lerp between 0 and 100 is 50
        Assert.Equal(50f, 0.5f.EaseLerp(0f, 100f, EaseType.Linear));

        // 0.5 quadIn (0.25) lerp between 0 and 100 is 25
        Assert.Equal(25f, 0.5f.EaseLerp(0f, 100f, EaseType.InQuad));
    }

    [Theory]
    [InlineData(EaseType.InQuad, 0.5f, 0.25f)]
    [InlineData(EaseType.OutQuad, 0.5f, 0.75f)]
    [InlineData(EaseType.InCubic, 0.5f, 0.125f)]
    public void StandardCurves_BehaveRoughlyExpected(EaseType type, float t, float expected)
    {
        float result = t.Ease(type);
        Assert.Equal(expected, result, 0.001f);
    }

    [Fact]
    public void AllCurves_StartAt0_EndAt1()
    {
        foreach (EaseType type in Enum.GetValues(typeof(EaseType)))
        {
            Assert.Equal(0f, 0f.Ease(type), 0.001f);
            Assert.Equal(1f, 1f.Ease(type), 0.001f);
        }
    }
}
