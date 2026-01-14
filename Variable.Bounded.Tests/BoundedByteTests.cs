using System.Globalization;
using Variable.Bounded;
using Xunit;

namespace Variable.Bounded.Tests;

public class BoundedByteTests
{
    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_ToByte_ReturnsCurrent()
    {
        var bounded = new BoundedByte(100, 42);
        byte value = bounded;
        Assert.Equal((byte)42, value);
    }

    #endregion

    #region Construction Tests

    [Fact]
    public void Constructor_WithMaxOnly_SetsCurrentToMax()
    {
        var bounded = new BoundedByte(100);
        Assert.Equal(100, bounded.Current);
        Assert.Equal(100, bounded.Max);
    }

    [Fact]
    public void Constructor_WithMaxAndCurrent_SetsValues()
    {
        var bounded = new BoundedByte(100, 50);
        Assert.Equal(50, bounded.Current);
        Assert.Equal(100, bounded.Max);
    }

    [Fact]
    public void Constructor_WithIntCurrent_Clamps()
    {
        var bounded = new BoundedByte(100, 150);
        Assert.Equal(100, bounded.Current);

        var bounded2 = new BoundedByte(100, -10);
        Assert.Equal(0, bounded2.Current);
    }

    #endregion

    #region State Tests

    [Fact]
    public void IsFull_ReturnsTrue_WhenCurrentEqualsMax()
    {
        var bounded = new BoundedByte(100);
        Assert.True(bounded.IsFull());
    }

    [Fact]
    public void IsFull_ReturnsFalse_WhenCurrentLessThanMax()
    {
        var bounded = new BoundedByte(100, 50);
        Assert.False(bounded.IsFull());
    }

    [Fact]
    public void IsEmpty_ReturnsTrue_WhenCurrentEqualsZero()
    {
        var bounded = new BoundedByte(100, 0);
        Assert.True(bounded.IsEmpty());
    }

    [Fact]
    public void IsEmpty_ReturnsFalse_WhenCurrentGreaterThanZero()
    {
        var bounded = new BoundedByte(100, 50);
        Assert.False(bounded.IsEmpty());
    }

    #endregion

    #region Ratio Tests

    [Fact]
    public void GetRatio_ReturnsCorrectRatio()
    {
        var bounded = new BoundedByte(100, 50);
        Assert.Equal(0.5, bounded.GetRatio(), 5);
    }

    [Fact]
    public void GetRatio_ReturnsZero_WhenMaxIsZero()
    {
        var bounded = new BoundedByte(0, 0);
        Assert.Equal(0.0, bounded.GetRatio());
    }

    #endregion

    #region Arithmetic Tests

    [Fact]
    public void Addition_ClampsToMax()
    {
        var bounded = new BoundedByte(100, 90);
        bounded = bounded + 20;
        Assert.Equal(100, bounded.Current);
    }

    [Fact]
    public void Addition_IntAndBounded_ClampsToMax()
    {
        var bounded = new BoundedByte(100, 90);
        bounded = 20 + bounded;
        Assert.Equal(100, bounded.Current);
    }

    [Fact]
    public void Subtraction_ClampsToMin()
    {
        var bounded = new BoundedByte(100, 10);
        bounded = bounded - 20;
        Assert.Equal(0, bounded.Current);
    }

    [Fact]
    public void Increment_IncreasesBy1()
    {
        var bounded = new BoundedByte(100, 50);
        bounded++;
        Assert.Equal(51, bounded.Current);
    }

    [Fact]
    public void Decrement_DecreasesBy1()
    {
        var bounded = new BoundedByte(100, 50);
        bounded--;
        Assert.Equal(49, bounded.Current);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_ReturnsTrue_ForIdenticalValues()
    {
        var a = new BoundedByte(100, 50);
        var b = new BoundedByte(100, 50);
        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Equals_ReturnsFalse_ForDifferentValues()
    {
        var a = new BoundedByte(100, 50);
        var b = new BoundedByte(100, 60);
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    [Fact]
    public void GetHashCode_IsSame_ForEqualValues()
    {
        var a = new BoundedByte(100, 50);
        var b = new BoundedByte(100, 50);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    #endregion

    #region Comparison Tests

    [Fact]
    public void CompareTo_ReturnsNegative_WhenLess()
    {
        var a = new BoundedByte(100, 25);
        var b = new BoundedByte(100, 75);
        Assert.True(a.CompareTo(b) < 0);
        Assert.True(a < b);
        Assert.True(a <= b);
    }

    [Fact]
    public void CompareTo_ReturnsPositive_WhenGreater()
    {
        var a = new BoundedByte(100, 75);
        var b = new BoundedByte(100, 25);
        Assert.True(a.CompareTo(b) > 0);
        Assert.True(a > b);
        Assert.True(a >= b);
    }

    [Fact]
    public void CompareTo_Object_Works()
    {
        var a = new BoundedByte(100, 50);
        object b = new BoundedByte(100, 50);
        Assert.Equal(0, a.CompareTo(b));
    }

    [Fact]
    public void CompareTo_Null_Throws()
    {
        var a = new BoundedByte(100, 50);
        Assert.Throws<ArgumentException>(() => a.CompareTo(new object()));
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var bounded = new BoundedByte(100, 50);
        Assert.Equal("50/100", bounded.ToString());
    }

    #endregion

    #region Extension Tests

    [Fact]
    public void Set_UpdatesValue()
    {
        var bounded = new BoundedByte(100, 0);
        bounded.Set(50);
        Assert.Equal(50, bounded.Current);
    }

    [Fact]
    public void Normalize_ClampsValue()
    {
        var bounded = new BoundedByte(100, 50);
        bounded.Current = 150;
        bounded.Normalize();
        Assert.Equal(100, bounded.Current);
    }

    [Fact]
    public void GetRange_ReturnsMax()
    {
        var bounded = new BoundedByte(100, 50);
        Assert.Equal(100, bounded.GetRange());
    }

    [Fact]
    public void GetRemaining_ReturnsCorrectValue()
    {
        var bounded = new BoundedByte(100, 60);
        Assert.Equal(40, bounded.GetRemaining());
    }

    [Fact]
    public void TryConsume_Works()
    {
        var bounded = new BoundedByte(100, 50);
        Assert.True(bounded.TryConsume(20));
        Assert.Equal(30, bounded.Current);
        Assert.False(bounded.TryConsume(40));
        Assert.Equal(30, bounded.Current);
    }

    #endregion
}
