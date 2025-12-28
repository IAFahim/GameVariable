namespace Variable.Stat.Tests;

public class StatLogicTests_Transfer
{
    [Fact]
    public void Transfer_MovesValue()
    {
        var source = 10f;
        var target = 5f;
        var transferred = StatLogic.Transfer(ref source, ref target, 20f, 5f);

        Assert.Equal(5f, source);
        Assert.Equal(10f, target);
        Assert.Equal(5f, transferred);
    }

    [Fact]
    public void Transfer_ClampsToSource()
    {
        var source = 3f;
        var target = 5f;
        var transferred = StatLogic.Transfer(ref source, ref target, 20f, 5f);

        Assert.Equal(0f, source);
        Assert.Equal(8f, target);
        Assert.Equal(3f, transferred);
    }

    [Fact]
    public void Transfer_ClampsToTargetMax()
    {
        var source = 10f;
        var target = 18f;
        var transferred = StatLogic.Transfer(ref source, ref target, 20f, 5f);

        Assert.Equal(8f, source);
        Assert.Equal(20f, target);
        Assert.Equal(2f, transferred);
    }
}