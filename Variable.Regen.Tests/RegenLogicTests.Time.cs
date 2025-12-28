namespace Variable.Regen.Tests;

public class RegenLogicTests_Time
{
    [Fact]
    public void GetTimeToFull_CalculatesCorrectly()
    {
        Assert.Equal(5f, RegenLogic.GetTimeToFull(50f, 100f, 10f));
    }

    [Fact]
    public void GetTimeToEmpty_CalculatesCorrectly()
    {
        Assert.Equal(5f, RegenLogic.GetTimeToEmpty(50f, -10f));
    }
}