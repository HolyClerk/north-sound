using Microsoft.AspNetCore.SignalR.Client;
using NorthSound.BLL.Other;
using NorthSound.Domain;

namespace NorthSound.Infrastructure;

public class Chat : IChat
{
    private HubConnection? _hubConnection;

    public event Action<Message>? MessageReceived;
    public bool IsConfigured => _hubConnection is not null;

    public IChat ConfigureConnection(string url, string token)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(url, options =>
            {
                options.Headers["Authorization"] = $"Bearer {token}";
            })
            .Build();

        _hubConnection.On<string, string>("ReceiveMessage", (sender, receivedMessage) =>
        {
            var message = new Message()
            {
                Text = receivedMessage,
                Username = sender,
            };

            MessageReceived?.Invoke(message);
        });

        return this;
    }

    public async Task StartAsync()
    {
        if (_hubConnection is null)
        {
            return;
        }

        await _hubConnection.StartAsync();
    }

    public async Task SendMessageAsync(Message viewModel)
    {
        if (_hubConnection is null)
        {
            return;
        }

        await _hubConnection.InvokeAsync("SendMessage", viewModel);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is null)
            return;

        await _hubConnection.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
