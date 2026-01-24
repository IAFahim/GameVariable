using Variable.Random;

namespace Variable.Random.Tests;

public class RngTests
{
    [Fact]
    public void Constructor_SeedZero_CorrectedToOne()
    {
        var rng = new RngState(0);
        Assert.NotEqual(0u, rng.State);
        Assert.Equal(1u, rng.State);
    }

    [Fact]
    public void Determinism_SameSeed_SameSequence()
    {
        var rng1 = new RngState(12345);
        var rng2 = new RngState(12345);

        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(rng1.Next(), rng2.Next());
        }
    }

    [Fact]
    public void NextFloat_InRange_0to1()
    {
        var rng = new RngState(999);
        for (int i = 0; i < 1000; i++)
        {
            float val = rng.NextFloat();
            Assert.True(val >= 0f && val < 1f, $"Value {val} out of range");
        }
    }

    [Fact]
    public void Range_Int_RespectsBounds()
    {
        var rng = new RngState(555);
        int min = 10;
        int max = 20;

        for (int i = 0; i < 1000; i++)
        {
            int val = rng.Range(min, max);
            Assert.True(val >= min && val < max, $"Value {val} out of range [{min}, {max})");
        }
    }

    [Fact]
    public void Range_Negative_RespectsBounds()
    {
        var rng = new RngState(777);
        int min = -50;
        int max = -20;

        for (int i = 0; i < 1000; i++)
        {
            int val = rng.Range(min, max);
            Assert.True(val >= min && val < max, $"Value {val} out of range [{min}, {max})");
        }
    }

    [Fact]
    public void Range_MinEqualsMax_ReturnsMin()
    {
        var rng = new RngState(100);
        int val = rng.Range(10, 10);
        Assert.Equal(10, val);
    }

    [Fact]
    public void Distribution_SanityCheck()
    {
        // Check that we get somewhat different numbers (not exhaustive statistical test)
        var rng = new RngState(123);
        int below = 0;
        int above = 0;
        int iterations = 1000;

        for (int i = 0; i < iterations; i++)
        {
            if (rng.NextFloat() < 0.5f) below++;
            else above++;
        }

        // Should be roughly 50/50. Allow 10% deviation.
        Assert.InRange(below, 400, 600);
        Assert.InRange(above, 400, 600);
    }
}
