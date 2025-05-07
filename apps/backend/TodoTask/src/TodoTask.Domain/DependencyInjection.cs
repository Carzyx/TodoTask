using Microsoft.Extensions.DependencyInjection;
using TodoTask.Domain.Aggregates;
using TodoTask.Domain.Interfaces;

namespace TodoTask.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ITodoList, TodoList>();
        return services;
    }
}