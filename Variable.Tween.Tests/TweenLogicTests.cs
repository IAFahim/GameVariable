namespace Variable.Tween.Tests;

public class TweenLogicTests
{
    [Fact]
    public void Tick_AdvancesTime()
    {
        float current = 0f;
        float duration = 10f;
        float dt = 1f;

        TweenLogic.Tick(in current, in duration, in dt, out float newElapsed, out bool isComplete);

        Assert.Equal(1f, newElapsed);
        Assert.False(isComplete);
    }

    [Fact]
    public void Tick_ClampsAtDuration()
    {
        float current = 9.5f;
        float duration = 10f;
        float dt = 1f;

        TweenLogic.Tick(in current, in duration, in dt, out float newElapsed, out bool isComplete);

        Assert.Equal(10f, newElapsed);
        Assert.True(isComplete);
    }

    [Theory]
    [InlineData(0f, 10f, 0f)]
    [InlineData(5f, 10f, 0.5f)]
    [InlineData(10f, 10f, 1f)]
    [InlineData(15f, 10f, 1f)]
    [InlineData(-5f, 10f, 0f)]
    public void GetNormalizedTime_CalculatesCorrectly(float elapsed, float duration, float expected)
    {
        TweenLogic.GetNormalizedTime(in elapsed, in duration, out float t);
        Assert.Equal(expected, t);
    }

    [Fact]
    public void Lerp_InterpolatesCorrectly()
    {
        float a = 0f;
        float b = 100f;
        float t = 0.5f;

        TweenLogic.Lerp(in a, in b, in t, out float result);

        Assert.Equal(50f, result);
    }

    [Theory]
    [InlineData(EasingType.Linear, 0.5f, 50f)]
    [InlineData(EasingType.QuadIn, 0.5f, 25f)] // 0.5^2 = 0.25 * 100 = 25
    public void Evaluate_CalculatesCorrectly(EasingType easing, float t, float expectedValue)
    {
        float start = 0f;
        float end = 100f;

        TweenLogic.Evaluate(in start, in end, in t, in easing, out float result);

        Assert.Equal(expectedValue, result, 0.001f);
    }
}
