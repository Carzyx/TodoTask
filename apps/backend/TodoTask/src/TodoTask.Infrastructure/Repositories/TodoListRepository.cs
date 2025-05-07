using TodoTask.Domain.Interfaces;

namespace TodoTask.Infrastructure.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private int _lastId = 0;
    private readonly List<string> _categories = ["Work", "Personal", "Study", "Health", "Finance"];

    public int GetNextId()
    {
        return ++_lastId;
    }

    public IEnumerable<string> GetAllCategories()
    {
        return _categories;
    }
}