namespace Variable.Stat.Tests;

public class StatLogicTests_Subtract
{
    [Fact]
    public void Subtract_DecreasesValue()
    {
        var current = 10f;
        var removed = StatLogic.Subtract(ref current, 5f);
        Assert.Equal(5f, current);
        Assert.Equal(5f, removed);
    }

    [Fact]
    public void Subtract_ClampsToMin()
    {
        var current = 2f;
        var removed = StatLogic.Subtract(ref current, 5f);
        Assert.Equal(0f, current);
        Assert.Equal(2f, removed);
    }

    [Fact]
    public void Subtract_WithCustomMin()
    {
        var current = 12f;
        var removed = StatLogic.Subtract(ref current, 5f, 10f);
        Assert.Equal(10f, current);
        Assert.Equal(2f, removed);
    }
}