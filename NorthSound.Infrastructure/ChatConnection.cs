using Microsoft.AspNetCore.SignalR.Client;
using NorthSound.BLL.Other;
using NorthSound.Domain.Chat;

namespace NorthSound.Infrastructure;

public class ChatConnection : IChatConnection
{
    private HubConnection? _hubConnection;

    public event Action<Message>? MessageReceived;
    public event Action<User>? ClientConnected;
    public event Action<User>? ClientDisconnected;
    public event Action<IReadOnlyCollection<User>>? ClientsReceived;

    public bool IsConfigured => _hubConnection is not null;

    public IChatConnection ConfigureConnection(string url, string token)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(url, options =>
            {
                options.Headers["Authorization"] = $"Bearer {token}";
            })
            .Build();

        ConfigureMessageReceive(_hubConnection);
        ConfigureConnectectionNotification(_hubConnection);
        ConfigureDisconnectNotification(_hubConnection);
        ConfigureClientsReceive(_hubConnection);

        return this;
    }

    public async Task<Result> StartAsync()
    {
        if (_hubConnection is null)
        {
            return Result.Failed("Hub connection - null");
        }

        await _hubConnection.StartAsync();
        return Result.Ok();
    }

    public async Task<Result> SendMessageAsync(Message viewModel)
    {
        if (_hubConnection is null)
        {
            return Result.Failed("Hub connection - null");
        }

        await _hubConnection.InvokeAsync("SendMessage", viewModel);
        return Result.Ok();
    }

    public async Task<Result> SendGetClientsRequest()
    {
        if (_hubConnection is null)
        {
            return Result.Failed("Hub connection - null");
        }

        await _hubConnection.InvokeAsync("GetAllClients");
        return Result.Ok();
    }

    private void ConfigureMessageReceive(HubConnection hubConnection)
    {
        hubConnection.On<string, string>("ReceiveMessage", (sender, receivedMessage) =>
        {
            var message = new Message()
            {
                Text = receivedMessage,
                Username = sender,
            };

            MessageReceived?.Invoke(message);
        });
    }

    private void ConfigureConnectectionNotification(HubConnection hubConnection)
    {
        hubConnection.On<User>("ReceiveConnectectionNotification", (connectedUser) 
            => ClientConnected?.Invoke(connectedUser));
    }

    private void ConfigureDisconnectNotification(HubConnection hubConnection)
    {
        hubConnection.On<User>("ReceiveDisconnectedNotification", (disconnectedUser)
            => ClientDisconnected?.Invoke(disconnectedUser));
    }

    private void ConfigureClientsReceive(HubConnection hubConnection)
    {
        hubConnection.On<IEnumerable<User>>("ReceiveClients", (users) 
            => ClientsReceived?.Invoke(users.ToList()));
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is null)
            return;

        await _hubConnection.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
