using TodoTask.Domain.ValueObjects;

namespace TodoTask.Domain.Entities;

public class TodoItem
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public List<Progression> Progressions { get; } = [];

    public bool IsCompleted => Progressions.Sum(p => p.Percent) >= 100m;

    public TodoItem(int id, string title, string description, string category)
    {
        if (id <= 0) throw new ArgumentException("Id debe ser mayor que cero", nameof(id));

        Id = id;

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título no puede estar vacío", nameof(title));

        Title = title;

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción no puede estar vacía", nameof(description));

        Description = description;

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("La categoría no puede estar vacía", nameof(category));

        Category = category;
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción no puede estar vacía", nameof(description));

        if (Progressions.Sum(p => p.Percent) > 50m)
            throw new InvalidOperationException("No se puede actualizar un TodoItem con más del 50% realizado.");

        Description = description;
    }

    public void AddProgression(DateTime date, decimal percent)
    {
        if (Progressions.Count > 0 && Progressions.Max(p => p.Date) >= date)
            throw new InvalidOperationException("La fecha de la nueva progresión debe ser mayor a las existentes.");

        decimal totalPercent = Progressions.Sum(p => p.Percent) + percent;
            
        if (totalPercent > 100m)
            throw new InvalidOperationException($"El porcentaje total ({totalPercent}%) supera el 100%.");

        Progressions.Add(Progression.Create(date, percent));
    }

    public decimal GetAccumulatedPercentAt(int index)
    {
        return Progressions.Take(index + 1).Sum(p => p.Percent);
    }
}