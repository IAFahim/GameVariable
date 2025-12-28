namespace Variable.Stat.Tests;

public class StatLogicTests_Set
{
    [Fact]
    public void Set_UpdatesValue()
    {
        var current = 10f;
        StatLogic.Set(ref current, 15f, 20f);
        Assert.Equal(15f, current);
    }

    [Fact]
    public void Set_ClampsToMax()
    {
        var current = 10f;
        StatLogic.Set(ref current, 25f, 20f);
        Assert.Equal(20f, current);
    }

    [Fact]
    public void Set_ClampsToMin()
    {
        var current = 10f;
        StatLogic.Set(ref current, -5f, 20f);
        Assert.Equal(0f, current);
    }
}