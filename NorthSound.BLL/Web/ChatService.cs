using NorthSound.BLL.Other;
using NorthSound.BLL.Tokens;
using NorthSound.BLL.Web.Base;
using NorthSound.Domain.Chat;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web;

public class ChatService : IChatService
{
    private readonly IChatConnection _chat;
    private readonly IServerInfo _serverInfo;
    private readonly ITokenStorage _tokeStorage;

    public ChatService(IChatConnection chat, 
        IServerInfo serverInfo,
        ITokenStorage tokenStorage)
    {
        _chat = chat;
        _serverInfo = serverInfo;
        _tokeStorage = tokenStorage;

        _chat.MessageReceived += message 
            => MessageReceived?.Invoke(message);

        Configure();
    }

    public event Action<Message>? MessageReceived;

    public async Task<ChatResult> SendMessageAsync(Message message)
    {
        if (_chat.IsConfigured)
        {
            return await _chat.SendMessageAsync(message);
        }

        try
        {
            Configure();
            return await _chat.StartAsync();
        }
        catch (Exception e)
        {
            return ChatResult.Failed(e.Message);
        }
    }

    private void Configure()
    {
        var chatUrl = _serverInfo.GetChatUrl();
        var token = _tokeStorage.ActualToken;

        if (token is null)
            return;

        _chat.ConfigureConnection(chatUrl, token.Token);
    }

    public async ValueTask DisposeAsync()
    {
        await _chat.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
