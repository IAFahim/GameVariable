namespace Variable.Timer.Tests;

public class TimerTests
{
    #region ToString Tests

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var timer = new Timer(10f, 5f);
        Assert.Equal("5.00/10.00", timer.ToString());
    }

    #endregion

    #region Construction Tests

    [Fact]
    public void Constructor_WithDuration_SetsCurrentToZero()
    {
        var timer = new Timer(10f);
        Assert.Equal(0f, timer.Current);
        Assert.Equal(10f, timer.Duration);
    }

    [Fact]
    public void Constructor_WithDurationAndCurrent_SetsValues()
    {
        var timer = new Timer(10f, 5f);
        Assert.Equal(5f, timer.Current);
        Assert.Equal(10f, timer.Duration);
    }

    [Fact]
    public void Constructor_ClampsCurrent_WhenExceedsDuration()
    {
        var timer = new Timer(10f, 15f);
        Assert.Equal(10f, timer.Current);
    }

    [Fact]
    public void Constructor_ClampsCurrent_WhenNegative()
    {
        var timer = new Timer(10f, -5f);
        Assert.Equal(0f, timer.Current);
    }

    #endregion

    #region Tick Tests

    [Fact]
    public void TickAndCheckComplete_IncreasesCurrentByDelta()
    {
        var timer = new Timer(10f);
        timer.TickAndCheckComplete(2.5f);
        Assert.Equal(2.5f, timer.Current);
    }

    [Fact]
    public void TickAndCheckComplete_ClampsAtDuration()
    {
        var timer = new Timer(10f, 8f);
        timer.TickAndCheckComplete(5f);
        Assert.Equal(10f, timer.Current);
    }

    [Fact]
    public void TickAndCheckComplete_MultipleCallsAccumulate()
    {
        var timer = new Timer(10f);
        timer.TickAndCheckComplete(1f);
        timer.TickAndCheckComplete(1f);
        timer.TickAndCheckComplete(1f);
        Assert.Equal(3f, timer.Current);
    }

    [Fact]
    public void TickAndCheckComplete_ReturnsFalse_WhenNotComplete()
    {
        var timer = new Timer(10f);
        var isComplete = timer.TickAndCheckComplete(5f);
        Assert.False(isComplete);
        Assert.Equal(5f, timer.Current);
    }

    [Fact]
    public void TickAndCheckComplete_ReturnsTrue_WhenReachesDuration()
    {
        var timer = new Timer(10f, 9f);
        var isComplete = timer.TickAndCheckComplete(1f);
        Assert.True(isComplete);
        Assert.Equal(10f, timer.Current);
    }

    [Fact]
    public void TickAndCheckComplete_ReturnsTrue_WhenExceedsDuration()
    {
        var timer = new Timer(10f, 8f);
        var isComplete = timer.TickAndCheckComplete(5f);
        Assert.True(isComplete);
        Assert.Equal(10f, timer.Current);
    }

    [Fact]
    public void TickAndCheckComplete_WithOverflow_OutputsCorrectOverflow()
    {
        var timer = new Timer(10f, 8f);
        var isComplete = timer.TickAndCheckComplete(5f, out var overflow);
        Assert.True(isComplete);
        Assert.Equal(10f, timer.Current);
        Assert.Equal(3f, overflow, 5); // 8 + 5 = 13, overflow = 13 - 10 = 3
    }

    [Fact]
    public void TickAndCheckComplete_WithOverflow_NoOverflowWhenNotComplete()
    {
        var timer = new Timer(10f);
        var isComplete = timer.TickAndCheckComplete(5f, out var overflow);
        Assert.False(isComplete);
        Assert.Equal(5f, timer.Current);
        Assert.Equal(0f, overflow);
    }

    [Fact]
    public void Tick_VoidVersion_AdvancesTimer()
    {
        var timer = new Timer(10f);
        timer.Tick(3f);
        Assert.Equal(3f, timer.Current);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenFinished()
    {
        var timer = new Timer(10f, 10f);
        Assert.True(timer.IsFull());
    }

    [Fact]
    public void IsFull_ReturnsFalse_WhenNotFinished()
    {
        var timer = new Timer(10f, 5f);
        Assert.False(timer.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenZero()
    {
        var timer = new Timer(10f);
        Assert.True(timer.IsEmpty());
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_WhenStarted()
    {
        var timer = new Timer(10f, 1f);
        Assert.False(timer.IsEmpty());
    }

    #endregion

    #region Reset/Finish Tests

    [Fact]
    public void Reset_SetsCurrentToZero()
    {
        var timer = new Timer(10f, 5f);
        timer.Reset();
        Assert.Equal(0f, timer.Current);
    }

    [Fact]
    public void Finish_SetsCurrentToDuration()
    {
        var timer = new Timer(10f, 5f);
        timer.Finish();
        Assert.Equal(10f, timer.Current);
    }

    #endregion

    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectValue()
    {
        var timer = new Timer(10f, 5f);
        Assert.Equal(0.5, timer.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsZero_WhenDurationIsZero()
    {
        var timer = new Timer(0f);
        Assert.Equal(0.0, timer.GetRatio());
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalTimers()
    {
        var a = new Timer(10f, 5f);
        var b = new Timer(10f, 5f);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentTimers()
    {
        var a = new Timer(10f, 5f);
        var b = new Timer(10f, 6f);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    #endregion
}