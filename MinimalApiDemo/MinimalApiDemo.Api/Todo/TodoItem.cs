using System;
using System.Text.Json;

namespace MinimalApiDemo.Api.Todo;

public class TodoItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool Completed { get; set; }
    public DateTime? CompletedTime { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public DateTime CreatedTime { get; set; }

    public override string ToString()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(this, options);
    }
}
