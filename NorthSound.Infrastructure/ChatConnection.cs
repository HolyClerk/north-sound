using Microsoft.AspNetCore.SignalR.Client;
using NorthSound.BLL.Other;
using NorthSound.Domain.Chat;

namespace NorthSound.Infrastructure;

public class ChatConnection : IChatConnection
{
    private HubConnection? _hubConnection;

    public event Action<Message>? MessageReceived;
    public bool IsConfigured => _hubConnection is not null;

    public IChatConnection ConfigureConnection(string url, string token)
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

    public async Task<ChatResult> StartAsync()
    {
        if (_hubConnection is null)
        {
            return ChatResult.Failed("Hub connection - null");
        }

        await _hubConnection.StartAsync();
        return ChatResult.Ok();
    }

    public async Task<ChatResult> SendMessageAsync(Message viewModel)
    {
        if (_hubConnection is null)
        {
            return ChatResult.Failed("Hub connection - null");
        }

        await _hubConnection.InvokeAsync("SendMessage", viewModel);
        return ChatResult.Ok();
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is null)
            return;

        await _hubConnection.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
