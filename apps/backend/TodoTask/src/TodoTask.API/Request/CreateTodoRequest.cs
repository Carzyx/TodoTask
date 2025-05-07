namespace TodoTask.API.Request;

public record CreateTodoRequest(string Title, string Description, string Category);
