using Moq;
using TodoTask.Domain.Entities;

namespace TodoTask.Domain.UnitTests.Aggregates;

public class TodoListTests : IClassFixture<TodoListFixture>
{
    private readonly TodoListFixture _fixture;

    public TodoListTests(TodoListFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void AddItem_WithValidData_ShouldAddSuccessfully()
    {
        _fixture.MockRepository.Setup(r => r.ExistsItemById(It.IsAny<int>())).Returns(false);

        _fixture.TodoList.AddItem(1, "Test Title", "Test Description", "Entrantes");

        _fixture.MockRepository.Verify(r => r.SaveItem(It.Is<TodoItem>(i =>
            i.Id == 1 &&
            i.Title == "Test Title" &&
            i.Description == "Test Description" &&
            i.Category == "Entrantes")), Times.Once);
    }

    [Fact]
    public void AddItem_WithExistingId_ShouldThrowException()
    {
        _fixture.MockRepository.Setup(r => r.ExistsItemById(1)).Returns(true);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            _fixture.TodoList.AddItem(1, "Test Title", "Test Description", "Entrantes"));

        Assert.Contains("Ya existe un TodoItem con Id", exception.Message);
    }

    [Fact]
    public void AddItem_WithValidData_ShouldCallRepositorySaveItem()
    {
        const int id = 1;
        const string title = "Test Title";
        const string description = "Test Description";
        const string category = "Entrantes";

        _fixture.MockRepository.Reset();

        _fixture.MockRepository.Setup(r => r.GetItemById(id))
            .Returns((TodoItem)null);

        _fixture.MockRepository.Setup(r => r.GetAllCategories())
            .Returns(new List<string> { "Entrantes", "Platos principales", "Postres" });

        _fixture.TodoList.AddItem(id, title, description, category);

        _fixture.MockRepository.Verify(r => r.SaveItem(It.Is<TodoItem>(item =>
            item.Id == id &&
            item.Title == title &&
            item.Description == description &&
            item.Category == category)), Times.Once);
    }

    [Fact]
    public void AddItem_WithInvalidCategory_ShouldThrowInvalidOperationException()
    {
        const string invalidCategory = "Categoría Inválida";

        var exception = Assert.Throws<InvalidOperationException>(() =>
            _fixture.TodoList.AddItem(1, "Test Title", "Test Description", invalidCategory));

        Assert.Contains(invalidCategory, exception.Message);
    }

    [Fact]
    public void AddItem_WithExistingId_ShouldThrowInvalidOperationException()
    {
        var existingId = 1;
        var existingItem = new TodoItem(existingId, "Existing Title", "Existing Description", "Entrantes");

        _fixture.MockRepository.Setup(r => r.ExistsItemById(existingId))
            .Returns(true);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            _fixture.TodoList.AddItem(existingId, "Test Title", "Test Description", "Entrantes"));

        Assert.Contains(existingId.ToString(), exception.Message);
    }

    [Fact]
    public void UpdateItem_WithValidData_ShouldUpdateAndSaveItem()
    {
        const int id = 1;
        const string oldDescription = "Old Description";
        const string newDescription = "New Description";
        var todoItem = new TodoItem(id, "Test Title", oldDescription, "Entrantes");

        _fixture.MockRepository.Setup(r => r.GetItemById(id)).Returns(todoItem);

        _fixture.TodoList.UpdateItem(id, newDescription);

        Assert.Equal(newDescription, todoItem.Description);
        _fixture.MockRepository.Verify(r => r.SaveItem(It.Is<TodoItem>(item =>
            item.Id == id && item.Description == newDescription)), Times.Once);
    }

    [Fact]
    public void RemoveItem_WithValidId_ShouldCallRepositoryDeleteItem()
    {
        const int id = 1;
        var todoItem = new TodoItem(id, "Test Title", "Test Description", "Entrantes");
        todoItem.AddProgression(DateTime.Now, 20m); // 20% completado

        _fixture.MockRepository.Setup(r => r.GetItemById(id)).Returns(todoItem);

        _fixture.TodoList.RemoveItem(id);

        _fixture.MockRepository.Verify(r => r.DeleteItem(id), Times.Once);
    }

    [Fact]
    public void RemoveItem_WithMoreThan50PercentComplete_ShouldThrowInvalidOperationException()
    {
        const int id = 1;
        var todoItem = new TodoItem(id, "Test Title", "Test Description", "Entrantes");
        todoItem.AddProgression(DateTime.Now, 51m); // 51% completado

        _fixture.MockRepository.Reset();

        _fixture.MockRepository.Setup(r => r.GetItemById(id))
            .Returns(todoItem);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            _fixture.TodoList.RemoveItem(id));

        Assert.Contains("50%", exception.Message);
    }

    [Fact]
    public void RegisterProgression_WithValidData_ShouldAddProgressionAndSaveItem()
    {
        const int id = 1;
        var todoItem = new TodoItem(id, "Test Title", "Test Description", "Entrantes");
        var date = DateTime.Now;
        var percent = 25m;

        _fixture.MockRepository.Setup(r => r.GetItemById(id)).Returns(todoItem);

        _fixture.TodoList.RegisterProgression(id, date, percent);

        Assert.Single(todoItem.Progressions);
        Assert.Equal(date, todoItem.Progressions.First().Date);
        Assert.Equal(percent, todoItem.Progressions.First().Percent);
        _fixture.MockRepository.Verify(r => r.SaveItem(It.Is<TodoItem>(item =>
            item.Id == id && item.Progressions.Count == 1)), Times.Once);
    }

    [Fact]
    public void GetAllItems_ShouldReturnRepositoryItems()
    {
        var items = new List<TodoItem>
        {
            new TodoItem(1, "Item 1", "Description 1", "Entrantes"),
            new TodoItem(2, "Item 2", "Description 2", "Postres")
        };

        _fixture.MockRepository.Setup(r => r.GetAllItems()).Returns(items);

        var result = _fixture.TodoList.GetAllItems().ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(items[0].Id, result[0].Id);
        Assert.Equal(items[1].Id, result[1].Id);
        _fixture.MockRepository.Verify(r => r.GetAllItems(), Times.Once);
    }
}