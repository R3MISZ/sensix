using Sensix.Api.Extensions;
using Sensix.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Register Services and Extensions
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomCors();
builder.Services.AddApplicationServices();
builder.Services.AddDatabaseContext(builder.Configuration);

var app = builder.Build();

// Middleware Pipeline
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

// Startup Tasks
await app.ApplyMigrationsAsync();

app.Run();