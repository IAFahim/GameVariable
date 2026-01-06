namespace Variable.Regen.Tests;

public class RegenLogicTests_Time
{
    [Fact]
    public void GetTimeToFull_CalculatesCorrectly()
    {
        RegenLogic.GetTimeToFull(50f, 100f, 10f, out var result);
        Assert.Equal(5f, result);
    }

    [Fact]
    public void GetTimeToEmpty_CalculatesCorrectly()
    {
        RegenLogic.GetTimeToEmpty(50f, -10f, out var result);
        Assert.Equal(5f, result);
    }
}