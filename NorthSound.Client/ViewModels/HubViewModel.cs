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

internal sealed class HubViewModel : ViewModelBase
{
    private readonly IHubService _hub;
    private readonly ITokenStorage _tokenStorage;

    private readonly List<User> _onlineUsers;

    public HubViewModel(IHubService chatService, ITokenStorage tokenStorage, Reconnector reconnector)
    {
        _onlineUsers = new();

        _hub = chatService;
        _tokenStorage = tokenStorage;

        _hub.NewSessionConnected += OnClientConnected;
        _hub.SessionDisconnected += OnClientDisconnected;
        _hub.SessionReceived += OnClientsReceived;

        OnlineUsersCollectionView = CollectionViewSource.GetDefaultView(new List<User>());
    }

    private ICollectionView _onlineUsersCollectionView = default!;
    public ICollectionView OnlineUsersCollectionView
    {
        get => _onlineUsersCollectionView;
        set => Set(ref _onlineUsersCollectionView, value);
    }

    public AsyncRelayCommand ReceiveSessionsCommand
    {
        get => new (async execute => await SendRequest());
    }

    private async Task SendRequest()
    {
        var result = await _hub.SendGetClientsRequestAsync();

        if (result.Status is not ResponseStatus.Success)
        {
            MessageBox.Show(result.Details);
        }
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
        _onlineUsers.RemoveAll(x => x.Name == disconnectedUser.Name);
        OnlineUsersCollectionView = CollectionViewSource.GetDefaultView(_onlineUsers);
    }

    private void OnClientConnected(User connectedUser)
    {
        _onlineUsers.Add(connectedUser);
        OnlineUsersCollectionView = CollectionViewSource.GetDefaultView(_onlineUsers);
    }
}
