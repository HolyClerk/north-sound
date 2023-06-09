using NorthSound.Domain.Chat;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace NorthSound.BLL.Web.Base;

public interface IChatMediator : IAsyncDisposable
{
    event Action<Message>? MessageReceived;
    event Action<User>? NewSessionConnected;
    event Action<User>? SessionDisconnected;
    event Action<IReadOnlyCollection<User>>? SessionsReceived;

    Task<Result> SendMessageAsync(Message message);
    Task<Result> RequestSessionsAsync();
}
