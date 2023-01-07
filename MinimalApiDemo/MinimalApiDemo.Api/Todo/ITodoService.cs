namespace MinimalApiDemo.Api.Todo;

public interface ITodoService
{
    Task<TodoItem> AddItemAsync(string title);
    Task<TodoItem?> GetItemAsync(int id);
    Task<List<TodoItem>> GetItemsAsync();
    Task<TodoItem?> MarkAsDoneAsync(int id);
    Task<TodoItem?> UpdateItemAsync(int id, string title);
}