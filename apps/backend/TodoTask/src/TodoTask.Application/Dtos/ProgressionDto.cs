namespace TodoTask.Application.Dtos;

public record ProgressionDto(
    DateTime Date,
    decimal Percent,
    decimal AccumulatedPercent
);