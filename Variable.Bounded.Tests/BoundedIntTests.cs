namespace Variable.Bounded.Tests;

public class BoundedIntTests
{
    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_ToInt_ReturnsCurrent()
    {
        var bounded = new BoundedInt(100, 0, 42);
        int value = bounded;
        Assert.Equal(42, value);
    }

    #endregion

    #region Construction Tests

    [Fact]
    public void Constructor_WithMaxOnly_SetsCurrentToMax()
    {
        var bounded = new BoundedInt(100);
        Assert.Equal(100, bounded.Current);
        Assert.Equal(100, bounded.Max);
        Assert.Equal(0, bounded.Min);
    }

    [Fact]
    public void Constructor_WithMaxAndCurrent_SetsValues()
    {
        var bounded = new BoundedInt(100, 50);
        Assert.Equal(50, bounded.Current);
        Assert.Equal(100, bounded.Max);
        Assert.Equal(0, bounded.Min); // Min defaults to 0
    }

    [Fact]
    public void Constructor_WithMinMaxCurrent_SetsAllValues()
    {
        var bounded = new BoundedInt(100, -50, 25);
        Assert.Equal(25, bounded.Current);
        Assert.Equal(100, bounded.Max);
        Assert.Equal(-50, bounded.Min);
    }

    [Fact]
    public void Constructor_ClampsToCeiling_WhenCurrentExceedsMax()
    {
        var bounded = new BoundedInt(100, 0, 150);
        Assert.Equal(100, bounded.Current);
    }

    [Fact]
    public void Constructor_ClampsToFloor_WhenCurrentBelowMin()
    {
        var bounded = new BoundedInt(100, 0, -10);
        Assert.Equal(0, bounded.Current);
    }

    [Fact]
    public void Constructor_SupportsNegativeMin()
    {
        var bounded = new BoundedInt(50, -50, -25);
        Assert.Equal(-25, bounded.Current);
        Assert.Equal(-50, bounded.Min);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenCurrentEqualsMax()
    {
        var bounded = new BoundedInt(100);
        Assert.True(bounded.IsFull());
    }

    [Fact]
    public void IsFull_ReturnsFalse_WhenCurrentLessThanMax()
    {
        var bounded = new BoundedInt(100, 0, 50);
        Assert.False(bounded.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenCurrentEqualsMin()
    {
        var bounded = new BoundedInt(100, 0, 0);
        Assert.True(bounded.IsEmpty());
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_WhenCurrentGreaterThanMin()
    {
        var bounded = new BoundedInt(100, 0, 50);
        Assert.False(bounded.IsEmpty());
    }

    #endregion

    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectRatio()
    {
        var bounded = new BoundedInt(100, 0, 50);
        Assert.Equal(0.5, bounded.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsZero_WhenRangeIsZero()
    {
        var bounded = new BoundedInt(0, 0, 0);
        Assert.Equal(0.0, bounded.GetRatio());
    }

    [Fact]
    public void GetRatio_WorksWithNegativeMin()
    {
        var bounded = new BoundedInt(50, -50, 0);
        Assert.Equal(0.5, bounded.GetRatio(), 5); // 0 is 50% between -50 and 50
    }

    #endregion

    #region Arithmetic Tests

    [Fact]
    public void Addition_ClampsToMax()
    {
        var bounded = new BoundedInt(100, 0, 90);
        bounded = bounded + 20;
        Assert.Equal(100, bounded.Current);
    }

    [Fact]
    public void Subtraction_ClampsToMin()
    {
        var bounded = new BoundedInt(100, 0, 10);
        bounded = bounded - 20;
        Assert.Equal(0, bounded.Current);
    }

    [Fact]
    public void Increment_IncreasesBy1()
    {
        var bounded = new BoundedInt(100, 0, 50);
        bounded++;
        Assert.Equal(51, bounded.Current);
    }

    [Fact]
    public void Decrement_DecreasesBy1()
    {
        var bounded = new BoundedInt(100, 0, 50);
        bounded--;
        Assert.Equal(49, bounded.Current);
    }

    [Fact]
    public void Addition_HandlesIntOverflow()
    {
        var bounded = new BoundedInt(int.MaxValue, 0, int.MaxValue - 100);
        bounded = bounded + 200;
        Assert.Equal(int.MaxValue, bounded.Current);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalValues()
    {
        var a = new BoundedInt(100, 0, 50);
        var b = new BoundedInt(100, 0, 50);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentValues()
    {
        var a = new BoundedInt(100, 0, 50);
        var b = new BoundedInt(100, 0, 60);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    #endregion
}