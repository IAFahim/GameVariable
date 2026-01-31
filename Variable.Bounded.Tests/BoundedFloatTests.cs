using System.Globalization;

namespace Variable.Bounded.Tests;

public class BoundedFloatTests
{
    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_ToFloat_ReturnsCurrent()
    {
        var bounded = new BoundedFloat(100f, 0f, 42f);
        float value = bounded;
        Assert.Equal(42f, value);
    }

    #endregion

    #region Deconstruct Tests

    [Fact]
    public void Deconstruct_ReturnsAllValues()
    {
        var bounded = new BoundedFloat(100f, -50f, 25f);
        var (current, min, max) = bounded;
        Assert.Equal(25f, current);
        Assert.Equal(-50f, min);
        Assert.Equal(100f, max);
    }

    #endregion

    #region Construction Tests

    [Fact]
    public void Constructor_WithMaxOnly_SetsCurrentToMax()
    {
        var bounded = new BoundedFloat(100f);
        Assert.Equal(100f, bounded.Current);
        Assert.Equal(100f, bounded.Max);
        Assert.Equal(0f, bounded.Min);
    }

    [Fact]
    public void Constructor_WithMaxAndCurrent_SetsValues()
    {
        var bounded = new BoundedFloat(100f, 50f);
        Assert.Equal(50f, bounded.Current);
        Assert.Equal(100f, bounded.Max);
        Assert.Equal(0f, bounded.Min); // Min defaults to 0
    }

    [Fact]
    public void Constructor_WithMinMaxCurrent_SetsAllValues()
    {
        var bounded = new BoundedFloat(100f, -50f, 25f);
        Assert.Equal(25f, bounded.Current);
        Assert.Equal(100f, bounded.Max);
        Assert.Equal(-50f, bounded.Min);
    }

    [Fact]
    public void Constructor_ClampsToCeiling_WhenCurrentExceedsMax()
    {
        var bounded = new BoundedFloat(100f, 0f, 150f);
        Assert.Equal(100f, bounded.Current);
    }

    [Fact]
    public void Constructor_ClampsToFloor_WhenCurrentBelowMin()
    {
        var bounded = new BoundedFloat(100f, 0f, -10f);
        Assert.Equal(0f, bounded.Current);
    }

    [Fact]
    public void Constructor_SupportsNegativeMin()
    {
        var bounded = new BoundedFloat(50f, -50f, -25f);
        Assert.Equal(-25f, bounded.Current);
        Assert.Equal(-50f, bounded.Min);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenCurrentEqualsMax()
    {
        var bounded = new BoundedFloat(100f);
        Assert.True(bounded.IsFull());
    }

    [Fact]
    public void IsFull_ReturnsFalse_WhenCurrentLessThanMax()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        Assert.False(bounded.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenCurrentEqualsMin()
    {
        var bounded = new BoundedFloat(100f, 0f, 0f);
        Assert.True(bounded.IsEmpty());
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_WhenCurrentGreaterThanMin()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        Assert.False(bounded.IsEmpty());
    }

    #endregion

    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectRatio()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        Assert.Equal(0.5f, bounded.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsZero_WhenRangeIsZero()
    {
        var bounded = new BoundedFloat(0f, 0f, 0f);
        Assert.Equal(0.0f, bounded.GetRatio());
    }

    [Fact]
    public void GetRatio_WorksWithNegativeMin()
    {
        var bounded = new BoundedFloat(50f, -50f, 0f);
        Assert.Equal(0.5f, bounded.GetRatio(), 5); // 0 is 50% between -50 and 50
    }

    #endregion

    #region Arithmetic Tests

    [Fact]
    public void Addition_ClampsToMax()
    {
        var bounded = new BoundedFloat(100f, 0f, 90f);
        bounded = bounded + 20f;
        Assert.Equal(100f, bounded.Current);
    }

    [Fact]
    public void Subtraction_ClampsToMin()
    {
        var bounded = new BoundedFloat(100f, 0f, 10f);
        bounded = bounded - 20f;
        Assert.Equal(0f, bounded.Current);
    }

    [Fact]
    public void Increment_IncreasesBy1()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        bounded++;
        Assert.Equal(51f, bounded.Current);
    }

    [Fact]
    public void Decrement_DecreasesBy1()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        bounded--;
        Assert.Equal(49f, bounded.Current);
    }

    #endregion

    #region Normalize Tests

    [Fact]
    public void Normalize_ClampsExceedingValue()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        bounded.Current = 150f;
        bounded.Normalize();
        Assert.Equal(100f, bounded.Current);
    }

    [Fact]
    public void Normalize_ClampsNegativeValue()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        bounded.Current = -10f;
        bounded.Normalize();
        Assert.Equal(0f, bounded.Current);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalValues()
    {
        var a = new BoundedFloat(100f, 0f, 50f);
        var b = new BoundedFloat(100f, 0f, 50f);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentValues()
    {
        var a = new BoundedFloat(100f, 0f, 50f);
        var b = new BoundedFloat(100f, 0f, 60f);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    [Fact]
    public void GetHashCode_IsSame_ForEqualValues()
    {
        var a = new BoundedFloat(100f, 0f, 50f);
        var b = new BoundedFloat(100f, 0f, 50f);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    #endregion

    #region Comparison Tests

    [Fact]
    public void CompareTo_ReturnsNegative_WhenLess()
    {
        var a = new BoundedFloat(100f, 0f, 25f);
        var b = new BoundedFloat(100f, 0f, 75f);
        Assert.True(a.CompareTo(b) < 0);
        Assert.True(a < b);
    }

    [Fact]
    public void CompareTo_ReturnsPositive_WhenGreater()
    {
        var a = new BoundedFloat(100f, 0f, 75f);
        var b = new BoundedFloat(100f, 0f, 25f);
        Assert.True(a.CompareTo(b) > 0);
        Assert.True(a > b);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        Assert.Equal("50/100", bounded.ToString());
    }

    [Fact]
    public void ToString_WithRatioFormat_ReturnsPercentage()
    {
        var bounded = new BoundedFloat(100f, 0f, 50f);
        var result = bounded.ToString("R", CultureInfo.InvariantCulture);
        Assert.Contains("50", result);
    }

    #endregion
}