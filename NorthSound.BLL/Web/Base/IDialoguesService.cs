using NorthSound.Domain.Chat;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Web.Base;

public interface IDialoguesService
{
    event Action<Dialogue>? DialogueChanged;
    Dialogue? GetDialogueWith(User interlocutor);
    Task<Result> SendMessageAsync(User interlocutor, string text);
}