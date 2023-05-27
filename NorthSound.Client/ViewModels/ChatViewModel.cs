using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Web.Base;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Chat;
using NorthSound.Domain.POCO;
using System.Threading.Tasks;
using System.Windows;

namespace NorthSound.Client.ViewModels;

internal sealed class ChatViewModel : ViewModelBase
{
    private readonly IChatService _chatService;

    public ChatViewModel(IChatService chatService)
    {
        _chatService = chatService;
        _chatService.MessageReceived += OnMessageReceived;
    }

    private string? _receiverUsername;
    public string? ReceiverUsername
    {
        get { return _receiverUsername; }
        set { _receiverUsername = value; }
    }

    public AsyncRelayCommand SendMessageCommand
    {
        get
        {
            return new AsyncRelayCommand(async execute => await SendMessageAsync("Hi"));
        }
    }

    private async Task SendMessageAsync(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            MessageBox.Show("Ошибка конвертирования текста в сообщение!");
            return;
        }

        var message = new Message()
        {
            Text = text,
            Username = ReceiverUsername,
        };

        var result = await _chatService.SendMessageAsync(message);

        if (result.Status is not ResponseStatus.Success)
        {
            MessageBox.Show(result.Details);
        }
    }

    private void OnMessageReceived(Message obj)
    {
        MessageBox.Show($"{obj.Username} -> {obj.Text}");
    }
}
