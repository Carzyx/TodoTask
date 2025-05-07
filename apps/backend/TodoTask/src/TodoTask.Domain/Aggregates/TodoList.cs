using TodoTask.Domain.Entities;
using TodoTask.Domain.Interfaces;

namespace TodoTask.Domain.Aggregates;

public class TodoList : ITodoList
{
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

        try 
        {
            var existingItem = _repository.GetItemById(id);
            if (existingItem != null)
            {
                throw new InvalidOperationException($"Ya existe un TodoItem con Id {id}.");
            }
        }
        catch (InvalidOperationException) 
        {
            // Si salta esta excepción significa que no existe, lo cual es correcto
        }

        var newItem = new TodoItem(id, title, description, category);
        _repository.SaveItem(newItem);
    }

    public void UpdateItem(int id, string description)
    {
        var item = _repository.GetItemById(id);
        item.UpdateDescription(description);
        _repository.SaveItem(item);
    }

    public void RemoveItem(int id)
    {
        var item = _repository.GetItemById(id);
            
        if (item.Progressions.Sum(p => p.Percent) > 50m)
        {
            throw new InvalidOperationException("No se puede borrar un TodoItem con más del 50% realizado.");
        }
            
        _repository.DeleteItem(id);
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = _repository.GetItemById(id);
        item.AddProgression(dateTime, percent);
        _repository.SaveItem(item);
    }
    
    public IEnumerable<TodoItem> GetAllItems()
    {
        return _repository.GetAllItems();
    }
}