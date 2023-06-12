namespace NorthSound.Domain.Chat;

public class Dialogue
{
    public Dialogue(User interlocutor)
    {
        Messages = new();
        Interlocutor = interlocutor;
    }

    public User Interlocutor { get; init; }

    public List<MessagePOCO> Messages { get; private set; }

    public void AddMessage(User sender, string message)
    {
        var poco = new MessagePOCO
        {
            Username = sender.Name,
            Text = message,
        };

        Messages.Add(poco);
    }
}
