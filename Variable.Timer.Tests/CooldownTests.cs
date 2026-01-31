namespace Variable.Timer.Tests;

public class CooldownTests
{
    #region Construction Tests

    [Fact]
    public void Constructor_WithDuration_SetsCurrentToZero()
    {
        var cooldown = new Cooldown(10f);
        Assert.Equal(0f, cooldown.Current);
        Assert.Equal(10f, cooldown.Duration);
    }

    [Fact]
    public void Constructor_WithDurationAndCurrent_SetsValues()
    {
        var cooldown = new Cooldown(10f, 5f);
        Assert.Equal(5f, cooldown.Current);
        Assert.Equal(10f, cooldown.Duration);
    }

    [Fact]
    public void Constructor_ClampsCurrent_WhenExceedsDuration()
    {
        var cooldown = new Cooldown(10f, 15f);
        Assert.Equal(10f, cooldown.Current);
    }

    [Fact]
    public void Constructor_ClampsCurrent_WhenNegative()
    {
        var cooldown = new Cooldown(10f, -5f);
        Assert.Equal(0f, cooldown.Current);
    }

    #endregion

    #region Tick Tests

    [Fact]
    public void TickAndCheckReady_DecreasesCurrentByDelta()
    {
        var cooldown = new Cooldown(10f, 5f);
        cooldown.TickAndCheckReady(2f);
        Assert.Equal(3f, cooldown.Current);
    }

    [Fact]
    public void TickAndCheckReady_ClampsAtZero()
    {
        var cooldown = new Cooldown(10f, 2f);
        cooldown.TickAndCheckReady(5f);
        Assert.Equal(0f, cooldown.Current);
    }

    [Fact]
    public void TickAndCheckReady_ReturnsFalse_WhenNotReady()
    {
        var cooldown = new Cooldown(10f, 5f);
        var isReady = cooldown.TickAndCheckReady(2f);
        Assert.False(isReady);
        Assert.Equal(3f, cooldown.Current);
    }

    [Fact]
    public void TickAndCheckReady_ReturnsTrue_WhenReachesZero()
    {
        var cooldown = new Cooldown(10f, 2f);
        var isReady = cooldown.TickAndCheckReady(2f);
        Assert.True(isReady);
        Assert.Equal(0f, cooldown.Current);
    }

    [Fact]
    public void TickAndCheckReady_ReturnsTrue_WhenExceedsZero()
    {
        var cooldown = new Cooldown(10f, 2f);
        var isReady = cooldown.TickAndCheckReady(5f);
        Assert.True(isReady);
        Assert.Equal(0f, cooldown.Current);
    }

    [Fact]
    public void TickAndCheckReady_ReturnsTrue_WhenAlreadyReady()
    {
        var cooldown = new Cooldown(10f);
        var isReady = cooldown.TickAndCheckReady(1f);
        Assert.True(isReady);
        Assert.Equal(0f, cooldown.Current);
    }

    [Fact]
    public void TickAndCheckReady_WithOverflow_OutputsCorrectOverflow()
    {
        var cooldown = new Cooldown(10f, 2f);
        var isReady = cooldown.TickAndCheckReady(5f, out var overflow);
        Assert.True(isReady);
        Assert.Equal(0f, cooldown.Current);
        Assert.Equal(3f, overflow, 5); // 2 - 5 = -3, overflow = 3
    }

    [Fact]
    public void TickAndCheckReady_WithOverflow_NoOverflowWhenNotReady()
    {
        var cooldown = new Cooldown(10f, 5f);
        var isReady = cooldown.TickAndCheckReady(2f, out var overflow);
        Assert.False(isReady);
        Assert.Equal(3f, cooldown.Current);
        Assert.Equal(0f, overflow);
    }

    [Fact]
    public void Tick_VoidVersion_AdvancesCooldown()
    {
        var cooldown = new Cooldown(10f, 5f);
        cooldown.Tick(2f);
        Assert.Equal(3f, cooldown.Current);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsReady_ReturnsTrue_WhenZero()
    {
        var cooldown = new Cooldown(10f);
        Assert.True(cooldown.IsReady());
    }

    [Fact]
    public void IsReady_ReturnsFalse_WhenOnCooldown()
    {
        var cooldown = new Cooldown(10f, 5f);
        Assert.False(cooldown.IsReady());
    }

    [Fact]
    public void IsFull_ReturnsTrue_WhenReady()
    {
        var cooldown = new Cooldown(10f);
        Assert.True(cooldown.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenAtMaxDuration()
    {
        var cooldown = new Cooldown(10f, 10f);
        Assert.True(cooldown.IsEmpty());
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_WhenNotFull()
    {
        var cooldown = new Cooldown(10f, 5f);
        Assert.False(cooldown.IsEmpty());
    }

    [Fact]
    public void IsOnCooldown_ReturnsTrue_WhenActive()
    {
        var cooldown = new Cooldown(10f, 5f);
        Assert.True(cooldown.IsOnCooldown());
    }

    [Fact]
    public void IsOnCooldown_ReturnsFalse_WhenReady()
    {
        var cooldown = new Cooldown(10f);
        Assert.False(cooldown.IsOnCooldown());
    }

    #endregion

    #region Reset/Finish Tests

    [Fact]
    public void Reset_SetsCurrentToDuration()
    {
        var cooldown = new Cooldown(10f, 5f);
        cooldown.Reset();
        Assert.Equal(10f, cooldown.Current);
    }

    [Fact]
    public void Reset_WithOverflow_AppliesOverflow()
    {
        var cooldown = new Cooldown(10f);
        cooldown.Reset(3f); // Duration 10, overflow 3 -> should be 7
        Assert.Equal(7f, cooldown.Current);
    }

    [Fact]
    public void Finish_SetsCurrentToZero()
    {
        var cooldown = new Cooldown(10f, 5f);
        cooldown.Finish();
        Assert.Equal(0f, cooldown.Current);
    }

    #endregion

    #region Ratio/Progress Tests

    [Fact]
    public void GetRatio_ReturnsCorrectValue()
    {
        var cooldown = new Cooldown(10f, 5f);
        Assert.Equal(0.5f, cooldown.GetRatio(), 5);
    }

    [Fact]
    public void GetProgress_ReturnsZero_WhenJustStarted()
    {
        var cooldown = new Cooldown(10f, 10f);
        Assert.Equal(0f, cooldown.GetProgress(), 5);
    }

    [Fact]
    public void GetProgress_ReturnsOne_WhenReady()
    {
        var cooldown = new Cooldown(10f);
        Assert.Equal(1f, cooldown.GetProgress(), 5);
    }

    [Fact]
    public void GetProgress_ReturnsHalf_WhenHalfway()
    {
        var cooldown = new Cooldown(10f, 5f);
        Assert.Equal(0.5f, cooldown.GetProgress(), 5);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ReturnsReady_WhenZero()
    {
        var cooldown = new Cooldown(10f);
        Assert.Equal("Ready", cooldown.ToString());
    }

    [Fact]
    public void ToString_ReturnsFormattedTime_WhenOnCooldown()
    {
        var cooldown = new Cooldown(10f, 5.5f);
        Assert.Equal("5.50s", cooldown.ToString());
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalCooldowns()
    {
        var a = new Cooldown(10f, 5f);
        var b = new Cooldown(10f, 5f);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentCooldowns()
    {
        var a = new Cooldown(10f, 5f);
        var b = new Cooldown(10f, 6f);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    #endregion
}