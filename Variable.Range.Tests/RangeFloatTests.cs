namespace Variable.Range.Tests;

public class RangeFloatTests
{
    #region Construction Tests

    [Fact]
    public void Constructor_WithMinMax_SetsCurrentToMin()
    {
        var range = new RangeFloat(0f, 100f);
        Assert.Equal(0f, range.Current);
        Assert.Equal(0f, range.Min);
        Assert.Equal(100f, range.Max);
    }

    [Fact]
    public void Constructor_WithAllValues_SetsCorrectly()
    {
        var range = new RangeFloat(-50f, 50f, 25f);
        Assert.Equal(25f, range.Current);
        Assert.Equal(-50f, range.Min);
        Assert.Equal(50f, range.Max);
    }

    [Fact]
    public void Constructor_ClampsCurrent_WhenExceedsMax()
    {
        var range = new RangeFloat(0f, 100f, 150f);
        Assert.Equal(100f, range.Current);
    }

    [Fact]
    public void Constructor_ClampsCurrent_WhenBelowMin()
    {
        var range = new RangeFloat(0f, 100f, -50f);
        Assert.Equal(0f, range.Current);
    }

    [Fact]
    public void Constructor_SupportsNegativeRange()
    {
        var range = new RangeFloat(-100f, -50f, -75f);
        Assert.Equal(-75f, range.Current);
        Assert.Equal(-100f, range.Min);
        Assert.Equal(-50f, range.Max);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenAtMax()
    {
        var range = new RangeFloat(0f, 100f, 100f);
        Assert.True(range.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenAtMin()
    {
        var range = new RangeFloat(0f, 100f, 0f);
        Assert.True(range.IsEmpty());
    }

    [Fact]
    public void IsEmpty_WorksWithNegativeMin()
    {
        var range = new RangeFloat(-50f, 50f, -50f);
        Assert.True(range.IsEmpty());
    }

    #endregion

    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectValue_StandardRange()
    {
        var range = new RangeFloat(0f, 100f, 50f);
        Assert.Equal(0.5, range.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsCorrectValue_NegativeRange()
    {
        var range = new RangeFloat(-100f, 100f, 0f);
        Assert.Equal(0.5, range.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsZero_WhenRangeIsZero()
    {
        var range = new RangeFloat(50f, 50f, 50f);
        Assert.Equal(0.0, range.GetRatio());
    }

    #endregion

    #region Arithmetic Tests

    [Fact]
    public void Addition_ClampsToMax()
    {
        var range = new RangeFloat(0f, 100f, 90f);
        range = range + 20f;
        Assert.Equal(100f, range.Current);
    }

    [Fact]
    public void Subtraction_ClampsToMin()
    {
        var range = new RangeFloat(0f, 100f, 10f);
        range = range - 20f;
        Assert.Equal(0f, range.Current);
    }

    [Fact]
    public void Increment_IncreasesBy1()
    {
        var range = new RangeFloat(0f, 100f, 50f);
        range++;
        Assert.Equal(51f, range.Current);
    }

    [Fact]
    public void Decrement_DecreasesBy1()
    {
        var range = new RangeFloat(0f, 100f, 50f);
        range--;
        Assert.Equal(49f, range.Current);
    }

    #endregion

    #region Normalize Tests

    [Fact]
    public void Normalize_ClampsExceedingValue()
    {
        var range = new RangeFloat(0f, 100f, 50f);
        range.Current = 150f;
        range.Normalize();
        Assert.Equal(100f, range.Current);
    }

    [Fact]
    public void Normalize_ClampsValueBelowMin()
    {
        var range = new RangeFloat(0f, 100f, 50f);
        range.Current = -10f;
        range.Normalize();
        Assert.Equal(0f, range.Current);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalRanges()
    {
        var a = new RangeFloat(0f, 100f, 50f);
        var b = new RangeFloat(0f, 100f, 50f);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentRanges()
    {
        var a = new RangeFloat(0f, 100f, 50f);
        var b = new RangeFloat(0f, 100f, 60f);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    #endregion

    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_ToFloat_ReturnsCurrent()
    {
        var range = new RangeFloat(0f, 100f, 42f);
        float value = range;
        Assert.Equal(42f, value);
    }

    #endregion

    #region Deconstruct Tests

    [Fact]
    public void Deconstruct_ReturnsAllValues()
    {
        var range = new RangeFloat(-50f, 100f, 25f);
        var (current, min, max) = range;
        Assert.Equal(25f, current);
        Assert.Equal(-50f, min);
        Assert.Equal(100f, max);
    }

    #endregion

    #region Temperature Use Case

    [Fact]
    public void Temperature_NegativeToPositiveRange()
    {
        // Simulate temperature from -40°C to 50°C
        var temp = new RangeFloat(-40f, 50f, 22f);
        
        Assert.Equal(22f, temp.Current);
        Assert.Equal(-40f, temp.Min);
        Assert.Equal(50f, temp.Max);
        
        // 22°C is about 68.9% of the way from -40 to 50
        var expectedRatio = (22f - (-40f)) / (50f - (-40f));
        Assert.Equal(expectedRatio, (float)temp.GetRatio(), 4);
    }

    #endregion
}
