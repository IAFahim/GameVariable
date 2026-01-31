namespace Variable.Curve.Tests;

public class CurveTests
{
    [Theory]
    [InlineData(EaseType.Linear)]
    [InlineData(EaseType.InQuad)]
    [InlineData(EaseType.OutQuad)]
    [InlineData(EaseType.InOutQuad)]
    [InlineData(EaseType.InCubic)]
    [InlineData(EaseType.OutCubic)]
    [InlineData(EaseType.InOutCubic)]
    [InlineData(EaseType.InQuart)]
    [InlineData(EaseType.OutQuart)]
    [InlineData(EaseType.InOutQuart)]
    [InlineData(EaseType.InQuint)]
    [InlineData(EaseType.OutQuint)]
    [InlineData(EaseType.InOutQuint)]
    [InlineData(EaseType.InSine)]
    [InlineData(EaseType.OutSine)]
    [InlineData(EaseType.InOutSine)]
    [InlineData(EaseType.InExpo)]
    [InlineData(EaseType.OutExpo)]
    [InlineData(EaseType.InOutExpo)]
    [InlineData(EaseType.InCirc)]
    [InlineData(EaseType.OutCirc)]
    [InlineData(EaseType.InOutCirc)]
    [InlineData(EaseType.InBack)]
    [InlineData(EaseType.OutBack)]
    [InlineData(EaseType.InOutBack)]
    [InlineData(EaseType.InElastic)]
    [InlineData(EaseType.OutElastic)]
    [InlineData(EaseType.InOutElastic)]
    [InlineData(EaseType.InBounce)]
    [InlineData(EaseType.OutBounce)]
    [InlineData(EaseType.InOutBounce)]
    public void Ease_StartAndEnd_AreCorrect(EaseType type)
    {
        float start = 0f.Ease(type);
        float end = 1f.Ease(type);

        // Easing functions usually start at 0 and end at 1.
        Assert.Equal(0f, start, 0.001f);
        Assert.Equal(1f, end, 0.001f);
    }

    [Fact]
    public void Linear_Midpoint_IsHalf()
    {
        Assert.Equal(0.5f, 0.5f.Ease(EaseType.Linear));
    }

    [Fact]
    public void InQuad_Midpoint_IsQuarter()
    {
        Assert.Equal(0.25f, 0.5f.Ease(EaseType.InQuad));
    }

    [Fact]
    public void OutQuad_Midpoint_IsThreeQuarters()
    {
        // 1 - (1-0.5)^2 = 1 - 0.25 = 0.75
        Assert.Equal(0.75f, 0.5f.Ease(EaseType.OutQuad));
    }

    [Fact]
    public void InOutQuad_Midpoint_IsHalf()
    {
        // InOut usually crosses 0.5 at 0.5
        Assert.Equal(0.5f, 0.5f.Ease(EaseType.InOutQuad));
    }

    [Fact]
    public void Back_Overshoots()
    {
        // InBack at 0.5 should be negative
        float val = 0.5f.Ease(EaseType.InBack);
        Assert.True(val < 0f);
    }
}
