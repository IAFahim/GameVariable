namespace Variable.Stat.Tests;

public class StatLogicTests_CalculateInt
{
    [Theory]
    [InlineData(10f, 0f, 0f, 1f, 10)]
    [InlineData(10f, 0.6f, 0f, 1f, 11)] // 10.6 -> 11
    [InlineData(10f, 0.4f, 0f, 1f, 10)] // 10.4 -> 10
    [InlineData(10f, 10f, 0.5f, 2f, 60)] // (10+10) * 1.5 * 2 = 60
    public void CalculateInt_Tests(float baseVal, float flat, float pctAdd, float pctMult, int expected)
    {
        StatLogic.CalculateInt(baseVal, flat, pctAdd, pctMult, out var result);
        Assert.Equal(expected, result);
    }
}