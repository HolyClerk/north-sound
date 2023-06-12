using Microsoft.AspNetCore.SignalR.Client;
using NorthSound.Domain.Chat;
using System.Windows;

namespace NorthSound.Infrastructure;

internal class SessionsNotifier
{
    private readonly HubConnection _hubConnection;

    public SessionsNotifier(HubConnection hubConnection)
    {
        _hubConnection = hubConnection;
        ConfigureActions();
    }

    public event Action<IReadOnlyList<User>>? SessionsReceived;

    public event Action<User>? NewSessionConnected;
    public event Action<User>? SessionDisconnected;

    public async Task<Result> RequestSessionsAsync()
    {
        try
        {
            await _hubConnection.InvokeAsync("GetAllClients");
        }
        catch (Exception e)
        {
            return Result.Failed(e.Message);
        }

        return Result.Ok();
    }

    private void ConfigureActions()
    {
        _hubConnection.On<User>("ReceiveConnectectionNotification", (connectedUser)
            => NewSessionConnected?.Invoke(connectedUser));

        _hubConnection.On<User>("ReceiveDisconnectedNotification", (disconnectedUser)
            => SessionDisconnected?.Invoke(disconnectedUser));

        _hubConnection.On<IEnumerable<User>>("ReceiveClients", (users)
            => SessionsReceived?.Invoke(users.ToList()));
    }
}