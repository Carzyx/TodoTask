using TodoTask.Application.Dtos;
using TodoTask.Application.Interfaces;
using TodoTask.Domain.Interfaces;

namespace TodoTask.Application.Services;

public class TodoTaskService : ITodoTaskService
{
    private readonly ITodoList _todoList;
    private readonly ITodoListRepository _repository;

    public TodoTaskService(ITodoList todoList, ITodoListRepository repository)
    {
        _todoList = todoList ?? throw new ArgumentNullException(nameof(todoList));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public void CreateTodo(string title, string description, string category)
    {
        int nextId = _repository.GetNextId();
        _todoList.AddItem(nextId, title, description, category);
    }

    public void UpdateTodo(int id, string description)
    {
        _todoList.UpdateItem(id, description);
    }

    public void DeleteTodo(int id)
    {
        _todoList.RemoveItem(id);
    }

    public void AddProgression(int id, DateTime dateTime, decimal percent)
    {
        _todoList.RegisterProgression(id, dateTime, percent);
    }

    public IEnumerable<TodoItemDto> GetAllTodos()
    {
        return _todoList.GetAllItems().Select(item => new TodoItemDto(
            item.Id,
            item.Title,
            item.Description,
            item.Category,
            item.IsCompleted,
            Enumerable.Range(0, item.Progressions.Count).Select(i => new ProgressionDto(
                item.Progressions[i].Date,
                item.Progressions[i].Percent,
                item.GetAccumulatedPercentAt(i)
            ))
        ));
    }
}