namespace Variable.Motion.Tests;

public class TweenTests
{
    [Fact]
    public void Constructor_SetsInitialState()
    {
        var tween = new TweenFloat(10f, 20f, 2f);
        Assert.Equal(10f, tween.Start);
        Assert.Equal(20f, tween.End);
        Assert.Equal(2f, tween.Duration);
        Assert.Equal(0f, tween.Time);
        Assert.Equal(10f, tween.Value);
        Assert.Equal(EasingType.Linear, tween.Easing);
    }

    [Fact]
    public void Tick_AdvancesTimeAndValue()
    {
        var tween = new TweenFloat(0f, 100f, 2f);

        // Tick 1s (50%)
        tween.Tick(1f);
        Assert.Equal(1f, tween.Time);
        Assert.Equal(50f, tween.Value); // Linear
        Assert.False(((ICompletable)tween).IsComplete);

        // Tick 1s (100%)
        var complete = tween.Tick(1f);
        Assert.Equal(2f, tween.Time);
        Assert.Equal(100f, tween.Value);
        Assert.True(complete);
    }

    [Fact]
    public void Tick_ClampsAtDuration()
    {
        var tween = new TweenFloat(0f, 100f, 2f);

        // Tick 5s (Overshoot)
        var complete = tween.Tick(5f);
        Assert.Equal(2f, tween.Time);
        Assert.Equal(100f, tween.Value);
        Assert.True(complete);
        Assert.True(((ICompletable)tween).IsComplete);
    }

    [Fact]
    public void Easing_CalculatesCorrectly()
    {
        // QuadIn: t^2
        var tween = new TweenFloat(0f, 100f, 2f, EasingType.QuadIn);

        tween.Tick(1f); // t = 0.5
        // 0.5^2 = 0.25 -> 25f
        Assert.Equal(25f, tween.Value, 0.01f);
    }

    [Fact]
    public void Reset_RestoresState()
    {
        var tween = new TweenFloat(0f, 100f, 2f);
        tween.Tick(2f);
        Assert.Equal(100f, tween.Value);

        tween.Reset();
        Assert.Equal(0f, tween.Time);
        Assert.Equal(0f, tween.Value);
    }

    [Fact]
    public void CalculateValue_IsPure()
    {
        var tween = new TweenFloat(0f, 100f, 10f);
        tween.Time = 5f; // Manual set

        // Should calculate 50f
        Assert.Equal(50f, tween.CalculateValue());

        // Should not modify state
        Assert.Equal(0f, tween.Value);
    }
}
