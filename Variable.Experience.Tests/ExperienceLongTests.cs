using System.Globalization;
using Variable.Experience;
using Xunit;

namespace Variable.Experience.Tests;

public class ExperienceLongTests
{
    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_ToLong_ReturnsCurrent()
    {
        var exp = new ExperienceLong(100, 42);
        long value = exp;
        Assert.Equal(42L, value);
    }

    #endregion

    #region Deconstruct Tests

    [Fact]
    public void Deconstruct_ReturnsAllValues()
    {
        var exp = new ExperienceLong(100, 50, 5);
        var (current, max, level) = exp;
        Assert.Equal(50L, current);
        Assert.Equal(100L, max);
        Assert.Equal(5, level);
    }

    #endregion

    #region Construction Tests

    [Fact]
    public void Constructor_WithMax_SetsDefaults()
    {
        var exp = new ExperienceLong(1000);
        Assert.Equal(0L, exp.Current);
        Assert.Equal(1000L, exp.Max);
        Assert.Equal(1, exp.Level);
    }

    [Fact]
    public void Constructor_WithAllValues_SetsCorrectly()
    {
        var exp = new ExperienceLong(500, 250, 5);
        Assert.Equal(250L, exp.Current);
        Assert.Equal(500L, exp.Max);
        Assert.Equal(5, exp.Level);
    }

    [Fact]
    public void Constructor_EnsuresMinMax()
    {
        var exp = new ExperienceLong(0); // Max < 1 should be clamped to 1
        Assert.Equal(1L, exp.Max);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenMaxedXP()
    {
        var exp = new ExperienceLong(100, 100);
        Assert.True(exp.IsFull());
    }

    [Fact]
    public void IsFull_ReturnsFalse_WhenNotMaxed()
    {
        var exp = new ExperienceLong(100, 50);
        Assert.False(exp.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenZeroXP()
    {
        var exp = new ExperienceLong(100);
        Assert.True(exp.IsEmpty());
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_WhenHasXP()
    {
        var exp = new ExperienceLong(100, 50);
        Assert.False(exp.IsEmpty());
    }

    #endregion

    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectValue()
    {
        var exp = new ExperienceLong(100, 50);
        Assert.Equal(0.5f, exp.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsZero_WhenMaxIsZero()
    {
        // This case is actually hard to reach via constructor due to clamping, but logic handles it
        var exp = new ExperienceLong(1);
        exp.Max = 0; // Force set
        Assert.Equal(0.0f, exp.GetRatio());
    }

    #endregion

    #region Arithmetic Tests

    [Fact]
    public void Addition_IncreasesXP()
    {
        var exp = new ExperienceLong(100, 50);
        exp = exp + 25L;
        Assert.Equal(75L, exp.Current);
    }

    [Fact]
    public void Addition_CanExceedMax()
    {
        var exp = new ExperienceLong(100, 50);
        exp = exp + 100L;
        Assert.Equal(150L, exp.Current);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalExperience()
    {
        var a = new ExperienceLong(100, 50, 5);
        var b = new ExperienceLong(100, 50, 5);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentLevel()
    {
        var a = new ExperienceLong(100, 50, 5);
        var b = new ExperienceLong(100, 50, 6);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentXP()
    {
        var a = new ExperienceLong(100, 50, 5);
        var b = new ExperienceLong(100, 60, 5);
        Assert.False(a.Equals(b));
    }

    #endregion

    #region Comparison Tests

    [Fact]
    public void CompareTo_ComparesLevelFirst()
    {
        var lowLevel = new ExperienceLong(100, 99, 5);
        var highLevel = new ExperienceLong(100, 1, 6);
        Assert.True(lowLevel < highLevel);
        Assert.True(lowLevel <= highLevel);
    }

    [Fact]
    public void CompareTo_ComparesCurrent_WhenSameLevel()
    {
        var lessXP = new ExperienceLong(100, 50, 5);
        var moreXP = new ExperienceLong(100, 75, 5);
        Assert.True(lessXP < moreXP);
    }

    [Fact]
    public void CompareTo_ComparesMax_WhenSameLevelAndCurrent()
    {
        var exp1 = new ExperienceLong(100, 50, 5);
        var exp2 = new ExperienceLong(200, 50, 5);
        Assert.True(exp1 < exp2);
    }

    [Fact]
    public void CompareTo_Object_Works()
    {
        var a = new ExperienceLong(100, 50, 5);
        object b = new ExperienceLong(100, 50, 5);
        Assert.Equal(0, a.CompareTo(b));
    }

    [Fact]
    public void CompareTo_Null_Throws()
    {
        var a = new ExperienceLong(100, 50, 5);
        Assert.Throws<ArgumentException>(() => a.CompareTo(new object()));
    }

    [Fact]
    public void Operators_Work()
    {
        var a = new ExperienceLong(100, 50, 5);
        var b = new ExperienceLong(100, 75, 5);
        
        Assert.True(a < b);
        Assert.True(a <= b);
        Assert.True(b > a);
        Assert.True(b >= a);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var exp = new ExperienceLong(100, 50, 5);
        Assert.Equal("Lvl 5 (50/100)", exp.ToString());
    }

    #endregion

    #region Logic Tests

    private readonly struct LinearLongFormula : INextMaxFormula<long>
    {
        public long Calculate(int level) => level * 1000L;
    }

    [Fact]
    public void Add_WithFormula_LevelsUp()
    {
        var xp = new ExperienceLong(1000, 0, 1);
        var levels = xp.Add(1500, new LinearLongFormula()); // Should reach level 2 and have 500/2000

        Assert.Equal(1, levels);
        Assert.Equal(2, xp.Level);
        Assert.Equal(500L, xp.Current);
        Assert.Equal(2000L, xp.Max);
    }

    [Fact]
    public void Add_WithFormula_MultiLevelUp()
    {
        var xp = new ExperienceLong(1000, 0, 1);
        // Add 4000 XP
        // Lvl 1: 1000 needed (rem: 3000)
        // Lvl 2: 2000 needed (rem: 1000)
        // Lvl 3: 3000 needed (has 1000)
        var levels = xp.Add(4000, new LinearLongFormula());

        Assert.Equal(2, levels);
        Assert.Equal(3, xp.Level);
        Assert.Equal(1000L, xp.Current);
        Assert.Equal(3000L, xp.Max);
    }

    #endregion
}
