using Xunit;
using Variable.Curve;

namespace Variable.Curve.Tests;

public class EasingTests
{
    [Theory]
    [InlineData(EasingType.Linear)]
    [InlineData(EasingType.InSine)]
    [InlineData(EasingType.OutSine)]
    [InlineData(EasingType.InOutSine)]
    [InlineData(EasingType.InQuad)]
    [InlineData(EasingType.OutQuad)]
    [InlineData(EasingType.InOutQuad)]
    [InlineData(EasingType.InCubic)]
    [InlineData(EasingType.OutCubic)]
    [InlineData(EasingType.InOutCubic)]
    [InlineData(EasingType.InQuart)]
    [InlineData(EasingType.OutQuart)]
    [InlineData(EasingType.InOutQuart)]
    [InlineData(EasingType.InQuint)]
    [InlineData(EasingType.OutQuint)]
    [InlineData(EasingType.InOutQuint)]
    [InlineData(EasingType.InExpo)]
    [InlineData(EasingType.OutExpo)]
    [InlineData(EasingType.InOutExpo)]
    [InlineData(EasingType.InCirc)]
    [InlineData(EasingType.OutCirc)]
    [InlineData(EasingType.InOutCirc)]
    [InlineData(EasingType.InBack)]
    [InlineData(EasingType.OutBack)]
    [InlineData(EasingType.InOutBack)]
    [InlineData(EasingType.InElastic)]
    [InlineData(EasingType.OutElastic)]
    [InlineData(EasingType.InOutElastic)]
    [InlineData(EasingType.InBounce)]
    [InlineData(EasingType.OutBounce)]
    [InlineData(EasingType.InOutBounce)]
    public void Easing_StartAndEnd_AreCorrect(EasingType type)
    {
        // All standard easings should start at 0 and end at 1
        // Using strict equality for 0 and 1 because these functions are usually exact at boundaries
        // But due to float precision, we should use tolerance.

        // However, for Linear, Quad, etc, 0 is 0.
        // Elastic/Bounce might overshoot but at t=0 and t=1 they should be exact.

        Assert.Equal(0f, 0f.Ease(type), 0.0001f);
        Assert.Equal(1f, 1f.Ease(type), 0.0001f);
    }

    [Fact]
    public void Easing_Linear_IsLinear()
    {
        Assert.Equal(0.5f, 0.5f.Ease(EasingType.Linear));
    }

    [Fact]
    public void Easing_EaseInQuad_IsQuad()
    {
        // 0.5^2 = 0.25
        Assert.Equal(0.25f, 0.5f.Ease(EasingType.InQuad));
    }

    [Fact]
    public void Lerp_WorksCorrectly()
    {
        Assert.Equal(5f, 0.5f.Lerp(0f, 10f));
        Assert.Equal(0f, 0f.Lerp(0f, 10f));
        Assert.Equal(10f, 1f.Lerp(0f, 10f));
    }

    [Fact]
    public void Easing_Extrapolation_Works()
    {
        // Linear: 2.0 -> 2.0
        Assert.Equal(2f, 2f.Ease(EasingType.Linear));

        // InQuad: 2.0^2 = 4.0
        Assert.Equal(4f, 2f.Ease(EasingType.InQuad));
    }
}
