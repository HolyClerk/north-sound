using NorthSound.Domain;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Other;

public interface IChat : IAsyncDisposable
{
    bool IsConfigured { get; }
    event Action<Message>? MessageReceived;
    IChat ConfigureConnection(string url, string token);
    Task StartAsync();
    Task SendMessageAsync(Message viewModel);
}
