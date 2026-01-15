using Variable.Tween;
using Xunit;

namespace Variable.Tween.Tests;

public class TweenLogicTests
{
    [Fact]
    public void Evaluate_Linear_ReturnsCorrectValues()
    {
        float start = 0f;
        float end = 100f;

        TweenLogic.Evaluate(start, end, 0f, EasingType.Linear, out var v0);
        Assert.Equal(0f, v0);

        TweenLogic.Evaluate(start, end, 0.5f, EasingType.Linear, out var v50);
        Assert.Equal(50f, v50);

        TweenLogic.Evaluate(start, end, 1f, EasingType.Linear, out var v100);
        Assert.Equal(100f, v100);
    }

    [Fact]
    public void Evaluate_QuadIn_ReturnsCorrectValues()
    {
        float start = 0f;
        float end = 100f;

        TweenLogic.Evaluate(start, end, 0.5f, EasingType.QuadIn, out var v);
        // 0.5^2 = 0.25 -> 25f
        Assert.Equal(25f, v);
    }

    [Fact]
    public void Evaluate_QuadOut_ReturnsCorrectValues()
    {
        float start = 0f;
        float end = 100f;

        TweenLogic.Evaluate(start, end, 0.5f, EasingType.QuadOut, out var v);
        // 0.5 * (2 - 0.5) = 0.5 * 1.5 = 0.75 -> 75f
        Assert.Equal(75f, v);
    }
}
