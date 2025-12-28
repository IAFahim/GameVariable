using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests
{
    public class StatLogicTests
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

        [Theory]
        [InlineData(10f, 0f, 0f, 1f, 10)]
        [InlineData(10f, 0.6f, 0f, 1f, 11)] // 10.6 -> 11
        [InlineData(10f, 0.4f, 0f, 1f, 10)] // 10.4 -> 10
        [InlineData(10f, 10f, 0.5f, 2f, 60)] // (10+10) * 1.5 * 2 = 60
        public void CalculateInt_Tests(float baseVal, float flat, float pctAdd, float pctMult, int expected)
        {
            Assert.Equal(expected, StatLogic.CalculateInt(baseVal, flat, pctAdd, pctMult));
        }

        [Theory]
        [InlineData(10d, 0d, 0d, 1d, 10d)]
        [InlineData(10d, 5d, 0d, 1d, 15d)]
        [InlineData(10d, 10d, 0.5d, 2d, 60d)]
        public void Calculate_Double_Tests(double baseVal, double flat, double pctAdd, double pctMult, double expected)
        {
            Assert.Equal(expected, StatLogic.Calculate(baseVal, flat, pctAdd, pctMult));
        }
    }
}
