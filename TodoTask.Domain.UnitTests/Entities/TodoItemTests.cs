using TodoTask.Domain.Entities;

namespace TodoTask.Domain.UnitTests.Entities;

public class TodoItemTests
{
    [Fact]
    public void Constructor_WithValidInputs_ShouldCreateInstance()
    {
        var todoItem = new TodoItem(1, "Test Title", "Test Description", "Test Category");

        Assert.Equal(1, todoItem.Id);
        Assert.Equal("Test Title", todoItem.Title);
        Assert.Equal("Test Description", todoItem.Description);
        Assert.Equal("Test Category", todoItem.Category);
        Assert.Empty(todoItem.Progressions);
        Assert.False(todoItem.IsCompleted);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidId_ShouldThrowArgumentException(int id)
    {
        var exception = Assert.Throws<ArgumentException>(() => 
            new TodoItem(id, "Test Title", "Test Description", "Test Category"));
                
        Assert.Contains("Id", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidTitle_ShouldThrowArgumentException(string? title)
    {
        var exception = Assert.Throws<ArgumentException>(() => 
            new TodoItem(1, title, "Test Description", "Test Category"));
                
        Assert.Contains("título", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidDescription_ShouldThrowArgumentException(string? description)
    {
        var exception = Assert.Throws<ArgumentException>(() => 
            new TodoItem(1, "Test Title", description, "Test Category"));
                
        Assert.Contains("descripción", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidCategory_ShouldThrowArgumentException(string? category)
    {
        var exception = Assert.Throws<ArgumentException>(() => 
            new TodoItem(1, "Test Title", "Test Description", category));
                
        Assert.Contains("categoría", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UpdateDescription_WithValidDescription_ShouldUpdateDescription()
    {
        var todoItem = new TodoItem(1, "Test Title", "Old Description", "Test Category");
        var newDescription = "New Description";

        todoItem.UpdateDescription(newDescription);

        Assert.Equal(newDescription, todoItem.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateDescription_WithInvalidDescription_ShouldThrowArgumentException(string? description)
    {
        var todoItem = new TodoItem(1, "Test Title", "Old Description", "Test Category");

        var exception = Assert.Throws<ArgumentException>(() => todoItem.UpdateDescription(description));
            
        Assert.Contains("descripción", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UpdateDescription_WhenMoreThan50PercentComplete_ShouldThrowInvalidOperationException()
    {
        var todoItem = new TodoItem(1, "Test Title", "Old Description", "Test Category");
        todoItem.AddProgression(DateTime.Now, 51m);

        var exception = Assert.Throws<InvalidOperationException>(() => 
            todoItem.UpdateDescription("New Description"));
                
        Assert.Contains("50%", exception.Message);
    }

    [Fact]
    public void AddProgression_WithValidValues_ShouldAddProgressionToList()
    {
        var todoItem = new TodoItem(1, "Test Title", "Test Description", "Test Category");
        var date = DateTime.Now;
        var percent = 25m;

        todoItem.AddProgression(date, percent);

        Assert.Single(todoItem.Progressions);
        Assert.Equal(date, todoItem.Progressions.First().Date);
        Assert.Equal(percent, todoItem.Progressions.First().Percent);
        Assert.False(todoItem.IsCompleted);
    }

    [Fact]
    public void AddProgression_WithLaterDate_ShouldAddMultipleProgressions()
    {
        var todoItem = new TodoItem(1, "Test Title", "Test Description", "Test Category");
        var date1 = new DateTime(2025, 1, 1);
        var date2 = new DateTime(2025, 1, 2);

        todoItem.AddProgression(date1, 25m);
        todoItem.AddProgression(date2, 25m);

        Assert.Equal(2, todoItem.Progressions.Count);
        Assert.Equal(50m, todoItem.Progressions.Sum(p => p.Percent));
        Assert.False(todoItem.IsCompleted);
    }

    [Fact]
    public void AddProgression_WithEarlierDate_ShouldThrowInvalidOperationException()
    {
        var todoItem = new TodoItem(1, "Test Title", "Test Description", "Test Category");
        var date1 = new DateTime(2025, 1, 2);
        var date2 = new DateTime(2025, 1, 1); // Fecha anterior
        todoItem.AddProgression(date1, 25m);

        var exception = Assert.Throws<InvalidOperationException>(() => 
            todoItem.AddProgression(date2, 25m));
                
        Assert.Contains("fecha", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AddProgression_ExceedingTotalPercent_ShouldThrowInvalidOperationException()
    {
        var todoItem = new TodoItem(1, "Test Title", "Test Description", "Test Category");
        todoItem.AddProgression(DateTime.Now, 60m);

        var exception = Assert.Throws<InvalidOperationException>(() => 
            todoItem.AddProgression(DateTime.Now.AddDays(1), 50m));
                
        Assert.Contains("100%", exception.Message);
    }

    [Fact]
    public void IsCompleted_WhenProgressionReaches100Percent_ShouldReturnTrue()
    {
        var todoItem = new TodoItem(1, "Test Title", "Test Description", "Test Category");
        todoItem.AddProgression(DateTime.Now, 50m);
        todoItem.AddProgression(DateTime.Now.AddDays(1), 50m);

        Assert.True(todoItem.IsCompleted);
    }

    [Fact]
    public void GetAccumulatedPercentAt_ShouldCalculateCorrectTotal()
    {
        var todoItem = new TodoItem(1, "Test Title", "Test Description", "Test Category");
        todoItem.AddProgression(DateTime.Now, 25m);
        todoItem.AddProgression(DateTime.Now.AddDays(1), 30m);
        todoItem.AddProgression(DateTime.Now.AddDays(2), 20m);

        Assert.Equal(25m, todoItem.GetAccumulatedPercentAt(0));
        Assert.Equal(55m, todoItem.GetAccumulatedPercentAt(1));
        Assert.Equal(75m, todoItem.GetAccumulatedPercentAt(2));
    }
}