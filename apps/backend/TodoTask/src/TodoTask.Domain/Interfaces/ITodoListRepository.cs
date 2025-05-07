namespace TodoTask.Domain.Interfaces;

public interface ITodoListRepository
{
    int GetNextId();
    IEnumerable<string> GetAllCategories();
}