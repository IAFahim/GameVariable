namespace Variable.Tween.Tests;

public class TweenExtensionTests
{
    [Fact]
    public void Tick_UpdatesStructValues()
    {
        var tween = new TweenFloat(0f, 10f, 2f, EasingType.Linear);

        tween.Tick(1f);

        Assert.Equal(1f, tween.ElapsedSeconds);
        Assert.Equal(5f, tween.CurrentValue);
        Assert.False(tween.IsComplete());
    }

    [Fact]
    public void IsComplete_ReturnsTrueWhenFinished()
    {
        var tween = new TweenFloat(0f, 10f, 1f);
        tween.ElapsedSeconds = 1f; // Manually fast forward

        Assert.True(tween.IsComplete());
    }

    [Fact]
    public void GetProgress_ReturnsCorrectRatio()
    {
        var tween = new TweenFloat(0f, 10f, 2f);
        tween.ElapsedSeconds = 1f;

        Assert.Equal(0.5f, tween.GetProgress());
    }

    [Fact]
    public void Reset_RestoresInitialState()
    {
        var tween = new TweenFloat(10f, 100f, 5f);
        tween.CurrentValue = 50f;
        tween.ElapsedSeconds = 5f;

        tween.Reset();

        Assert.Equal(0f, tween.ElapsedSeconds);
        Assert.Equal(10f, tween.CurrentValue);
    }

    [Fact]
    public void Complete_ForcesEndState()
    {
        var tween = new TweenFloat(0f, 100f, 10f);

        tween.Complete();

        Assert.Equal(10f, tween.ElapsedSeconds);
        Assert.Equal(100f, tween.CurrentValue);
        Assert.True(tween.IsComplete());
    }
}
