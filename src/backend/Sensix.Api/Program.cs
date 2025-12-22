using Sensix.Lib.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(
            "http://localhost:5173",
            "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add Lib
builder.Services.AddLibServices();

//Add Database
builder.Services.AddDatabaseService(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();