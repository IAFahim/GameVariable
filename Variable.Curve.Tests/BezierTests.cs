using Variable.Curve;
using Xunit;

namespace Variable.Curve.Tests;

public class BezierTests
{
    [Fact]
    public void Evaluate_StartAndEnd_AreExact()
    {
        var curve = new BezierCurve(0f, 10f, 10f, 100f);

        Assert.Equal(0f, curve.Evaluate(0f));
        Assert.Equal(100f, curve.Evaluate(1f));
    }

    [Fact]
    public void Evaluate_Linear_IsLinear()
    {
        // P0=0, P3=10.
        // P1=3.333, P2=6.666

        var curve = new BezierCurve(0f, 3.333333f, 6.666666f, 10f);

        float mid = curve.Evaluate(0.5f);
        Assert.Equal(5f, mid, 3); // Check with precision
    }

    [Fact]
    public void Evaluate_Curve_Works()
    {
        // P0=0, P1=10, P2=10, P3=0 (Hill)
        var curve = new BezierCurve(0f, 10f, 10f, 0f);

        float mid = curve.Evaluate(0.5f);
        // At 0.5:
        // u=0.5, t=0.5.
        // uuu*0 + 3*uu*t*10 + 3*u*tt*10 + ttt*0
        // 3 * 0.125 * 10 + 3 * 0.125 * 10
        // 3.75 + 3.75 = 7.5
        Assert.Equal(7.5f, mid);
    }
}
