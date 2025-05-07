using TodoTask.API.Extensions;
using TodoTask.Application;
using TodoTask.Domain;
using TodoTask.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddInfrastructure();
builder.Services.AddDomain();
builder.Services.AddApplication();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();