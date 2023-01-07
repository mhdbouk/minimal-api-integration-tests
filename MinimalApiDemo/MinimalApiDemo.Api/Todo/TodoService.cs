using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace MinimalApiDemo.Api.Todo;

public class TodoService : ITodoService
{
    private readonly TodoDbContext _dbContext;

    public TodoService(TodoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TodoItem> AddItemAsync(string title)
    {
        var item = new TodoItem
        {
            Title = title,
            CreatedTime = DateTime.Now
        };

        await _dbContext.Todos.AddAsync(item);
        await _dbContext.SaveChangesAsync();

        return item;
    }

    public async Task<TodoItem?> UpdateItemAsync(int id, string title)
    {
        var item = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id);
        if (item == null)
        {
            return null;
        }
        item.Title = title;
        item.UpdatedTime = DateTime.Now;

        await _dbContext.SaveChangesAsync();

        return item;
    }

    public async Task<TodoItem?> MarkAsDoneAsync(int id)
    {
        var item = await _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id);
        if (item == null)
        {
            return null;
        }
        item.Completed = true;
        item.UpdatedTime = DateTime.Now;
        item.CompletedTime = DateTime.Now;

        await _dbContext.SaveChangesAsync();

        return item;
    }

    public Task<List<TodoItem>> GetItemsAsync() => _dbContext.Todos.ToListAsync();

    public Task<TodoItem?> GetItemAsync(int id) => _dbContext.Todos.FirstOrDefaultAsync(x => x.Id == id);
}

