using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_CalculateDouble
{
    [Theory]
    [InlineData(10d, 0d, 0d, 1d, 10d)]
    [InlineData(10d, 5d, 0d, 1d, 15d)]
    [InlineData(10d, 10d, 0.5d, 2d, 60d)]
    public void Calculate_Double_Tests(double baseVal, double flat, double pctAdd, double pctMult, double expected)
    {
        Assert.Equal(expected, StatLogic.Calculate(baseVal, flat, pctAdd, pctMult));
    }
}
