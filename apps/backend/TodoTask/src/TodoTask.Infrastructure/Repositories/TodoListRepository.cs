using Microsoft.Extensions.Caching.Memory;
using TodoTask.Domain.Entities;
using TodoTask.Domain.Interfaces;

namespace TodoTask.Infrastructure.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly IMemoryCache _cache;

    private readonly List<string> _categories =
        ["Entrantes", "Platos principales", "Postres", "Bebidas", "Menú del día"];

    private const string CacheKeyAllItems = "TodoItems_All";
    private const string CacheKeyLastId = "TodoItems_LastId";

    public TodoListRepository(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        InitializeCache();
    }

    public int GetNextId()
    {
        if (!_cache.TryGetValue(CacheKeyLastId, out int lastId))
        {
            lastId = 0;
            _cache.Set(CacheKeyLastId, lastId);
        }

        lastId++;
        _cache.Set(CacheKeyLastId, lastId);
        return lastId;
    }

    public IEnumerable<string> GetAllCategories()
    {
        return _categories.ToList();
    }

    public void SaveItem(TodoItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "No se puede guardar un item nulo");
        }

        var items = GetItemsCollection();
        var existingItem = items.FirstOrDefault(i => i.Id == item.Id);

        if (existingItem != null)
        {
            var index = items.IndexOf(existingItem);
            items[index] = item;
        }
        else
        {
            items.Add(item);
        }

        _cache.Set(CacheKeyAllItems, items);
    }

    public void DeleteItem(int id)
    {
        var items = GetItemsCollection();
        var itemToRemove = items.FirstOrDefault(i => i.Id == id);

        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
            _cache.Set(CacheKeyAllItems, items);
        }
        else
        {
            throw new InvalidOperationException($"No existe un TodoItem con Id {id}.");
        }
    }

    public TodoItem GetItemById(int id)
    {
        var items = GetItemsCollection();
        var item = items.FirstOrDefault(i => i.Id == id);

        if (item == null)
        {
            throw new InvalidOperationException($"No existe un TodoItem con Id {id}.");
        }

        return item;
    }

    public bool ExistsItemById(int id)
    {
        var items = GetItemsCollection();
        return items.Any(i => i.Id == id);
    }

    public IEnumerable<TodoItem> GetAllItems()
    {
        var items = GetItemsCollection();
        return items.ToList();
    }

    private void InitializeCache()
    {
        if (_cache.TryGetValue(CacheKeyAllItems, out List<TodoItem>? existingItems) &&
            existingItems is { Count: > 0 })
        {
            return;
        }

        _cache.Set(CacheKeyLastId, 3);

        var sampleItems = new List<TodoItem>
        {
            CreatePaellaExample(),
            CreateTartaExample(),
            CreateEnsaladaExample()
        };

        _cache.Set(CacheKeyAllItems, sampleItems);
    }

    private List<TodoItem> GetItemsCollection()
    {
        if (!_cache.TryGetValue(CacheKeyAllItems, out List<TodoItem>? items) || items == null)
        {
            items = [];
            _cache.Set(CacheKeyAllItems, items);
        }

        return items;
    }

    private static TodoItem CreatePaellaExample()
    {
        var item = new TodoItem(1, "Paella para mesa 5", "Paella valenciana para 4 personas, sin marisco",
            "Platos principales");
        item.AddProgression(new DateTime(2025, 3, 18, 13, 15, 0), 10m);
        item.AddProgression(new DateTime(2025, 3, 18, 13, 30, 0), 10m);
        item.AddProgression(new DateTime(2025, 3, 18, 13, 55, 0), 20m);
        item.AddProgression(new DateTime(2025, 3, 18, 14, 45, 0), 60m);
        return item;
    }

    private static TodoItem CreateTartaExample()
    {
        var item = new TodoItem(2, "Tarta de queso para mesa 3",
            "Tarta de queso con mermelada de frutos rojos, sin gluten", "Postres");
        item.AddProgression(new DateTime(2025, 3, 18, 13, 40, 0), 25m);
        item.AddProgression(new DateTime(2025, 3, 18, 13, 55, 0), 25m);
        return item;
    }

    private static TodoItem CreateEnsaladaExample()
    {
        var item = new TodoItem(3, "Ensalada mediterránea para mesa 7",
            "Ensalada con tomate, pepino, cebolla, aceitunas y queso feta", "Entrantes");
        item.AddProgression(new DateTime(2025, 3, 18, 14, 00, 0), 10m);
        return item;
    }
}