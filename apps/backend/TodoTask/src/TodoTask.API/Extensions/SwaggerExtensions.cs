using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace TodoTask.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "TodoTask API",
                Description = "API para gestionar tareas y sus progresiones",
                Contact = new OpenApiContact
                {
                    Name = "Miguel Angel Archilla",
                    Email = "miguel@support.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
            
            c.EnableAnnotations();
            
            c.UseInlineDefinitionsForEnums();

            c.CustomSchemaIds(type => type.FullName);
        });

        return services;
    }
}