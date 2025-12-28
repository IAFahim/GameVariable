using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_Calculate
{
    [Theory]
    [InlineData(10f, 0f, 0f, 1f, 10f)] // Base only
    [InlineData(10f, 5f, 0f, 1f, 15f)] // Base + Flat
    [InlineData(10f, 0f, 0.5f, 1f, 15f)] // Base + 50% Add
    [InlineData(10f, 0f, 0f, 2f, 20f)] // Base * 2 Mult
    [InlineData(10f, 10f, 0.5f, 2f, 60f)] // (10+10) * 1.5 * 2 = 60
    [InlineData(10f, 10f, -0.5f, 1f, 10f)] // (10+10) * 0.5 = 10
    public void Calculate_Float_Tests(float baseVal, float flat, float pctAdd, float pctMult, float expected)
    {
        Assert.Equal(expected, StatLogic.Calculate(baseVal, flat, pctAdd, pctMult));
    }
}
