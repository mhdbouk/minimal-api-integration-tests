using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Api;
using MinimalApiDemo.Api.Todo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ITodoService, TodoService>();

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    options.UseSqlite($"Data Source={Path.Join(path, "MinimalApiDemo.db")}");
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<TodoDbContext>();
db?.Database.EnsureCreated();

app.MapGet("/todo/{id}", async (int id, ITodoService todoService) =>
{
    var item = await todoService.GetItemAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(item);
});

app.MapGet("/todo/", (ITodoService todoService) => todoService.GetItemsAsync());

app.MapPost("/todo/", async (TodoItem item, ITodoService todoService) =>
{
    if (item is null)
    {
        return Results.BadRequest("Body is null");
    }
    if (string.IsNullOrWhiteSpace(item.Title))
    {
        return Results.BadRequest("Title is null");
    }
    var result = await todoService.AddItemAsync(item.Title!);
    return Results.Created($"/todo/{result.Id}", result);
});

app.MapPut("/todo/{id}", async (int id, TodoItem item, ITodoService todoService) =>
{
    var existingItem = await todoService.GetItemAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(todoService.UpdateItemAsync(id, item.Title!));
});

app.MapPut("/todo/{id}/done", async (int id, ITodoService todoService) =>
{
    var existingItem = await todoService.GetItemAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(await todoService.MarkAsDoneAsync(id));
});

app.Run();

public partial class Program { }