using NorthSound.BLL.Other;
using NorthSound.BLL.Tokens;
using NorthSound.BLL.Web.Base;
using NorthSound.Domain;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web;

public class ChatService : IChatService
{
    private readonly IChat _chat;
    private readonly IServerInfo _serverInfo;
    private readonly ITokenStorage _tokeStorage;

    public ChatService(IChat chat, 
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

    public async Task SendMessageAsync(Message message)
    {
        if (_chat.IsConfigured is false)
        {
            Configure();
            await _chat.StartAsync();
        }

        await _chat.SendMessageAsync(message);
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
