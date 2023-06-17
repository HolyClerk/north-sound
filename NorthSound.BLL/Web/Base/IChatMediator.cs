using NorthSound.Domain.Chat;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace NorthSound.BLL.Web.Base;

public interface IChatMediator : IAsyncDisposable
{
    event Action<MessagePOCO>? MessageReceived;
    event Action<User>? NewSessionConnected;
    event Action<User>? SessionDisconnected;
    event Action<IReadOnlyList<User>>? SessionsReceived;

    Task<Result> SendMessageAsync(MessagePOCO message);
    Task<Result> RequestSessionsAsync();
}
