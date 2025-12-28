using Xunit;
using Variable.Magazine;
using Variable.Bounded;

namespace Variable.Magazine.Tests;

public class MagazineLogicTests_Int
{
    [Fact]
    public void Reload_FillsClip_FromReserve()
    {
        int current = 5;
        int reserve = 20;
        int reloaded = MagazineLogic.Reload(ref current, 10, ref reserve);

        Assert.Equal(10, current);
        Assert.Equal(15, reserve);
        Assert.Equal(5, reloaded);
    }

    [Fact]
    public void Reload_PartiallyFills_WhenReserveLow()
    {
        int current = 5;
        int reserve = 3;
        int reloaded = MagazineLogic.Reload(ref current, 10, ref reserve);

        Assert.Equal(8, current);
        Assert.Equal(0, reserve);
        Assert.Equal(3, reloaded);
    }

    [Fact]
    public void Reload_DoesNothing_WhenFull()
    {
        int current = 10;
        int reserve = 20;
        int reloaded = MagazineLogic.Reload(ref current, 10, ref reserve);

        Assert.Equal(10, current);
        Assert.Equal(20, reserve);
        Assert.Equal(0, reloaded);
    }

    [Fact]
    public void Reload_BoundedInt_Works()
    {
        var clip = new BoundedInt(10, 5);
        int reserve = 20;
        int reloaded = MagazineLogic.Reload(ref clip, ref reserve);

        Assert.Equal(10, clip.Current);
        Assert.Equal(15, reserve);
        Assert.Equal(5, reloaded);
    }
}
