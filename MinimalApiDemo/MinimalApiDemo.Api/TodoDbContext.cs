using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Api.Todo;

namespace MinimalApiDemo.Api;

public class TodoDbContext : DbContext
{
    public DbSet<TodoItem> Todos => Set<TodoItem>();

    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }
}