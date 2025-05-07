using TodoTask.Domain.ValueObjects;

namespace TodoTask.Domain.UnitTests.ValueObjects;

public class ProgressionTests
{
    [Fact]
    public void Create_WithValidValues_ShouldCreateProgressionInstance()
    {
        // Arrange
        var date = DateTime.Now;
        decimal percent = 25.5m;

        // Act
        var progression = Progression.Create(date, percent);

        // Assert
        Assert.NotNull(progression);
        Assert.Equal(date, progression.Date);
        Assert.Equal(percent, progression.Percent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(-10)]
    [InlineData(110)]
    public void Create_WithInvalidPercent_ShouldThrowArgumentException(decimal percent)
    {
        // Arrange
        var date = DateTime.Now;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Progression.Create(date, percent));
        Assert.Contains("porcentaje", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}