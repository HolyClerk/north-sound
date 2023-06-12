using Microsoft.AspNetCore.SignalR.Client;
using NorthSound.BLL.Web.Base;
using NorthSound.Domain.Chat;

namespace NorthSound.Infrastructure;

public class ChatMediator : IChatMediator
{
    private readonly MessageExchanger _messageExchanger;
    private readonly SessionsNotifier _sessionsNotifier;

    private readonly HubConnection _hubConnection;

    public ChatMediator(string url, string token)
    {
        var builder = new HubConnectionBuilder();

        _hubConnection = builder
            .WithUrl(url, options => options.Headers["Authorization"] = $"Bearer {token}")
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.StartAsync().Wait(); 

        _messageExchanger = new(_hubConnection);
        _sessionsNotifier = new(_hubConnection);

        _messageExchanger.MessageReceived += (message) 
            => MessageReceived?.Invoke(message);

        _sessionsNotifier.SessionsReceived += (users)
            => SessionsReceived?.Invoke(users);

        _sessionsNotifier.NewSessionConnected += (user)
            => NewSessionConnected?.Invoke(user);

        _sessionsNotifier.SessionDisconnected += (user)
            => SessionDisconnected?.Invoke(user);
    }

    /// <summary>
    /// Событие получения сообщения.
    /// </summary>
    public event Action<MessagePOCO>? MessageReceived;

    /// <summary>
    /// Событие подключения нового клиента.
    /// </summary>
    public event Action<User>? NewSessionConnected;

    /// <summary>
    /// Событие отключения клиента.
    /// </summary>
    public event Action<User>? SessionDisconnected;

    /// <summary>
    /// Событие получения информации о всех текущих сессиях (по запросу).
    /// </summary>
    public event Action<IReadOnlyList<User>>? SessionsReceived;

    /// <summary>
    /// Отправка сообщения.
    /// </summary>
    public Task<Result> SendMessageAsync(MessagePOCO message)
    {
        return _messageExchanger.SendMessageAsync(message);
    }

    /// <summary>
    /// Отправка запроса на получение информации о всех клиентах.
    /// </summary>
    public Task<Result> RequestSessionsAsync()
    {
        return _sessionsNotifier.RequestSessionsAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}