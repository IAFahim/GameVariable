namespace Variable.Random.Tests;

public class RandomTests
{
    [Fact]
    public void Determinism_SameSeed_SameSequence()
    {
        var rng1 = new RandomState(12345);
        var rng2 = new RandomState(12345);

        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(rng1.NextUInt(), rng2.NextUInt());
        }
    }

    [Fact]
    public void Divergence_DifferentSeed_DifferentSequence()
    {
        var rng1 = new RandomState(12345);
        var rng2 = new RandomState(67890);

        bool anyDifference = false;
        for (int i = 0; i < 100; i++)
        {
            if (rng1.NextUInt() != rng2.NextUInt())
            {
                anyDifference = true;
                break;
            }
        }
        Assert.True(anyDifference, "Different seeds should produce different sequences");
    }

    [Fact]
    public void NextInt_RespectsRange()
    {
        var rng = new RandomState(42);
        int min = 10;
        int max = 20;

        for (int i = 0; i < 1000; i++)
        {
            int val = rng.NextInt(min, max);
            Assert.True(val >= min, $"Value {val} should be >= {min}");
            Assert.True(val < max, $"Value {val} should be < {max}");
        }
    }

    [Fact]
    public void NextFloat_InRange()
    {
        var rng = new RandomState(42);
        for (int i = 0; i < 1000; i++)
        {
            float f = rng.NextFloat();
            Assert.True(f >= 0.0f, $"Float {f} should be >= 0.0f");
            Assert.True(f < 1.0f, $"Float {f} should be < 1.0f");
        }
    }

    [Fact]
    public void NextBool_Distribution()
    {
        // Basic sanity check that we get both true and false
        var rng = new RandomState(999);
        bool seenTrue = false;
        bool seenFalse = false;

        for (int i = 0; i < 100; i++)
        {
            if (rng.NextBool()) seenTrue = true;
            else seenFalse = true;

            if (seenTrue && seenFalse) break;
        }

        Assert.True(seenTrue && seenFalse, "Should generate both true and false");
    }
}
