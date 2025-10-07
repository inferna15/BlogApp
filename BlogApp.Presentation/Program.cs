using BlogApp.Application;
using BlogApp.Infrastructure;
using BlogApp.Infrastructure.Persistence;
using BlogApp.Presentation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddPresentation(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

// Blazor CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// SSL sorunları için yorum satırına alındı
//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowBlazorApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var database = services.GetRequiredService<AppDbContext>();
        if (database.Database.IsRelational())
        {
            await database.Database.MigrateAsync();
        }

        var seeder = services.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database seeding.");
    }
}

app.Run();
