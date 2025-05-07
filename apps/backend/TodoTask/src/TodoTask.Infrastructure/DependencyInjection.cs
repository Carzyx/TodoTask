using Microsoft.Extensions.DependencyInjection;
using TodoTask.Domain.Interfaces;
using TodoTask.Infrastructure.Repositories;

namespace TodoTask.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            return services;
        }
    }
}
