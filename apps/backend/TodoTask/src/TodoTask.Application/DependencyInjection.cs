using Microsoft.Extensions.DependencyInjection;
using TodoTask.Application.Interfaces;
using TodoTask.Application.Services;

namespace TodoTask.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITodoTaskService, TodoTaskService>();
        return services;
    }
}