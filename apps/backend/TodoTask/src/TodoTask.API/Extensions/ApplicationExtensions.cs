using Asp.Versioning.ApiExplorer;

namespace TodoTask.API.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"{description.GroupName}");
                }
            
                c.RoutePrefix = string.Empty;
            });
        }
    
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    
        return app;
    }
}