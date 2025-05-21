namespace TodoTask.Application.Dtos;

public record TodoItemDto(
    int Id,
    string Title,
    string Description,
    string Category,
    bool IsCompleted,
    IEnumerable<ProgressionDto> Progressions
);