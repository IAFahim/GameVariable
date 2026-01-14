using System.Globalization;

namespace Variable.Experience.Tests;

public class ExperienceIntTests
{
    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_ToInt_ReturnsCurrent()
    {
        var exp = new ExperienceInt(100, 42);
        int value = exp;
        Assert.Equal(42, value);
    }

    #endregion

    #region Deconstruct Tests

    [Fact]
    public void Deconstruct_ReturnsAllValues()
    {
        var exp = new ExperienceInt(100, 50, 5);
        var (current, max, level) = exp;
        Assert.Equal(50, current);
        Assert.Equal(100, max);
        Assert.Equal(5, level);
    }

    #endregion

    #region Construction Tests

    [Fact]
    public void Constructor_WithMax_SetsDefaults()
    {
        var exp = new ExperienceInt(1000);
        Assert.Equal(0, exp.Current);
        Assert.Equal(1000, exp.Max);
        Assert.Equal(1, exp.Level);
    }

    [Fact]
    public void Constructor_WithAllValues_SetsCorrectly()
    {
        var exp = new ExperienceInt(500, 250, 5);
        Assert.Equal(250, exp.Current);
        Assert.Equal(500, exp.Max);
        Assert.Equal(5, exp.Level);
    }

    [Fact]
    public void Constructor_EnsuresMinMax()
    {
        var exp = new ExperienceInt(0); // Max < 1 should be clamped to 1
        Assert.Equal(1, exp.Max);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenMaxedXP()
    {
        var exp = new ExperienceInt(100, 100);
        Assert.True(exp.IsFull());
    }

    [Fact]
    public void IsFull_ReturnsFalse_WhenNotMaxed()
    {
        var exp = new ExperienceInt(100, 50);
        Assert.False(exp.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenZeroXP()
    {
        var exp = new ExperienceInt(100);
        Assert.True(exp.IsEmpty());
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_WhenHasXP()
    {
        var exp = new ExperienceInt(100, 50);
        Assert.False(exp.IsEmpty());
    }

    #endregion

    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectValue()
    {
        var exp = new ExperienceInt(100, 50);
        Assert.Equal(0.5, exp.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsZero_WhenMaxIsZero()
    {
        var exp = new ExperienceInt(0);
        Assert.Equal(0.0, exp.GetRatio());
    }

    #endregion

    #region Arithmetic Tests

    [Fact]
    public void Addition_IncreasesXP()
    {
        var exp = new ExperienceInt(100, 50);
        exp = exp + 25;
        Assert.Equal(75, exp.Current);
    }

    [Fact]
    public void Addition_CanExceedMax()
    {
        // Note: ExperienceInt doesn't automatically level up - that's left to extension methods
        var exp = new ExperienceInt(100, 50);
        exp = exp + 100;
        Assert.Equal(150, exp.Current);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalExperience()
    {
        var a = new ExperienceInt(100, 50, 5);
        var b = new ExperienceInt(100, 50, 5);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentLevel()
    {
        var a = new ExperienceInt(100, 50, 5);
        var b = new ExperienceInt(100, 50, 6);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentXP()
    {
        var a = new ExperienceInt(100, 50, 5);
        var b = new ExperienceInt(100, 60, 5);
        Assert.False(a.Equals(b));
    }

    #endregion

    #region Comparison Tests

    [Fact]
    public void CompareTo_ComparesLevelFirst()
    {
        var lowLevel = new ExperienceInt(100, 99, 5);
        var highLevel = new ExperienceInt(100, 1, 6);
        Assert.True(lowLevel < highLevel);
    }

    [Fact]
    public void CompareTo_ComparesCurrent_WhenSameLevel()
    {
        var lessXP = new ExperienceInt(100, 50, 5);
        var moreXP = new ExperienceInt(100, 75, 5);
        Assert.True(lessXP < moreXP);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var exp = new ExperienceInt(100, 50, 5);
        Assert.Equal("Lvl 5 (50/100)", exp.ToString());
    }

    [Fact]
    public void ToString_LevelFormat_ReturnsLevelOnly()
    {
        var exp = new ExperienceInt(100, 50, 5);
        Assert.Equal("5", exp.ToString("L", CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ToString_CurrentFormat_ReturnsXPFraction()
    {
        var exp = new ExperienceInt(100, 50, 5);
        Assert.Equal("50/100", exp.ToString("C", CultureInfo.InvariantCulture));
    }

    #endregion
}