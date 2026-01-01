namespace Variable.RPG.Tests;

public class DamageLogicTests
{
    // 1. Define Game Constants
    private static class MyIds
    {
        public const int Health = 0;
        public const int Armor = 1;      // Flat
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

    [Fact]
    public void Resolve_Physical_UsesArmor()
    {
        var sheet = new AttributeSheet(5);
        sheet.SetBase(MyIds.Armor, 10f);
        sheet.Attributes[MyIds.Armor].IsDirty = 1; // 10 Armor

        var damages = new DamagePacket[]
        {
            new DamagePacket { ElementId = MyIds.DmgPhysical, Amount = 50f }
        };

        var final = DamageLogic.ResolveDamage(sheet.AsSpan(), damages, new MyConfig());

        // 50 Dmg - 10 Armor = 40
        Assert.Equal(40f, final);
    }

    [Fact]
    public void Resolve_Fire_UsesResistance()
    {
        var sheet = new AttributeSheet(5);
        sheet.SetBase(MyIds.FireResist, 0.5f);
        sheet.Attributes[MyIds.FireResist].IsDirty = 1; // 50% Resist

        var damages = new DamagePacket[]
        {
            new DamagePacket { ElementId = MyIds.DmgFire, Amount = 100f }
        };

        var final = DamageLogic.ResolveDamage(sheet.AsSpan(), damages, new MyConfig());

        // 100 Dmg * (1.0 - 0.5) = 50
        Assert.Equal(50f, final);
    }

    [Fact]
    public void Resolve_MixedDamage_Aggregates()
    {
        var sheet = new AttributeSheet(5);
        sheet.SetBase(MyIds.Armor, 5f);         // -5 Flat
        sheet.SetBase(MyIds.FireResist, 0.25f); // -25% Percent

        var damages = new DamagePacket[]
        {
            new DamagePacket { ElementId = MyIds.DmgPhysical, Amount = 20f }, // -> 15
            new DamagePacket { ElementId = MyIds.DmgFire, Amount = 100f }     // -> 75
        };

        var final = DamageLogic.ResolveDamage(sheet.AsSpan(), damages, new MyConfig());

        Assert.Equal(90f, final); // 15 + 75
    }

    [Fact]
    public void Resolve_NegativeDamage_ClampsToZero()
    {
        var sheet = new AttributeSheet(5);
        sheet.SetBase(MyIds.Armor, 100f);
        sheet.Attributes[MyIds.Armor].IsDirty = 1; // Massive armor

        var damages = new DamagePacket[]
        {
            new DamagePacket { ElementId = MyIds.DmgPhysical, Amount = 10f }
        };

        var final = DamageLogic.ResolveDamage(sheet.AsSpan(), damages, new MyConfig());

        // 10 - 100 = -90, clamped to 0
        Assert.Equal(0f, final);
    }

    [Fact]
    public void Resolve_UnmappedElement_TakesFullDamage()
    {
        var sheet = new AttributeSheet(5);

        var damages = new DamagePacket[]
        {
            new DamagePacket { ElementId = 999, Amount = 50f } // No mapping for 999
        };

        var final = DamageLogic.ResolveDamage(sheet.AsSpan(), damages, new MyConfig());

        Assert.Equal(50f, final); // Full damage
    }

    [Fact]
    public void Resolve_OverResistance_AmplifiedDamage()
    {
        var sheet = new AttributeSheet(5);
        // Allow negative resistance (amplified damage)
        sheet.Attributes[MyIds.FireResist] = new Attribute(-0.5f, -1f, 1f); // Min -1, Max 1

        var damages = new DamagePacket[]
        {
            new DamagePacket { ElementId = MyIds.DmgFire, Amount = 100f }
        };

        var final = DamageLogic.ResolveDamage(sheet.AsSpan(), damages, new MyConfig());

        // 100 * (1 - (-0.5)) = 100 * 1.5 = 150
        Assert.Equal(150f, final);
    }

    [Fact]
    public void Resolve_EmptyDamageArray_ReturnsZero()
    {
        var sheet = new AttributeSheet(5);
        var damages = Array.Empty<DamagePacket>();

        var final = DamageLogic.ResolveDamage(sheet.AsSpan(), damages, new MyConfig());

        Assert.Equal(0f, final);
    }
}
