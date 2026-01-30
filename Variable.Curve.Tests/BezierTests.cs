using Xunit;
using Variable.Curve;

namespace Variable.Curve.Tests;

public class BezierTests
{
    [Fact]
    public void Evaluate_Quad_StartAndEnd_AreCorrect()
    {
        // 0 -> P0 = 0
        Assert.Equal(0f, BezierLogic.Evaluate(0f, 0f, 1f, 2f));
        // 1 -> P2 = 2
        Assert.Equal(2f, BezierLogic.Evaluate(1f, 0f, 1f, 2f));
    }

    [Fact]
    public void Evaluate_Quad_MidPoint_IsCorrect()
    {
        // P0=0, P1=2, P2=0 (Parabola peak at 1)
        // (1-0.5)^2 * 0 + 2(1-0.5)(0.5)*2 + 0.5^2 * 0
        // 0.25*0 + 0.5*2 + 0.25*0 = 1
        Assert.Equal(1f, BezierLogic.Evaluate(0.5f, 0f, 2f, 0f));
    }

    [Fact]
    public void Evaluate_Cubic_StartAndEnd_AreCorrect()
    {
        // 0 -> P0 = 0
        Assert.Equal(0f, BezierLogic.Evaluate(0f, 0f, 1f, 2f, 3f));
        // 1 -> P3 = 3
        Assert.Equal(3f, BezierLogic.Evaluate(1f, 0f, 1f, 2f, 3f));
    }

    [Fact]
    public void Evaluate_Cubic_MidPoint_IsCorrect()
    {
        // P0=0, P1=0, P2=1, P3=1
        // (1-0.5)^3 * 0 + 3(1-0.5)^2(0.5)*0 + 3(1-0.5)(0.5)^2 * 1 + 0.5^3 * 1
        // 0 + 0 + 3(0.5)(0.25)*1 + 0.125
        // 0.375 + 0.125 = 0.5
        Assert.Equal(0.5f, BezierLogic.Evaluate(0.5f, 0f, 0f, 1f, 1f));
    }

    [Fact]
    public void EvaluateDerivative_Quad_IsCorrect()
    {
        // P0=0, P1=1, P2=0
        // Derivative at 0.5 (peak) should be 0.
        // 2(1-t)(P1-P0) + 2t(P2-P1)
        // 2(0.5)(1-0) + 2(0.5)(0-1)
        // 1(1) + 1(-1) = 0
        Assert.Equal(0f, BezierLogic.EvaluateDerivative(0.5f, 0f, 1f, 0f));
    }
}
