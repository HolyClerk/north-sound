namespace NorthSound.BLL.Other;

public interface IServerInfo
{
    string GetLoginUrl();
    string GetRegisterUrl();
    string GetCurrentSongUrl(int id);
    string GetLibraryUrl();
    string GetBaseUrl();
    string GetChatUrl();
}