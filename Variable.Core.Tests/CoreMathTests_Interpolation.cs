using Variable.Core;
using Xunit;

namespace Variable.Core.Tests;

public class CoreMathTests_Interpolation
{
    [Fact]
    public void Lerp_Works()
    {
        // 0.5 of 0-10 is 5
        CoreMath.Lerp(0f, 10f, 0.5f, out var result);
        Assert.Equal(5f, result);

        // 0 of 0-10 is 0
        CoreMath.Lerp(0f, 10f, 0f, out result);
        Assert.Equal(0f, result);

        // 1 of 0-10 is 10
        CoreMath.Lerp(0f, 10f, 1f, out result);
        Assert.Equal(10f, result);

        // Unclamped: 2 of 0-10 is 20
        CoreMath.Lerp(0f, 10f, 2f, out result);
        Assert.Equal(20f, result);

        // Negative start
        CoreMath.Lerp(-10f, 10f, 0.5f, out result);
        Assert.Equal(0f, result);
    }

    [Fact]
    public void InverseLerp_Works()
    {
        // 5 is 0.5 of 0-10
        CoreMath.InverseLerp(0f, 10f, 5f, out var result);
        Assert.Equal(0.5f, result);

        // 0 is 0
        CoreMath.InverseLerp(0f, 10f, 0f, out result);
        Assert.Equal(0f, result);

        // 10 is 1
        CoreMath.InverseLerp(0f, 10f, 10f, out result);
        Assert.Equal(1f, result);

        // Unclamped: 20 is 2
        CoreMath.InverseLerp(0f, 10f, 20f, out result);
        Assert.Equal(2f, result);

        // Zero length range
        CoreMath.InverseLerp(10f, 10f, 5f, out result);
        Assert.Equal(0f, result);
    }

    [Fact]
    public void Remap_Works()
    {
        // 5 in 0-10 -> 0.5 -> 50 in 0-100
        CoreMath.Remap(5f, 0f, 10f, 0f, 100f, out var result);
        Assert.Equal(50f, result);

        // Unclamped extrapolation
        // 15 in 0-10 -> 1.5 -> 150 in 0-100
        CoreMath.Remap(15f, 0f, 10f, 0f, 100f, out result);
        Assert.Equal(150f, result);

        // Inverse range
        // 5 in 0-10 -> 0.5 -> 50 in 100-0
        CoreMath.Remap(5f, 0f, 10f, 100f, 0f, out result);
        Assert.Equal(50f, result);
    }
}
