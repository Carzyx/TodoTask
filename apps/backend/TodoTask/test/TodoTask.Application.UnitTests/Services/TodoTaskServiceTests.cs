using Moq;
using TodoTask.Application.Services;
using TodoTask.Domain.Entities;
using TodoTask.Domain.Interfaces;

namespace TodoTask.Application.UnitTests.Services;

public class TodoTaskServiceTests
{
    private readonly Mock<ITodoList> _mockTodoList;
    private readonly Mock<ITodoListRepository> _mockRepository;
    private readonly TodoTaskService _service;

    public TodoTaskServiceTests()
    {
        _mockTodoList = new Mock<ITodoList>();
        _mockRepository = new Mock<ITodoListRepository>();
        _mockRepository.Setup(r => r.GetNextId()).Returns(1);
        _service = new TodoTaskService(_mockTodoList.Object, _mockRepository.Object);
    }

    [Fact]
    public void CreateTodo_WithValidData_ShouldCallAddItem()
    {
        var title = "Test Title";
        var description = "Test Description";
        var category = "Test Category";

        _service.CreateTodo(title, description, category);

        _mockTodoList.Verify(l => l.AddItem(
                It.IsAny<int>(), 
                It.Is<string>(t => t == title), 
                It.Is<string>(d => d == description), 
                It.Is<string>(c => c == category)), 
            Times.Once);
    }

    [Fact]
    public void CreateTodo_WhenAddItemThrowsException_ShouldRethrowException()
    {
        var title = "Test Title";
        var description = "Test Description";
        var category = "Invalid Category";
            
        _mockTodoList.Setup(l => l.AddItem(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Throws(new InvalidOperationException("Test exception"));

        var exception = Assert.Throws<InvalidOperationException>(() => 
            _service.CreateTodo(title, description, category));
                
        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public void UpdateTodo_WithValidData_ShouldCallUpdateItem()
    {
        var id = 1;
        var description = "New Description";

        _service.UpdateTodo(id, description);

        _mockTodoList.Verify(l => l.UpdateItem(id, description), Times.Once);
    }

    [Fact]
    public void UpdateTodo_WhenUpdateItemThrowsException_ShouldRethrowException()
    {
        var id = 1;
        var description = "New Description";
            
        _mockTodoList.Setup(l => l.UpdateItem(id, description))
            .Throws(new InvalidOperationException("Test exception"));

        var exception = Assert.Throws<InvalidOperationException>(() => 
            _service.UpdateTodo(id, description));
                
        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public void DeleteTodo_WithValidId_ShouldCallRemoveItem()
    {
        var id = 1;

        _service.DeleteTodo(id);

        _mockTodoList.Verify(l => l.RemoveItem(id), Times.Once);
    }

    [Fact]
    public void DeleteTodo_WhenRemoveItemThrowsException_ShouldRethrowException()
    {
        var id = 1;
            
        _mockTodoList.Setup(l => l.RemoveItem(id))
            .Throws(new InvalidOperationException("Test exception"));

        var exception = Assert.Throws<InvalidOperationException>(() => 
            _service.DeleteTodo(id));
                
        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public void AddProgression_WithValidData_ShouldCallRegisterProgression()
    {
        var id = 1;
        var date = DateTime.Now;
        var percent = 25m;

        _service.AddProgression(id, date, percent);

        _mockTodoList.Verify(l => l.RegisterProgression(id, date, percent), Times.Once);
    }

    [Fact]
    public void AddProgression_WhenRegisterProgressionThrowsException_ShouldRethrowException()
    {
        var id = 1;
        var date = DateTime.Now;
        var percent = 25m;
            
        _mockTodoList.Setup(l => l.RegisterProgression(id, date, percent))
            .Throws(new InvalidOperationException("Test exception"));

        var exception = Assert.Throws<InvalidOperationException>(() => 
            _service.AddProgression(id, date, percent));
                
        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public void GetAllTodos_ShouldReturnAllItems()
    {
        var todoItems = new List<TodoItem>
        {
            new TodoItem(1, "Item 1", "Description 1", "Category 1"),
            new TodoItem(2, "Item 2", "Description 2", "Category 2")
        };
            
        _mockTodoList.Setup(l => l.GetAllItems()).Returns(todoItems);

        var result = _service.GetAllTodos().ToList();

        Assert.Equal(2, result.Count);
        _mockTodoList.Verify(l => l.GetAllItems(), Times.Once);
    }
}