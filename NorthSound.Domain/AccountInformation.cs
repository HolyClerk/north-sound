namespace NorthSound.Domain;

public class AccountInformation
{
    public AccountInformation(string username)
    {
        Username = username;
        StartUpDate = DateTime.Now;
    }

    public string Username { get; private set; }

    public DateTime StartUpDate { get; }
}
