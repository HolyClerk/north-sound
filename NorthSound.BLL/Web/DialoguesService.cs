using NorthSound.BLL.Web.Base;
using NorthSound.Domain.Chat;
using NorthSound.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web;

public class DialoguesService : IDialoguesService
{
    private readonly List<Dialogue> _dialogues;
    private readonly IHubService _hub;

    public DialoguesService(IHubService hub)
    {
        _hub = hub;
        _dialogues = new List<Dialogue>();

        _hub.MessageReceived += OnMessageReceived;
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

        CreateMessage(interlocutor, text);
        return Result.Ok();
    }

    public Dialogue? GetDialogueWith(User interlocutor)
    {
        return _dialogues.FirstOrDefault(dialogue => dialogue.Interlocutor.Name == interlocutor.Name);
    }

    private void CreateMessage(User interlocutor, string message)
    {
        Dialogue? dialogue = _dialogues.FirstOrDefault(dialogue => dialogue.Interlocutor.Name == interlocutor.Name);

        if (dialogue is null)
        {
            dialogue = new(interlocutor);
            _dialogues.Add(dialogue);
        }

        dialogue.AddMessage(interlocutor, message);
        DialogueChanged?.Invoke(dialogue);
    }

    private void OnMessageReceived(MessagePOCO message)
    {
        var user = new User
        {
            Name = message.Username
        };

        CreateMessage(user, message.Text);
    }
}
