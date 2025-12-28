namespace Variable.Stat.Tests;

public class StatLogicTests_IsMin
{
    [Fact]
    public void IsMin_ReturnsTrue_WhenAtMin()
    {
        Assert.True(StatLogic.IsMin(0f));
    }

    [Fact]
    public void IsMin_ReturnsTrue_WhenBelowMin()
    {
        Assert.True(StatLogic.IsMin(-1f));
    }

    [Fact]
    public void IsMin_ReturnsFalse_WhenAboveMin()
    {
        Assert.False(StatLogic.IsMin(1f));
    }
}