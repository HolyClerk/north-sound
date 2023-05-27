using NorthSound.Domain;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web.Base;

public interface IChatService : IAsyncDisposable
{
    event Action<Message>? MessageReceived;
    Task SendMessageAsync(Message message);
}
