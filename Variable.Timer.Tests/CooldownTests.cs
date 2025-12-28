namespace Variable.Timer.Tests;

public class CooldownTests
{
    #region Construction Tests

    [Fact]
    public void Constructor_WithDuration_SetsCurrentToZero()
    {
        var cd = new Cooldown(5f);
        Assert.Equal(0f, cd.Current);
        Assert.Equal(5f, cd.Duration);
    }

    [Fact]
    public void Constructor_WithDurationAndCurrent_SetsValues()
    {
        var cd = new Cooldown(5f, 3f);
        Assert.Equal(3f, cd.Current);
        Assert.Equal(5f, cd.Duration);
    }

    #endregion

    #region Tick Tests

    [Fact]
    public void Tick_DecreasesCurrent()
    {
        var cd = new Cooldown(5f, 5f);
        cd.Tick(2f);
        Assert.Equal(3f, cd.Current);
    }

    [Fact]
    public void Tick_ClampsAtZero()
    {
        var cd = new Cooldown(5f, 2f);
        cd.Tick(5f);
        Assert.Equal(0f, cd.Current);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenReady()
    {
        var cd = new Cooldown(5f, 0f);
        Assert.True(cd.IsFull());
    }

    [Fact]
    public void IsFull_ReturnsFalse_WhenOnCooldown()
    {
        var cd = new Cooldown(5f, 3f);
        Assert.False(cd.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenFullyCoolingDown()
    {
        var cd = new Cooldown(5f, 5f);
        Assert.True(cd.IsEmpty());
    }

    #endregion

    #region Reset/Finish Tests

    [Fact]
    public void Reset_SetsCurrentToDuration()
    {
        var cd = new Cooldown(5f, 0f);
        cd.Reset();
        Assert.Equal(5f, cd.Current);
    }

    [Fact]
    public void Finish_SetsCurrentToZero()
    {
        var cd = new Cooldown(5f, 3f);
        cd.Finish();
        Assert.Equal(0f, cd.Current);
    }

    #endregion

    #region Usage Pattern Tests

    [Fact]
    public void TypicalUsage_AbilityWithCooldown()
    {
        var cd = new Cooldown(3f); // 3 second cooldown
        
        // Initially ready
        Assert.True(cd.IsFull());
        
        // Use ability - start cooldown
        cd.Reset();
        Assert.False(cd.IsFull());
        Assert.Equal(3f, cd.Current);
        
        // Time passes
        cd.Tick(1f);
        Assert.Equal(2f, cd.Current);
        Assert.False(cd.IsFull());
        
        cd.Tick(1f);
        Assert.Equal(1f, cd.Current);
        Assert.False(cd.IsFull());
        
        cd.Tick(1f);
        Assert.Equal(0f, cd.Current);
        Assert.True(cd.IsFull()); // Ready again
    }

    #endregion

    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectValue()
    {
        var cd = new Cooldown(10f, 5f);
        Assert.Equal(0.5, cd.GetRatio(), 5);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalCooldowns()
    {
        var a = new Cooldown(5f, 3f);
        var b = new Cooldown(5f, 3f);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentCooldowns()
    {
        var a = new Cooldown(5f, 3f);
        var b = new Cooldown(5f, 2f);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    #endregion
}
