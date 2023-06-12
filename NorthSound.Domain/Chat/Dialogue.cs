namespace NorthSound.Domain.Chat;

public class Dialogue
{
    public Dialogue(User receiver)
    {
        Messages = new();
        Receiver = receiver;
    }

    public User Receiver { get; init; }

    public List<MessagePOCO> Messages { get; private set; }

    public void AddMessage(User owner, string message)
    {
        var poco = new MessagePOCO
        {
            Username = owner.Name,
            Text = message,
        };

        Messages.Add(poco);
    }
}
