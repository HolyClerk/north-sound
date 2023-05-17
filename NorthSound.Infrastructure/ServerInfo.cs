using NorthSound.BLL.Other;

namespace NorthSound.Infrastructure;

public class ServerInfo : IServerInfo
{
    public string GetLoginUrl()
    {
        return "http://localhost:5000/api/Account/login";
    }

    public string GetRegisterUrl()
    {
        return "http://localhost:5000/api/Account/register";
    }

    public string GetLibraryUrl()
    {
        return $"http://localhost:5000/api/Library/";
    }

    public string GetCurrentSongUrl(int id)
    {
        return $"http://localhost:5000/api/Library/{id}";
    }

    public string GetBaseUrl()
    {
        return "http://localhost:5000/";
    }
}
