namespace Variable.Timer.Tests;

public class CooldownTests
{
    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectValue()
    {
        var cd = new Cooldown(10f, 5f);
        Assert.Equal(0.5, cd.GetRatio(), 5);
    }

    #endregion

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

    [Fact]
    public void Tick_ReturnsOverflow_WhenExceedsCooldown()
    {
        var cd = new Cooldown(0.5f, 0.5f); // 0.5 second cooldown
        var overflow = cd.Tick(2f); // 2 second tick

        Assert.Equal(0f, cd.Current); // Clamped to 0
        Assert.Equal(1.5f, overflow, 4); // 2.0 - 0.5 = 1.5 overflow
    }

    [Fact]
    public void Tick_ReturnsZero_WhenNoOverflow()
    {
        var cd = new Cooldown(5f, 5f);
        var overflow = cd.Tick(2f);

        Assert.Equal(0f, overflow);
        Assert.Equal(3f, cd.Current);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenReady()
    {
        var cd = new Cooldown(5f);
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

    [Fact]
    public void IsReady_IsSynonymForIsFull()
    {
        var ready = new Cooldown(5f);
        var notReady = new Cooldown(5f, 3f);

        Assert.True(ready.IsReady());
        Assert.False(notReady.IsReady());
    }

    [Fact]
    public void IsOnCooldown_OppositeOfIsReady()
    {
        var ready = new Cooldown(5f);
        var notReady = new Cooldown(5f, 3f);

        Assert.False(ready.IsOnCooldown());
        Assert.True(notReady.IsOnCooldown());
    }

    [Fact]
    public void Progress_Returns0When_JustStarted()
    {
        var cd = new Cooldown(10f, 10f); // Full cooldown
        Assert.Equal(0f, cd.Progress, 4);
    }

    [Fact]
    public void Progress_Returns1When_Ready()
    {
        var cd = new Cooldown(10f); // Ready
        Assert.Equal(1f, cd.Progress, 4);
    }

    [Fact]
    public void Progress_Returns50Percent_Halfway()
    {
        var cd = new Cooldown(10f, 5f); // Halfway
        Assert.Equal(0.5f, cd.Progress, 4);
    }

    #endregion

    #region Reset/Finish Tests

    [Fact]
    public void Reset_SetsCurrentToDuration()
    {
        var cd = new Cooldown(5f);
        cd.Reset();
        Assert.Equal(5f, cd.Current);
    }

    [Fact]
    public void Reset_WithOverflow_SubtractsFromDuration()
    {
        var cd = new Cooldown(5f);
        cd.Reset(1.5f); // Apply 1.5s overflow
        Assert.Equal(3.5f, cd.Current); // 5 - 1.5 = 3.5
    }

    [Fact]
    public void Reset_WithLargeOverflow_ClampsToZero()
    {
        var cd = new Cooldown(5f);
        cd.Reset(10f); // Overflow larger than duration
        Assert.Equal(0f, cd.Current);
    }

    [Fact]
    public void Finish_SetsCurrentToZero()
    {
        var cd = new Cooldown(5f, 3f);
        cd.Finish();
        Assert.Equal(0f, cd.Current);
    }

    #endregion

    #region TryUse Tests

    [Fact]
    public void TryUse_ReturnsTrue_WhenReady()
    {
        var cd = new Cooldown(5f); // Ready
        Assert.True(cd.TryUse());
        Assert.Equal(5f, cd.Current); // Now on cooldown
    }

    [Fact]
    public void TryUse_ReturnsFalse_WhenOnCooldown()
    {
        var cd = new Cooldown(5f, 3f); // On cooldown
        Assert.False(cd.TryUse());
        Assert.Equal(3f, cd.Current); // Unchanged
    }

    [Fact]
    public void TryUse_WithOverflow_AppliesOverflow()
    {
        var cd = new Cooldown(5f);
        Assert.True(cd.TryUse(1f)); // Use with 1s overflow
        Assert.Equal(4f, cd.Current); // 5 - 1 = 4
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

    [Fact]
    public void HighPrecisionUsage_WithOverflowHandling()
    {
        // Scenario: 5s cooldown, ready, use with 1.5s overflow from previous
        var cd = new Cooldown(5f);

        Assert.True(cd.IsReady());

        // Use ability with overflow from previous cycle
        cd.Reset(1.5f);
        Assert.Equal(3.5f, cd.Current, 4); // 5 - 1.5 = 3.5

        // Big tick that overshoots
        var overflow = cd.Tick(5f);
        Assert.True(cd.IsReady());
        Assert.Equal(1.5f, overflow, 4); // 5 - 3.5 = 1.5 overflow
    }

    [Fact]
    public void PreciseOverflow_RhythmGameScenario()
    {
        // Beat every 0.5 seconds
        var beatInterval = new Cooldown(0.5f, 0.5f);
        var beatCount = 0;
        var totalTime = 2.0f;

        var overflow = beatInterval.Tick(totalTime);
        if (beatInterval.IsReady()) beatCount++;

        // We passed 2s with 0.5s interval, so we should have hit 4 beats
        // But with single tick we only detect one completion
        // The overflow (1.5s) represents the extra 3 beats worth of time
        Assert.True(beatInterval.IsReady());
        Assert.Equal(1.5f, overflow, 4);
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

    #region ToString Tests

    [Fact]
    public void ToString_ReturnsReady_WhenComplete()
    {
        var cd = new Cooldown(5f);
        Assert.Equal("Ready", cd.ToString());
    }

    [Fact]
    public void ToString_ReturnsFormattedTime_WhenOnCooldown()
    {
        var cd = new Cooldown(5f, 3f);
        Assert.Equal("3.00s", cd.ToString());
    }

    #endregion
}