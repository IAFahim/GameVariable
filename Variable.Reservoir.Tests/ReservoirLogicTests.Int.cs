using Xunit;
using Variable.Reservoir;
using Variable.Bounded;

namespace Variable.Reservoir.Tests;

public class ReservoirLogicTests_Int
{
    [Fact]
    public void Refill_FillsVolume_FromReserve()
    {
        int current = 5;
        int reserve = 20;
        int refilled = ReservoirLogic.Refill(ref current, 10, ref reserve);

        Assert.Equal(10, current);
        Assert.Equal(15, reserve);
        Assert.Equal(5, refilled);
    }

    [Fact]
    public void Refill_PartiallyFills_WhenReserveLow()
    {
        int current = 5;
        int reserve = 3;
        int refilled = ReservoirLogic.Refill(ref current, 10, ref reserve);

        Assert.Equal(8, current);
        Assert.Equal(0, reserve);
        Assert.Equal(3, refilled);
    }

    [Fact]
    public void Refill_DoesNothing_WhenFull()
    {
        int current = 10;
        int reserve = 20;
        int refilled = ReservoirLogic.Refill(ref current, 10, ref reserve);

        Assert.Equal(10, current);
        Assert.Equal(20, reserve);
        Assert.Equal(0, refilled);
    }

    [Fact]
    public void Refill_BoundedInt_Works()
    {
        var volume = new BoundedInt(10, 5);
        int reserve = 20;
        int refilled = ReservoirLogic.Refill(ref volume, ref reserve);

        Assert.Equal(10, volume.Current);
        Assert.Equal(15, reserve);
        Assert.Equal(5, refilled);
    }
}
