namespace Variable.Tween.Tests;

public class TweenTests
{
    [Fact]
    public void LinearTween_InterpolatesCorrectly()
    {
        var tween = new TweenFloat(0f, 100f, 2f); // 0 to 100 over 2 sec

        Assert.Equal(0f, tween.Current);

        bool finished = tween.Tick(1f); // 1 sec passed (50%)

        Assert.Equal(50f, tween.Current, 0.001f);
        Assert.Equal(1f, tween.Elapsed);
        Assert.False(finished);
        Assert.False(tween.IsComplete());

        finished = tween.Tick(1f); // 2 sec passed (100%)

        Assert.Equal(100f, tween.Current);
        Assert.True(finished);
        Assert.True(tween.IsComplete());
    }

    [Fact]
    public void Overshoot_ClampsToEnd()
    {
        var tween = new TweenFloat(0f, 100f, 1f);

        bool finished = tween.Tick(1.5f); // 150% time

        Assert.Equal(100f, tween.Current);
        Assert.Equal(1f, tween.Elapsed); // Should clamp elapsed to duration
        Assert.True(finished);
    }

    [Fact]
    public void Play_ResetsAndConfigures()
    {
        var tween = new TweenFloat(0f, 10f, 1f);
        tween.Tick(1f);
        Assert.True(tween.IsComplete());

        tween.Play(10f, 20f, 2f);
        Assert.Equal(10f, tween.Start);
        Assert.Equal(20f, tween.End);
        Assert.Equal(0f, tween.Elapsed);
        Assert.Equal(10f, tween.Current);
        Assert.False(tween.IsComplete());
    }

    [Fact]
    public void Easing_QuadIn_CalculatesCorrectly()
    {
        // QuadIn: t^2
        var tween = new TweenFloat(0f, 100f, 2f, EasingType.QuadIn);

        tween.Tick(1f); // t = 0.5
        // Result = 0 + 100 * (0.5)^2 = 25

        Assert.Equal(25f, tween.Current, 0.001f);
    }

    [Fact]
    public void Reset_SetsToStart()
    {
        var tween = new TweenFloat(0f, 100f, 1f);
        tween.Tick(0.5f);
        Assert.Equal(50f, tween.Current);

        tween.Reset();
        Assert.Equal(0f, tween.Current);
        Assert.Equal(0f, tween.Elapsed);
    }
}
