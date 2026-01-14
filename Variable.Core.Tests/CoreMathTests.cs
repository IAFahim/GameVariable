using Variable.Core;
using Xunit;

namespace Variable.Core.Tests;

public class CoreMathTests
{
    #region Clamp Tests

    [Fact]
    public void Clamp_Float_Works()
    {
        CoreMath.Clamp(10f, 0f, 5f, out var result);
        Assert.Equal(5f, result);

        CoreMath.Clamp(-10f, 0f, 5f, out result);
        Assert.Equal(0f, result);

        CoreMath.Clamp(2.5f, 0f, 5f, out result);
        Assert.Equal(2.5f, result);
    }

    [Fact]
    public void Clamp_Int_Works()
    {
        CoreMath.Clamp(10, 0, 5, out var result);
        Assert.Equal(5, result);

        CoreMath.Clamp(-10, 0, 5, out result);
        Assert.Equal(0, result);

        CoreMath.Clamp(2, 0, 5, out result);
        Assert.Equal(2, result);
    }

    [Fact]
    public void Clamp_Long_Works()
    {
        CoreMath.Clamp(10L, 0L, 5L, out var result);
        Assert.Equal(5L, result);

        CoreMath.Clamp(-10L, 0L, 5L, out result);
        Assert.Equal(0L, result);

        CoreMath.Clamp(2L, 0L, 5L, out result);
        Assert.Equal(2L, result);
    }

    [Fact]
    public void Clamp_Byte_Max_Works()
    {
        CoreMath.Clamp((byte)10, (byte)5, out var result);
        Assert.Equal((byte)5, result);

        CoreMath.Clamp((byte)2, (byte)5, out result);
        Assert.Equal((byte)2, result);
    }

    [Fact]
    public void Clamp_Int_To_Byte_Works()
    {
        CoreMath.Clamp(300, (byte)255, out var result);
        Assert.Equal((byte)255, result);

        CoreMath.Clamp(-1, (byte)255, out result);
        Assert.Equal((byte)0, result);

        CoreMath.Clamp(100, (byte)255, out result);
        Assert.Equal((byte)100, result);
    }

    [Fact]
    public void Clamp_Double_Works()
    {
        CoreMath.Clamp(10.0, 0.0, 5.0, out var result);
        Assert.Equal(5.0, result);

        CoreMath.Clamp(-10.0, 0.0, 5.0, out result);
        Assert.Equal(0.0, result);

        CoreMath.Clamp(2.5, 0.0, 5.0, out result);
        Assert.Equal(2.5, result);
    }

    #endregion

    #region Min Tests

    [Fact]
    public void Min_Float_Works()
    {
        CoreMath.Min(1f, 2f, out var result);
        Assert.Equal(1f, result);

        CoreMath.Min(2f, 1f, out result);
        Assert.Equal(1f, result);
    }

    [Fact]
    public void Min_Int_Works()
    {
        CoreMath.Min(1, 2, out var result);
        Assert.Equal(1, result);

        CoreMath.Min(2, 1, out result);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Min_Long_Works()
    {
        CoreMath.Min(1L, 2L, out var result);
        Assert.Equal(1L, result);

        CoreMath.Min(2L, 1L, out result);
        Assert.Equal(1L, result);
    }

    #endregion

    #region Max Tests

    [Fact]
    public void Max_Float_Works()
    {
        CoreMath.Max(1f, 2f, out var result);
        Assert.Equal(2f, result);

        CoreMath.Max(2f, 1f, out result);
        Assert.Equal(2f, result);
    }

    [Fact]
    public void Max_Int_Works()
    {
        CoreMath.Max(1, 2, out var result);
        Assert.Equal(2, result);

        CoreMath.Max(2, 1, out result);
        Assert.Equal(2, result);
    }

    [Fact]
    public void Max_Long_Works()
    {
        CoreMath.Max(1L, 2L, out var result);
        Assert.Equal(2L, result);

        CoreMath.Max(2L, 1L, out result);
        Assert.Equal(2L, result);
    }

    #endregion

    #region Abs Tests

    [Fact]
    public void Abs_Float_Works()
    {
        CoreMath.Abs(-1f, out var result);
        Assert.Equal(1f, result);

        CoreMath.Abs(1f, out result);
        Assert.Equal(1f, result);
    }

    [Fact]
    public void Abs_Int_Works()
    {
        CoreMath.Abs(-1, out var result);
        Assert.Equal(1, result);

        CoreMath.Abs(1, out result);
        Assert.Equal(1, result);
    }

    #endregion
}
