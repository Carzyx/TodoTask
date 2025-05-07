using Microsoft.Extensions.Caching.Memory;
using TodoTask.Domain.Entities;
using TodoTask.Infrastructure.Repositories;

namespace TodoTask.Infrastructure.UnitTests.Repositories;

public class TodoListRepositoryTests
{
    private readonly IMemoryCache _memoryCache;
    private readonly TodoListRepository _repository;

    public TodoListRepositoryTests()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _repository = new TodoListRepository(_memoryCache);
    }

    [Fact]
    public void GetNextId_ShouldIncrementIdValue()
    {
        var firstId = _repository.GetNextId();
        var secondId = _repository.GetNextId();
        var thirdId = _repository.GetNextId();

        Assert.Equal(firstId + 1, secondId);
        Assert.Equal(secondId + 1, thirdId);
    }

    [Fact]
    public void GetAllCategories_ShouldReturnAllCategories()
    {
        var categories = _repository.GetAllCategories().ToList();

        Assert.Contains("Entrantes", categories);
        Assert.Contains("Platos principales", categories);
        Assert.Contains("Postres", categories);
        Assert.Contains("Bebidas", categories);
        Assert.Contains("Menú del día", categories);
        Assert.Equal(5, categories.Count);
    }

    [Fact]
    public void SaveItem_WithNewItem_ShouldAddItem()
    {
        var newItem = new TodoItem(100, "Test Title", "Test Description", "Entrantes");

        _repository.SaveItem(newItem);
        var result = _repository.GetItemById(100);

        Assert.Equal(newItem.Id, result.Id);
        Assert.Equal(newItem.Title, result.Title);
        Assert.Equal(newItem.Description, result.Description);
        Assert.Equal(newItem.Category, result.Category);
    }

    [Fact]
    public void SaveItem_WithExistingItem_ShouldUpdateItem()
    {
        var item = new TodoItem(100, "Original Title", "Original Description", "Entrantes");
        _repository.SaveItem(item);

        item = new TodoItem(100, "Updated Title", "Updated Description", "Entrantes");
        _repository.SaveItem(item);

        var result = _repository.GetItemById(100);

        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Description", result.Description);
    }

    [Fact]
    public void SaveItem_WithNullItem_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _repository.SaveItem(null));
    }

    [Fact]
    public void DeleteItem_WithExistingId_ShouldRemoveItem()
    {
        var item = new TodoItem(100, "Test Title", "Test Description", "Entrantes");
        _repository.SaveItem(item);

        _repository.DeleteItem(100);

        Assert.Throws<InvalidOperationException>(() => _repository.GetItemById(100));
    }

    [Fact]
    public void DeleteItem_WithNonExistingId_ShouldThrowInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => _repository.DeleteItem(999));
    }

    [Fact]
    public void GetItemById_WithExistingId_ShouldReturnItem()
    {
        var item = new TodoItem(100, "Test Title", "Test Description", "Entrantes");
        _repository.SaveItem(item);

        var result = _repository.GetItemById(100);

        Assert.Equal(item.Id, result.Id);
        Assert.Equal(item.Title, result.Title);
        Assert.Equal(item.Description, result.Description);
        Assert.Equal(item.Category, result.Category);
    }

    [Fact]
    public void GetItemById_WithNonExistingId_ShouldThrowInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => _repository.GetItemById(999));
    }

    [Fact]
    public void GetAllItems_ShouldReturnAllSavedItems()
    {
        // Limpiamos los items que el repositorio inicializa por defecto
        var initialItems = _repository.GetAllItems().ToList();
        foreach (var item in initialItems)
        {
            // Usamos try-catch porque algunos items ya podrían tener progreso > 50%
            try
            {
                _repository.DeleteItem(item.Id);
            }
            catch
            {
                // Ignoramos errores al eliminar
            }
        }

        var item1 = new TodoItem(101, "Item 1", "Description 1", "Entrantes");
        var item2 = new TodoItem(102, "Item 2", "Description 2", "Postres");
        _repository.SaveItem(item1);
        _repository.SaveItem(item2);

        var results = _repository.GetAllItems().ToList();

        Assert.Equal(2, results.Count);
        Assert.Contains(results, item => item.Id == 101);
        Assert.Contains(results, item => item.Id == 102);
    }

    [Fact]
    public void Constructor_ShouldInitializeCacheWithSampleItems()
    {
        var newRepository = new TodoListRepository(_memoryCache);
        var items = newRepository.GetAllItems().ToList();

        Assert.True(items.Count >= 3);
        Assert.Contains(items, item => item.Title.Contains("Paella"));
        Assert.Contains(items, item => item.Title.Contains("Tarta"));
        Assert.Contains(items, item => item.Title.Contains("Ensalada"));
    }

    [Fact]
    public void InitializeSampleItems_PaellaShouldHaveCompletedProgressions()
    {
        var items = _repository.GetAllItems().ToList();
        var paella = items.FirstOrDefault(i => i.Title.Contains("Paella"));

        Assert.NotNull(paella);
        Assert.True(paella.Progressions.Count > 0);
        Assert.Equal(100m, paella.Progressions.Sum(p => p.Percent));
        Assert.True(paella.IsCompleted);
    }
}