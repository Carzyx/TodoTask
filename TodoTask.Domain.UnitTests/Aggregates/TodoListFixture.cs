using Moq;
using TodoTask.Domain.Aggregates;
using TodoTask.Domain.Interfaces;

namespace TodoTask.Domain.UnitTests.Aggregates;

public class TodoListFixture : IDisposable
{
    public Mock<ITodoListRepository> MockRepository { get; }
    public TodoList TodoList { get; }
    private List<string> ValidCategories { get; } = new() { "Entrantes", "Platos principales", "Postres" };

    public TodoListFixture()
    {
        MockRepository = new Mock<ITodoListRepository>();
        MockRepository.Setup(r => r.GetAllCategories()).Returns(ValidCategories);
        TodoList = new TodoList(MockRepository.Object);
    }

    public void Dispose()
    {
        // Limpieza si es necesaria
    }
}