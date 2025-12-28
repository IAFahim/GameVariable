using Xunit;
using Variable.Stat;

namespace Variable.Stat.Tests;

public class StatLogicTests_Transfer
{
    [Fact]
    public void Transfer_MovesValue()
    {
        float source = 10f;
        float target = 5f;
        float transferred = StatLogic.Transfer(ref source, ref target, 20f, 5f);
        
        Assert.Equal(5f, source);
        Assert.Equal(10f, target);
        Assert.Equal(5f, transferred);
    }

    [Fact]
    public void Transfer_ClampsToSource()
    {
        float source = 3f;
        float target = 5f;
        float transferred = StatLogic.Transfer(ref source, ref target, 20f, 5f);
        
        Assert.Equal(0f, source);
        Assert.Equal(8f, target);
        Assert.Equal(3f, transferred);
    }

    [Fact]
    public void Transfer_ClampsToTargetMax()
    {
        float source = 10f;
        float target = 18f;
        float transferred = StatLogic.Transfer(ref source, ref target, 20f, 5f);
        
        Assert.Equal(8f, source);
        Assert.Equal(20f, target);
        Assert.Equal(2f, transferred);
    }
}
