namespace Variable.Random.Tests;

public class RandomTests
{
    [Fact]
    public void Next_IsDeterministic()
    {
        var rng1 = new RandomState();
        rng1.Init(42);

        var rng2 = new RandomState();
        rng2.Init(42);

        rng1.Next(out uint val1);
        rng2.Next(out uint val2);

        Assert.Equal(val1, val2);

        rng1.NextFloat(out float f1);
        rng2.NextFloat(out float f2);

        Assert.Equal(f1, f2);
    }

    [Fact]
    public void Next_UpdatesState()
    {
        var rng = new RandomState();
        rng.Init(123);

        uint initialS0 = rng.S0;
        uint initialS1 = rng.S1;
        uint initialS2 = rng.S2;
        uint initialS3 = rng.S3;

        rng.Next(out _);

        bool changed = rng.S0 != initialS0 || rng.S1 != initialS1 || rng.S2 != initialS2 || rng.S3 != initialS3;
        Assert.True(changed, "State should update after Next call");
    }

    [Fact]
    public void Range_RespectsBounds()
    {
        var rng = new RandomState();
        rng.Init(555);

        for (int i = 0; i < 100; i++)
        {
            rng.Range(10, 20, out int val);
            Assert.True(val >= 10);
            Assert.True(val < 20);
        }
    }

    [Fact]
    public void NextFloat_IsInRange()
    {
        var rng = new RandomState();
        rng.Init(777);

        for (int i = 0; i < 1000; i++)
        {
            rng.NextFloat(out float val);
            Assert.True(val >= 0.0f);
            Assert.True(val < 1.0f);
        }
    }

    [Fact]
    public void Chance_BehavesSensibly()
    {
        var rng = new RandomState();
        rng.Init(999);

        int trueCount = 0;
        int trials = 1000;

        for (int i = 0; i < trials; i++)
        {
            rng.Chance(0.5f, out bool result);
            if (result) trueCount++;
        }

        // Very rough check, just to ensure it's not always true or false
        Assert.True(trueCount > 400 && trueCount < 600);
    }

    [Fact]
    public void NextInt_IsPositive()
    {
        var rng = new RandomState();
        rng.Init(101);

        for (int i = 0; i < 100; i++)
        {
            rng.NextInt(out int val);
            Assert.True(val >= 0);
        }
    }
}
