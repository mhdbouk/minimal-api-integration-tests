namespace MinimalApiDemo.Api;

public class SendGridEmailProvider : IEmailProvider
{
    public Task SendAsync(string emailAddress, string body)
    {
        // Implement logic to send email using SendGrid
        return Task.CompletedTask;
    }
}

