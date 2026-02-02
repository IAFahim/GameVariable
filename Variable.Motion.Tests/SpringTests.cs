namespace Variable.Motion.Tests;

public class SpringTests
{
    [Fact]
    public void Constructor_Raw_SetsValues()
    {
        var spring = new SpringFloat(0f, 100f, 50f, 5f);
        Assert.Equal(0f, spring.Current);
        Assert.Equal(100f, spring.Target);
        Assert.Equal(50f, spring.Stiffness);
        Assert.Equal(5f, spring.Damping);
        Assert.Equal(0f, spring.Velocity);
    }

    [Fact]
    public void FromFrequency_CalculatesCorrectly()
    {
        // f = 1Hz, z = 1
        // k = (2*pi)^2 ~= 39.47
        // c = 4*pi ~= 12.56
        var spring = SpringFloat.FromFrequency(0f, 100f, 1f, 1f);

        Assert.True(spring.Stiffness > 39f && spring.Stiffness < 40f);
        Assert.True(spring.Damping > 12f && spring.Damping < 13f);
    }

    [Fact]
    public void Tick_MovesTowardsTarget()
    {
        var spring = SpringFloat.FromFrequency(0f, 100f, 1f, 1f);

        // Initial force: 100 * k
        // Initial accel: 100 * k
        // Tick small amount
        spring.Tick(0.016f);

        Assert.True(spring.Current > 0f); // Moved forward
        Assert.True(spring.Velocity > 0f); // Gained velocity
    }

    [Fact]
    public void Snap_Teleports()
    {
        var spring = new SpringFloat(0f, 100f, 50f, 5f);
        spring.Velocity = 500f;

        spring.Snap();

        Assert.Equal(100f, spring.Current);
        Assert.Equal(0f, spring.Velocity);
    }

    [Fact]
    public void IsSettled_ReturnsTrueWhenClose()
    {
        var spring = new SpringFloat(100f, 100f, 50f, 5f);
        Assert.True(spring.IsSettled());

        spring.Current = 99f; // Dist = 1
        Assert.False(spring.IsSettled(0.1f));

        spring.Current = 100f;
        spring.Velocity = 10f;
        Assert.False(spring.IsSettled(0.1f));
    }
}
