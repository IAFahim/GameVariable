using NUnit.Framework;
using Variable.Random;

namespace Variable.Random.Tests;

[TestFixture]
public class PcgRandomTests
{
    [Test]
    public void Determinism_SameSeed_ProducesSameSequence()
    {
        var rng1 = new PcgRandom(12345);
        var rng2 = new PcgRandom(12345);

        for (int i = 0; i < 100; i++)
        {
            Assert.That(rng1.Next(), Is.EqualTo(rng2.Next()));
        }
    }

    [Test]
    public void Determinism_DifferentSeeds_ProducesDifferentSequence()
    {
        var rng1 = new PcgRandom(12345);
        var rng2 = new PcgRandom(54321);

        bool allSame = true;
        for (int i = 0; i < 100; i++)
        {
            if (rng1.Next() != rng2.Next())
            {
                allSame = false;
                break;
            }
        }
        Assert.That(allSame, Is.False, "Different seeds should produce different sequences.");
    }

    [Test]
    public void NextFloat_ReturnsValuesBetween0And1()
    {
        var rng = new PcgRandom(42);
        for (int i = 0; i < 1000; i++)
        {
            float val = rng.NextFloat();
            Assert.That(val, Is.GreaterThanOrEqualTo(0f));
            Assert.That(val, Is.LessThan(1f));
        }
    }

    [Test]
    public void NextInt_Range_RespectsBounds()
    {
        var rng = new PcgRandom(42);
        for (int i = 0; i < 1000; i++)
        {
            int val = rng.NextInt(10, 20);
            Assert.That(val, Is.GreaterThanOrEqualTo(10));
            Assert.That(val, Is.LessThan(20));
        }
    }

    [Test]
    public void NextInt_ZeroRange_ReturnsMin()
    {
        var rng = new PcgRandom(42);
        Assert.That(rng.NextInt(10, 10), Is.EqualTo(10));
    }

    [Test]
    public void Next_Max_RespectsBounds()
    {
        var rng = new PcgRandom(42);
        for (int i = 0; i < 1000; i++)
        {
            uint val = rng.Next(10);
            Assert.That(val, Is.LessThan(10));
        }
    }

    [Test]
    public void Serialization_Equals()
    {
        var rng1 = new PcgRandom(123);
        rng1.Next(); // Advance state

        var rng2 = rng1; // Copy struct

        Assert.That(rng2, Is.EqualTo(rng1));
        Assert.That(rng2.Next(), Is.EqualTo(rng1.Next()));
    }

    [Test]
    public void Streams_IndependentSequences()
    {
        // Same seed, different stream (sequence)
        var rng1 = new PcgRandom(12345, 1);
        var rng2 = new PcgRandom(12345, 2);

        bool allSame = true;
        for (int i = 0; i < 100; i++)
        {
            if (rng1.Next() != rng2.Next())
            {
                allSame = false;
                break;
            }
        }
        Assert.That(allSame, Is.False, "Different streams should produce different sequences.");
    }
}
