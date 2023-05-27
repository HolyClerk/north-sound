using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Web.Base;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain;
using NorthSound.Domain.Models;
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

        await _chatService.SendMessageAsync(message);
    }

    private void OnMessageReceived(Message obj)
    {
        MessageBox.Show($"{obj.Username} -> {obj.Text}");
    }
}
