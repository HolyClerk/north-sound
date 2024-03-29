﻿using NorthSound.Domain.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web.Base;

public interface IHubService : IAsyncDisposable
{
    event Action<MessagePOCO>? MessageReceived;
    event Action<User>? NewSessionConnected;
    event Action<User>? SessionDisconnected;
    event Action<IReadOnlyCollection<User>>? SessionReceived;

    Task<Result> SendMessageAsync(MessagePOCO message);
    Task<Result> SendGetClientsRequestAsync();
    Task<Result> SetChatConnectionAsync(IChatMediator mediator);
}
