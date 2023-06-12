using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Tokens;
using NorthSound.BLL.Web.Base;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Chat;
using NorthSound.Domain.POCO;
using NorthSound.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal sealed class ChatViewModel : ViewModelBase
{
    private readonly IHubService _hub;
    private readonly ITokenStorage _tokenStorage;

    private readonly List<User> _onlineUsers;

    public ChatViewModel(IHubService chatService, ITokenStorage tokenStorage, Reconnector reconnector)
    {
        _onlineUsers = new();
        _hub = chatService;
        _tokenStorage = tokenStorage;

        _hub.MessageReceived += OnMessageReceived;
        _hub.NewSessionConnected += OnClientConnected;
        _hub.SessionDisconnected += OnClientDisconnected;
        _hub.SessionReceived += OnClientsReceived;

        OnlineUsersCollectionView = CollectionViewSource.GetDefaultView(new List<User>());
    }

    private User? _selectedUser;
    public User? SelectedUser
    {
        get => _selectedUser;
        set => Set(ref _selectedUser, value); 
    }

    private ICollectionView _onlineUsersCollectionView = default!;
    public ICollectionView OnlineUsersCollectionView
    {
        get => _onlineUsersCollectionView;
        set => Set(ref _onlineUsersCollectionView, value);
    }

    private string? _messageText;
    public string? MessageText
    {
        get => _messageText; 
        set => Set(ref _messageText, value);
    }

    public AsyncRelayCommand SendMessageCommand
    {
        get => new (async execute => await SendMessageAsync(MessageText));
    }

    public AsyncRelayCommand ConnectCommand
    {
        get => new (async execute => await SetChatConnection());
    }

    private async Task SetChatConnection()
    {
        var url = new ServerInfo().GetChatUrl();
        var token = _tokenStorage.ActualToken?.Token;

        if (token is null)
        {
            MessageBox.Show("Проблемы с соединением.");
            return;
        }

        var mediator = new ChatMediator(url, token);
        await _hub.SetChatConnectionAsync(mediator);
        MessageBox.Show("Соединение успешно.");
    }

    private async Task SendMessageAsync(string? text)
    {
        /*if (string.IsNullOrWhiteSpace(text) || SelectedUser is null)
        {
            MessageBox.Show("Ошибка конвертирования текста в сообщение!");
            return;
        }*/

        var message = new Message()
        {
            Text = "test",
            Username = "string",
        };

        var result = await _hub.SendMessageAsync(message);

        if (result.Status is not ResponseStatus.Success)
        {
            MessageBox.Show(result.Details);
        }
    }

    private void OnMessageReceived(Message obj)
    {
        MessageBox.Show($"{obj.Username} -> {obj.Text}");
    }

    private void OnClientsReceived(IReadOnlyCollection<User> users)
    {
        _onlineUsers.Clear();

        foreach (var user in users)
        {
            _onlineUsers.Add(user);
        }

        OnlineUsersCollectionView = CollectionViewSource.GetDefaultView(_onlineUsers);
    }

    private void OnClientDisconnected(User disconnectedUser)
    {
        _onlineUsers.RemoveAll(x => x.Username == disconnectedUser.Username);
        OnlineUsersCollectionView = CollectionViewSource.GetDefaultView(_onlineUsers);
    }

    private void OnClientConnected(User connectedUser)
    {
        _onlineUsers.Add(connectedUser);
        OnlineUsersCollectionView = CollectionViewSource.GetDefaultView(_onlineUsers);
    }
}
