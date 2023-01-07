using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MinimalApiDemo.Api;
using MinimalApiDemo.Api.Todo;

namespace MinimalApiDemo.Tests;

public class TodoTests : IClassFixture<TodoWebApplicationFactory<Program>>
{
    private readonly TodoWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public TodoTests(TodoWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task AddTodoItem_ReturnsCreatedSuccess()
    {
        // Arrange
        var todoItem = new TodoItem { Title = "Cool Integration Test Item" };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(todoItem), Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/todo/", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var item = System.Text.Json.JsonSerializer.Deserialize<TodoItem>(responseContent, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(item);
        Assert.NotNull(response.Headers.Location);
        Assert.Equal(response.Headers.Location.ToString(), $"/todo/{item.Id}");
    }

    [Fact]
    public async Task GetTodoItemTest()
    {
        // Arrange
        var context = _factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetRequiredService<TodoDbContext>();
        var todoItem = await context!.Todos.AddAsync(new TodoItem { Title = "Hi", CreatedTime = DateTime.Now });
        await context!.SaveChangesAsync();

        // Act
        var response = await _httpClient.GetAsync($"/todo/{todoItem.Entity.Id}");
        var content = await response.Content.ReadAsStringAsync();
        var item = System.Text.Json.JsonSerializer.Deserialize<TodoItem>(content, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(item);
        Assert.Equal(todoItem.Entity.Id, item.Id);
    }
}