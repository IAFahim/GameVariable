namespace Variable.RPG.Tests;

public class DamageLogicTests
{
    [Fact]
    public void Resolve_Physical_UsesArmor()
    {
        var sheet = new RpgStatSheet(5);
        sheet.SetBase(MyIds.Armor, 10f);

        var damages = new[]
        {
            new DamagePacket { ElementId = MyIds.DmgPhysical, Amount = 50f }
        };

        var final = sheet.AsSpan().ResolveDamage(damages, new MyConfig());

        // 50 Dmg - 10 Armor = 40
        Assert.Equal(40f, final);

        sheet.Dispose();
    }

    [Fact]
    public void Resolve_Fire_UsesResistance()
    {
        var sheet = new RpgStatSheet(5);
        sheet.SetBase(MyIds.FireResist, 0.5f);

        var damages = new[]
        {
            new DamagePacket { ElementId = MyIds.DmgFire, Amount = 100f }
        };

        var final = sheet.AsSpan().ResolveDamage(damages, new MyConfig());

        // 100 Dmg * (1.0 - 0.5) = 50
        Assert.Equal(50f, final);

        sheet.Dispose();
    }

    [Fact]
    public void Resolve_MixedDamage_Aggregates()
    {
        var sheet = new RpgStatSheet(5);
        sheet.SetBase(MyIds.Armor, 5f); // -5 Flat
        sheet.SetBase(MyIds.FireResist, 0.25f); // -25% Percent

        var damages = new[]
        {
            new DamagePacket { ElementId = MyIds.DmgPhysical, Amount = 20f }, // -> 15
            new DamagePacket { ElementId = MyIds.DmgFire, Amount = 100f } // -> 75
        };

        var final = sheet.AsSpan().ResolveDamage(damages, new MyConfig());

        Assert.Equal(90f, final); // 15 + 75

        sheet.Dispose();
    }

    [Fact]
    public void Resolve_NegativeDamage_ClampsToZero()
    {
        var sheet = new RpgStatSheet(5);
        sheet.SetBase(MyIds.Armor, 100f);

        var damages = new[]
        {
            new DamagePacket { ElementId = MyIds.DmgPhysical, Amount = 10f }
        };

        var final = sheet.AsSpan().ResolveDamage(damages, new MyConfig());

        // 10 - 100 = -90, clamped to 0
        Assert.Equal(0f, final);

        sheet.Dispose();
    }

    [Fact]
    public void Resolve_UnmappedElement_TakesFullDamage()
    {
        var sheet = new RpgStatSheet(5);

        var damages = new[]
        {
            new DamagePacket { ElementId = 999, Amount = 50f } // No mapping for 999
        };

        var final = sheet.AsSpan().ResolveDamage(damages, new MyConfig());

        Assert.Equal(50f, final); // Full damage

        sheet.Dispose();
    }

    [Fact]
    public void Resolve_OverResistance_AmplifiedDamage()
    {
        var sheet = new RpgStatSheet(5);
        // Allow negative resistance (amplified damage)
        sheet[MyIds.FireResist] = new RpgStat(-0.5f, -1f, 1f); // Min -1, Max 1

        var damages = new[]
        {
            new DamagePacket { ElementId = MyIds.DmgFire, Amount = 100f }
        };

        var final = sheet.AsSpan().ResolveDamage(damages, new MyConfig());

        // 100 * (1 - (-0.5)) = 100 * 1.5 = 150
        Assert.Equal(150f, final);

        sheet.Dispose();
    }

    [Fact]
    public void Resolve_EmptyDamageArray_ReturnsZero()
    {
        var sheet = new RpgStatSheet(5);
        var damages = Array.Empty<DamagePacket>();

        var final = sheet.AsSpan().ResolveDamage(damages, new MyConfig());

        Assert.Equal(0f, final);

        sheet.Dispose();
    }

    // 1. Define Game Constants
    private static class MyIds
    {
        public const int Armor = 1; // Flat
        public const int FireResist = 2; // Percent

        public const int DmgPhysical = 100;
        public const int DmgFire = 101;
    }

    // 2. Define Game Config
    private struct MyConfig : IDamageConfig
    {
        public bool TryGetMitigationStat(int elementId, out int statId, out bool isFlat)
        {
            statId = -1;
            isFlat = false;

            if (elementId == MyIds.DmgPhysical)
            {
                statId = MyIds.Armor;
                isFlat = true;
                return true;
            }

            if (elementId == MyIds.DmgFire)
            {
                statId = MyIds.FireResist;
                isFlat = false;
                return true;
            }

            return false;
        }
    }
}