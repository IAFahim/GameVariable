namespace Variable.Curve.Tests;

public class CurveTests
{
    [Test]
    public void Linear_EvaluatesCorrectly()
    {
        var curve = CurveFloat.Linear(2f, 5f); // y = 2x + 5

        Assert.That(curve.Evaluate(0f), Is.EqualTo(5f));
        Assert.That(curve.Evaluate(1f), Is.EqualTo(7f));
        Assert.That(curve.Evaluate(10f), Is.EqualTo(25f));
        Assert.That(curve.Evaluate(-5f), Is.EqualTo(-5f));
    }

    [Test]
    public void Power_EvaluatesCorrectly()
    {
        var curve = CurveFloat.Power(2f, 2f, 10f); // y = 2 * x^2 + 10

        Assert.That(curve.Evaluate(0f), Is.EqualTo(10f));
        Assert.That(curve.Evaluate(1f), Is.EqualTo(12f)); // 2*1 + 10
        Assert.That(curve.Evaluate(2f), Is.EqualTo(18f)); // 2*4 + 10
        Assert.That(curve.Evaluate(3f), Is.EqualTo(28f)); // 2*9 + 10
    }

    [Test]
    public void Exponential_EvaluatesCorrectly()
    {
        var curve = CurveFloat.Exponential(2f, 3f, 1f); // y = 2 * 3^x + 1

        Assert.That(curve.Evaluate(0f), Is.EqualTo(3f)); // 2*1 + 1
        Assert.That(curve.Evaluate(1f), Is.EqualTo(7f)); // 2*3 + 1
        Assert.That(curve.Evaluate(2f), Is.EqualTo(19f)); // 2*9 + 1
    }

    [Test]
    public void Logarithmic_EvaluatesCorrectly()
    {
        // y = 10 * log2(x + 1) + 5
        var curve = CurveFloat.Logarithmic(10f, 2f, 5f);

        Assert.That(curve.Evaluate(0f), Is.EqualTo(5f)); // log2(1) = 0 -> 5
        Assert.That(curve.Evaluate(1f), Is.EqualTo(15f)); // log2(2) = 1 -> 10*1 + 5 = 15
        Assert.That(curve.Evaluate(3f), Is.EqualTo(25f)); // log2(4) = 2 -> 10*2 + 5 = 25
        Assert.That(curve.Evaluate(7f), Is.EqualTo(35f)); // log2(8) = 3 -> 10*3 + 5 = 35
    }

    [Test]
    public void Logarithmic_HandlesBase1_Safely()
    {
        var curve = CurveFloat.Logarithmic(10f, 1f, 5f);
        Assert.That(curve.Evaluate(10f), Is.EqualTo(5f)); // Should not NaN, just returns offset
    }

    [Test]
    public void Logistic_EvaluatesCorrectly()
    {
        // Max 10, Steepness 1, Midpoint 0, Offset 0
        // y = 10 / (1 + e^(-1 * (x - 0)))
        var curve = CurveFloat.Logistic(10f, 1f, 0f);

        Assert.That(curve.Evaluate(0f), Is.EqualTo(5f)); // At midpoint, value is Max/2
        Assert.That(curve.Evaluate(100f), Is.EqualTo(10f).Within(0.001f)); // Far right -> Max
        Assert.That(curve.Evaluate(-100f), Is.EqualTo(0f).Within(0.001f)); // Far left -> 0
    }

    [Test]
    public void Sine_EvaluatesCorrectly()
    {
        // y = 5 * sin(x * PI + 0) + 10
        var curve = CurveFloat.Sine(5f, MathF.PI, 0f, 10f);

        Assert.That(curve.Evaluate(0f), Is.EqualTo(10f).Within(0.001f)); // sin(0)=0 -> 10
        Assert.That(curve.Evaluate(0.5f), Is.EqualTo(15f).Within(0.001f)); // sin(PI/2)=1 -> 5+10=15
        Assert.That(curve.Evaluate(1f), Is.EqualTo(10f).Within(0.001f)); // sin(PI)=0 -> 10
        Assert.That(curve.Evaluate(1.5f), Is.EqualTo(5f).Within(0.001f)); // sin(3PI/2)=-1 -> -5+10=5
    }

    [Test]
    public void Equality_Works()
    {
        var c1 = CurveFloat.Linear(1f, 2f);
        var c2 = CurveFloat.Linear(1f, 2f);
        var c3 = CurveFloat.Linear(1f, 3f);

        Assert.That(c1, Is.EqualTo(c2));
        Assert.That(c1, Is.Not.EqualTo(c3));
        Assert.That(c1 == c2, Is.True);
        Assert.That(c1 != c3, Is.True);
    }

    [Test]
    public void ToString_FormatsCorrectly()
    {
        var c = CurveFloat.Linear(2.5f, 1.1f);
        Assert.That(c.ToString(), Does.Contain("Linear").And.Contain("2.5").And.Contain("1.1"));
    }
}
