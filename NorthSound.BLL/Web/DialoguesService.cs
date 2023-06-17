using NorthSound.BLL.Tokens;
using NorthSound.BLL.Web.Base;
using NorthSound.Domain;
using NorthSound.Domain.Chat;
using NorthSound.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web;

public class DialoguesService : IDialoguesService
{
    private readonly IHubService _hub;
    private readonly IAccountInformationStorage _account;

    private readonly List<Dialogue> _dialogues;

    public DialoguesService(IHubService hub, IAccountInformationStorage account)
    {
        _dialogues = new List<Dialogue>();
        _account = account;
        _hub = hub;

        _hub.MessageReceived += (message) 
            => CreateMessage(message.Username, message.Username, message.Text); 
    }

    public event Action<Dialogue>? DialogueChanged;

    public async Task<Result> SendMessageAsync(User interlocutor, string text)
    {
        if (string.IsNullOrWhiteSpace(text) || interlocutor is null)
            return Result.Failed("Ошибка! Необходимо выбрать пользователя и заполнить поле с сообщением!");

        var message = new MessagePOCO()
        {
            Username = interlocutor.Name,
            Text = text,
        };

        var result = await _hub.SendMessageAsync(message);

        if (result.Status is not ResponseStatus.Success)
            return Result.Failed(result.Details);

        if (_account.Account is not AccountInformation info)
            return Result.Failed("Невозможно получить информацию из вашего аккаунта");

        CreateMessage(info.Username, interlocutor.Name, text);
        return Result.Ok();
    }

    public Dialogue? GetDialogueWith(User interlocutor)
    {
        return _dialogues.FirstOrDefault(dialogue => dialogue.Receiver.Name == interlocutor.Name);
    }

    private void CreateMessage(User sender, User interlocutor, string message)
    {
        Dialogue? dialogue = _dialogues.FirstOrDefault(dialogue => dialogue.Receiver.Name == interlocutor.Name);

        if (dialogue is null)
        {
            dialogue = new(sender);
            _dialogues.Add(dialogue);
        }

        dialogue.AddMessage(sender, message);
        DialogueChanged?.Invoke(dialogue);
    }

    private void CreateMessage(string senderUsername, string interlocutor, string message)
    {
        var sender = new User { Name = senderUsername };
        var user = new User { Name = interlocutor };
        CreateMessage(sender, user, message);
    }
}
