using TodoTask.Application.Dtos;

namespace TodoTask.Application.Interfaces;

public interface ITodoTaskService
{
    void CreateTodo(string title, string description, string category);
    void UpdateTodo(int id, string description);
    void DeleteTodo(int id);
    void AddProgression(int id, DateTime dateTime, decimal percent);
    IEnumerable<TodoItemDto> GetAllTodos();
    
}