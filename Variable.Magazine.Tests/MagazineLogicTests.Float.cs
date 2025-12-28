using Xunit;
using Variable.Magazine;
using Variable.Bounded;

namespace Variable.Magazine.Tests;

public class MagazineLogicTests_Float
{
    [Fact]
    public void Reload_FillsClip_FromReserve()
    {
        float current = 5f;
        float reserve = 20f;
        float reloaded = MagazineLogic.Reload(ref current, 10f, ref reserve);

        Assert.Equal(10f, current);
        Assert.Equal(15f, reserve);
        Assert.Equal(5f, reloaded);
    }

    [Fact]
    public void Reload_PartiallyFills_WhenReserveLow()
    {
        float current = 5f;
        float reserve = 3f;
        float reloaded = MagazineLogic.Reload(ref current, 10f, ref reserve);

        Assert.Equal(8f, current);
        Assert.Equal(0f, reserve);
        Assert.Equal(3f, reloaded);
    }

    [Fact]
    public void Reload_BoundedFloat_Works()
    {
        var clip = new BoundedFloat(10f, 5f);
        float reserve = 20f;
        float reloaded = MagazineLogic.Reload(ref clip, ref reserve);

        Assert.Equal(10f, clip.Current);
        Assert.Equal(15f, reserve);
        Assert.Equal(5f, reloaded);
    }
}
