using TodoTask.Domain.Entities;
using TodoTask.Domain.Interfaces;

namespace TodoTask.Domain.Aggregates;

public class TodoList : ITodoList
{
    private readonly List<TodoItem> _items = [];
    private readonly ITodoListRepository _repository;

    public TodoList(ITodoListRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public void AddItem(int id, string title, string description, string category)
    {
        var validCategories = _repository.GetAllCategories();
        if (!validCategories.Contains(category))
        {
            throw new InvalidOperationException($"La categoría '{category}' no es válida.");
        }

        if (_items.Any(i => i.Id == id))
        {
            throw new InvalidOperationException($"Ya existe un TodoItem con Id {id}.");
        }

        _items.Add(new TodoItem(id, title, description, category));
    }

    public void UpdateItem(int id, string description)
    {
        var item = GetItemById(id);
        item.UpdateDescription(description);
    }

    public void RemoveItem(int id)
    {
        var item = GetItemById(id);
            
        if (item.Progressions.Sum(p => p.Percent) > 50m)
        {
            throw new InvalidOperationException("No se puede borrar un TodoItem con más del 50% realizado.");
        }
            
        _items.Remove(item);
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = GetItemById(id);
        item.AddProgression(dateTime, percent);
    }

    public void PrintItems()
    {
        foreach (var item in _items.OrderBy(i => i.Id))
        {
            Console.WriteLine($"{item.Id}) {item.Title} - {item.Description} ({item.Category}) Completed:{item.IsCompleted}");
                
            for (var i = 0; i < item.Progressions.Count; i++)
            {
                var progression = item.Progressions[i];
                var accumulatedPercent = item.GetAccumulatedPercentAt(i);
                var progressBar = GenerateProgressBar(accumulatedPercent);
                    
                Console.WriteLine($"{progression.Date} - {accumulatedPercent}% |{progressBar}|");
            }
        }
    }

    private string GenerateProgressBar(decimal percent)
    {
        int barLength = 50;
        int filledLength = (int)(barLength * percent / 100);
        return new string('O', filledLength);
    }

    private TodoItem GetItemById(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            throw new InvalidOperationException($"No existe un TodoItem con Id {id}.");
        }
        return item;
    }
}