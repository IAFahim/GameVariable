using Variable.Tween;
using Xunit;

namespace Variable.Tween.Tests;

public class TweenFloatTests
{
    [Fact]
    public void Constructor_SetsInitialValues()
    {
        var tween = new TweenFloat(0f, 100f, 2f, EasingType.Linear);
        Assert.Equal(0f, tween.Start);
        Assert.Equal(100f, tween.End);
        Assert.Equal(2f, tween.Timer.Duration);
        Assert.Equal(0f, tween.Timer.Current);
        Assert.Equal(0f, tween.Current);
        Assert.Equal(EasingType.Linear, tween.Easing);
    }

    [Fact]
    public void Tick_UpdatesCurrent()
    {
        var tween = new TweenFloat(0f, 100f, 2f, EasingType.Linear);

        tween.Tick(1f); // 50%

        Assert.Equal(1f, tween.Timer.Current);
        Assert.Equal(50f, tween.Current);
    }

    [Fact]
    public void Tick_Completes()
    {
        var tween = new TweenFloat(0f, 100f, 2f, EasingType.Linear);

        bool finished = tween.Tick(2f);

        Assert.True(finished);
        Assert.True(tween.IsComplete());
        Assert.Equal(100f, tween.Current);
    }

    [Fact]
    public void Reset_RestoresInitialState()
    {
        var tween = new TweenFloat(0f, 100f, 2f, EasingType.Linear);
        tween.Tick(2f);

        tween.Reset();

        Assert.Equal(0f, tween.Timer.Current);
        Assert.Equal(0f, tween.Current);
        Assert.False(tween.IsComplete());
    }

    [Fact]
    public void Tick_ZeroDuration_SnapsToEnd()
    {
        var tween = new TweenFloat(0f, 100f, 0f, EasingType.Linear);
        bool finished = tween.Tick(0.1f);

        Assert.True(finished);
        Assert.Equal(100f, tween.Current);
    }
}
