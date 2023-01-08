using System;
namespace MinimalApiDemo.Api;

public interface IEmailProvider
{
    Task SendAsync(string emailAddress, string body);
}
