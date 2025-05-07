namespace TodoTask.Domain.ValueObjects;

public record Progression
{
    public DateTime Date { get; }
    public decimal Percent { get; }

    private Progression(DateTime date, decimal percent)
    {
        Date = date;
        Percent = percent;
    }

    public static Progression Create(DateTime date, decimal percent)
    {
        if (percent <= 0 || percent >= 100)
        {
            throw new ArgumentException("El porcentaje debe ser mayor a 0 y menor a 100.", nameof(percent));
        }

        return new Progression(date, percent);
    }
}