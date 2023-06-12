using NorthSound.BLL.Web.Base;
using NorthSound.Domain.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web;

public class HubService : IHubService
{
    private IChatMediator? _chat;

    public event Action<User>? NewSessionConnected;
    public event Action<User>? SessionDisconnected;
    public event Action<IReadOnlyCollection<User>>? SessionReceived;
    public event Action<Message>? MessageReceived;

    public async Task<Result> SetChatConnectionAsync(IChatMediator mediator)
    {
        try
        {
            if (_chat is not null)
            {
                await _chat.DisposeAsync();
            }

            _chat = mediator;
            ConnectActions();
        }
        catch (Exception)
        {
            throw;
        }
        
        return Result.Ok();
    }

    public async Task<Result> SendMessageAsync(Message message)
    {
        if (_chat is null)
        {
            return Result.Failed("No connection");
        }

        return await _chat.SendMessageAsync(message);
    }

    public async Task<Result> SendGetClientsRequestAsync()
    {
        if (_chat is null)
        {
            return Result.Failed("No connection");
        }

        return await _chat.RequestSessionsAsync();
    }

    private void ConnectActions()
    {
        if (_chat is null)
        {
            throw new NullReferenceException("Chat Mediator is null");
        }

        _chat.MessageReceived += message
            => MessageReceived?.Invoke(message);

        _chat.NewSessionConnected += client
            => NewSessionConnected?.Invoke(client);

        _chat.SessionDisconnected += client
            => SessionDisconnected?.Invoke(client);

        _chat.SessionsReceived += clients
            => SessionReceived?.Invoke(clients);
    }

    public async ValueTask DisposeAsync()
    {
        if (_chat is not null)
        {
            await _chat.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
