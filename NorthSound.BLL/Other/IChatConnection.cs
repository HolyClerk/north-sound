using NorthSound.Domain.Chat;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Other;

public interface IChatConnection : IAsyncDisposable
{
    bool IsConfigured { get; }
    event Action<Message>? MessageReceived;
    IChatConnection ConfigureConnection(string url, string token);
    Task<ChatResult> StartAsync();
    Task<ChatResult> SendMessageAsync(Message viewModel);
}
