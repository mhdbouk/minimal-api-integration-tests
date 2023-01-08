using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalApiDemo.Api;

namespace MinimalApiDemo.Tests;

public class TodoWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Fake Email Provider
            var emailDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailProvider));

            services.Remove(emailDescriptor!);

            services.AddSingleton<IEmailProvider, FakeEmailProvider>();

            // Test DB Context
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TodoDbContext>));

            services.Remove(dbContextDescriptor!);

            services.AddDbContext<TodoDbContext>(options =>
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                options.UseSqlite($"Data Source={Path.Join(path, "MinimalApiDemoTests.db")}");
            });
        });

        builder.UseEnvironment("development");
    }
}

