using Microsoft.AspNetCore.SignalR.Client;
using NorthSound.Domain.Chat;

namespace NorthSound.Infrastructure;

internal class MessageExchanger
{
    private readonly HubConnection _hubConnection;

    public MessageExchanger(HubConnection hubConnection)
    {
        _hubConnection = hubConnection;

        _hubConnection.On<string, string>("ReceiveMessage", (sender, receivedMessage) =>
        {
            var message = new MessagePOCO()
            {
                Text = receivedMessage,
                Username = sender,
            };

            MessageReceived?.Invoke(message);
        });
    }

    public event Action<MessagePOCO>? MessageReceived;

    public async Task<Result> SendMessageAsync(MessagePOCO message)
    {
        try
        {
            await _hubConnection.InvokeAsync("SendMessage", message);
        }
        catch (Exception e)
        {
            return Result.Failed(e.Message);
        }

        return Result.Ok();
    }
}