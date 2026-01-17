using NUnit.Framework;
using Variable.Random;

namespace Variable.Random.Tests;

public class RandomTests
{
    [Test]
    public void TestDeterminism()
    {
        var rng1 = new RandomState(12345);
        var rng2 = new RandomState(12345);

        for (int i = 0; i < 100; i++)
        {
            Assert.That(rng1.Next(), Is.EqualTo(rng2.Next()));
        }
    }

    [Test]
    public void TestDifferentSeeds()
    {
        var rng1 = new RandomState(12345);
        var rng2 = new RandomState(67890);

        // It is statistically impossible for 100 random numbers to be identical
        bool allSame = true;
        for (int i = 0; i < 100; i++)
        {
            if (rng1.Next() != rng2.Next())
            {
                allSame = false;
                break;
            }
        }
        Assert.That(allSame, Is.False);
    }

    [Test]
    public void TestRange()
    {
        var rng = new RandomState(42);
        for (int i = 0; i < 1000; i++)
        {
            int val = rng.Range(10, 20);
            Assert.That(val, Is.GreaterThanOrEqualTo(10));
            Assert.That(val, Is.LessThan(20));
        }
    }

    [Test]
    public void TestRangeSingle()
    {
        var rng = new RandomState(42);
        int val = rng.Range(10, 10);
        Assert.That(val, Is.EqualTo(10));
    }

    [Test]
    public void TestFloatDistribution()
    {
        var rng = new RandomState(999);
        int bins = 10;
        int[] counts = new int[bins];
        int iterations = 10000;

        for (int i = 0; i < iterations; i++)
        {
            float val = rng.NextFloat();
            Assert.That(val, Is.GreaterThanOrEqualTo(0f));
            Assert.That(val, Is.LessThan(1f));

            int bin = (int)(val * bins);
            if (bin >= bins) bin = bins - 1;
            counts[bin]++;
        }

        // Basic uniformity check (very loose)
        int expected = iterations / bins;
        int tolerance = (int)(expected * 0.2f); // 20% tolerance

        for (int i = 0; i < bins; i++)
        {
            Assert.That(counts[i], Is.InRange(expected - tolerance, expected + tolerance), $"Bin {i} count {counts[i]} out of range");
        }
    }

    [Test]
    public void TestSerialization()
    {
        var rng = new RandomState(100);
        rng.Next();
        rng.Next();

        // Manually copy state
        var rngCopy = new RandomState(rng.S0, rng.S1, rng.S2, rng.S3);

        Assert.That(rngCopy.S0, Is.EqualTo(rng.S0));
        Assert.That(rngCopy.S1, Is.EqualTo(rng.S1));

        Assert.That(rngCopy.Next(), Is.EqualTo(rng.Next()));
    }
}
