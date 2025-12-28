using Xunit;
using Variable.Magazine;

namespace Variable.Magazine.Tests;

public class MagazineStructTests
{
    [Fact]
    public void MagazineInt_Reload_Works()
    {
        var mag = new MagazineInt(10, 5, 20);
        int reloaded = mag.Reload();

        Assert.Equal(10, mag.Clip.Current);
        Assert.Equal(15, mag.Reserve);
        Assert.Equal(5, reloaded);
    }

    [Fact]
    public void MagazineFloat_Reload_Works()
    {
        var mag = new MagazineFloat(10f, 5f, 20f);
        float reloaded = mag.Reload();

        Assert.Equal(10f, mag.Clip.Current);
        Assert.Equal(15f, mag.Reserve);
        Assert.Equal(5f, reloaded);
    }
}
