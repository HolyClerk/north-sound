using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Web.Base;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Chat;
using NorthSound.Domain.POCO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal class DialoguesViewModel : ViewModelBase
{
    private readonly IDialoguesService _dialogueService;

    public DialoguesViewModel(IDialoguesService dialogueService)
    {
        _dialogueService = dialogueService;
        _dialogueService.DialogueChanged += OnDialogueChanged;

        SelectedDialogueCollectionView = CollectionViewSource.GetDefaultView(new List<Dialogue>());
    }

    private ICollectionView _selectedDialogueCollectionView = default!;
    public ICollectionView SelectedDialogueCollectionView
    {
        get => _selectedDialogueCollectionView;
        set => Set(ref _selectedDialogueCollectionView, value);
    }

    private User? _selectedUser;
    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (value is null)
                return;

            Set(ref _selectedUser, value);
            SelectDialogueWith(value);
        }
    }

    private string? _messageText;
    public string? MessageText
    {
        get => _messageText;
        set => Set(ref _messageText, value);
    }

    public AsyncRelayCommand SendMessageCommand
    {
        get => new(async execute => await SendMessageAsync());
    }

    private async Task SendMessageAsync()
    {
        if (SelectedUser is null || string.IsNullOrEmpty(MessageText))
        {
            MessageBox.Show("Текст не заполнен, или пользователь не выбран!");
            return;
        }

        var result = await _dialogueService.SendMessageAsync(SelectedUser, MessageText);

        if (result.Status is not ResponseStatus.Success)
        {
            MessageBox.Show(result.Details);
        }
    }

    private void SelectDialogueWith(User user)
    {
        var dialogue = _dialogueService.GetDialogueWith(user);

        if (dialogue is not null)
        {
            SelectedDialogueCollectionView = CollectionViewSource.GetDefaultView(dialogue.Messages);
            return;
        }

        SelectedDialogueCollectionView = CollectionViewSource.GetDefaultView(null);
    }

    private void OnDialogueChanged(Dialogue dialogue)
    {
        if (SelectedUser is null)
            return;

        if (dialogue.Receiver.Name == SelectedUser.Name)
        {
            SelectedDialogueCollectionView = CollectionViewSource.GetDefaultView(dialogue.Messages);
        }
    }
}
