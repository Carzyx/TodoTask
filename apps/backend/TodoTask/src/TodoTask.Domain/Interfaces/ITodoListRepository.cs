using TodoTask.Domain.Entities;

namespace TodoTask.Domain.Interfaces;

public interface ITodoListRepository
{
    int GetNextId();
    IEnumerable<string> GetAllCategories();
    void SaveItem(TodoItem item);
    void DeleteItem(int id);
    TodoItem GetItemById(int id);
    IEnumerable<TodoItem> GetAllItems();
}