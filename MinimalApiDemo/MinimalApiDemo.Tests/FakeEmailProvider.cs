using System;
using MinimalApiDemo.Api;

namespace MinimalApiDemo.Tests;

public class FakeEmailProvider : IEmailProvider
{
    public Task SendAsync(string emailAddress, string body)
    {
        // We don't want to actually send real emails when running integration tests.
        return Task.CompletedTask;
    }
}
