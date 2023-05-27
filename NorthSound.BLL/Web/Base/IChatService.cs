using NorthSound.Domain.Chat;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web.Base;

public interface IChatService : IAsyncDisposable
{
    event Action<Message>? MessageReceived;
    Task<ChatResult> SendMessageAsync(Message message);
}
