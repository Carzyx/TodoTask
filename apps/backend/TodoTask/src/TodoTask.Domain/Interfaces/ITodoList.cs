using TodoTask.Domain.Entities;

namespace TodoTask.Domain.Interfaces;

public interface ITodoList
{
    void AddItem(int id, string title, string description, string category);
    void UpdateItem(int id, string description);
    void RemoveItem(int id);
    void RegisterProgression(int id, DateTime dateTime, decimal percent);
    IEnumerable<TodoItem> GetAllItems();
}