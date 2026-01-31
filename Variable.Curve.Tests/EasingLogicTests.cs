namespace Variable.Curve.Tests;

public class EasingLogicTests
{
    [Theory]
    [InlineData(EaseType.Linear)]
    [InlineData(EaseType.InQuad)]
    [InlineData(EaseType.OutQuad)]
    [InlineData(EaseType.InOutQuad)]
    [InlineData(EaseType.InCubic)]
    [InlineData(EaseType.OutCubic)]
    [InlineData(EaseType.InQuart)]
    [InlineData(EaseType.OutQuint)]
    [InlineData(EaseType.InSine)]
    [InlineData(EaseType.OutExpo)]
    [InlineData(EaseType.InCirc)]
    [InlineData(EaseType.InBack)]
    [InlineData(EaseType.InElastic)]
    [InlineData(EaseType.InBounce)]
    public void Evaluate_AtZero_ReturnsZero(EaseType type)
    {
        // Act
        float t = 0f;
        float result = t.Ease(type);

        // Assert
        Assert.Equal(0f, result, 0.0001f);
    }

    [Theory]
    [InlineData(EaseType.Linear)]
    [InlineData(EaseType.InQuad)]
    [InlineData(EaseType.OutQuad)]
    [InlineData(EaseType.InOutQuad)]
    [InlineData(EaseType.InCubic)]
    [InlineData(EaseType.OutCubic)]
    [InlineData(EaseType.InQuart)]
    [InlineData(EaseType.OutQuint)]
    [InlineData(EaseType.InSine)]
    [InlineData(EaseType.OutExpo)]
    [InlineData(EaseType.InCirc)]
    [InlineData(EaseType.InBack)]
    [InlineData(EaseType.InElastic)]
    [InlineData(EaseType.InBounce)]
    public void Evaluate_AtOne_ReturnsOne(EaseType type)
    {
        // Act
        float t = 1f;
        float result = t.Ease(type);

        // Assert
        Assert.Equal(1f, result, 0.0001f);
    }

    [Fact]
    public void InQuad_CalculatesCorrectly()
    {
        Assert.Equal(0.25f, 0.5f.Ease(EaseType.InQuad), 0.0001f);
    }

    [Fact]
    public void OutQuad_CalculatesCorrectly()
    {
        // 1 - (1-0.5)^2 = 1 - 0.25 = 0.75
        Assert.Equal(0.75f, 0.5f.Ease(EaseType.OutQuad), 0.0001f);
    }

    [Fact]
    public void Linear_CalculatesCorrectly()
    {
        Assert.Equal(0.5f, 0.5f.Ease(EaseType.Linear), 0.0001f);
        Assert.Equal(0.23f, 0.23f.Ease(EaseType.Linear), 0.0001f);
    }

    [Fact]
    public void InOutQuad_CalculatesCorrectly()
    {
        // t < 0.5: 2 * t * t = 2 * 0.25 * 0.25 = 2 * 0.0625 = 0.125
        Assert.Equal(0.125f, 0.25f.Ease(EaseType.InOutQuad), 0.0001f);
    }
}
