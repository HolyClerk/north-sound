using NorthSound.BLL.Tokens;
using NorthSound.BLL.Web.Base;
using NorthSound.Domain.POCO;
using NorthSound.Infrastructure;
using System.Threading.Tasks;

namespace NorthSound.Client;

public class Reconnector
{
    private readonly IHubService _hubService;

    public Reconnector(ITokenStorage tokenStorage, IHubService service)
    {
        tokenStorage.TokenUpdated += OnTokenUpdated;
        _hubService = service;
    }

    private async void OnTokenUpdated(JwtToken token)
    {
        await ReconnectChat(token);
    }

    private async Task ReconnectChat(JwtToken token)
    {
        var info = new ServerInfo();
        var mediator = new ChatMediator(info.GetChatUrl(), token.Token);

        await _hubService.SetChatConnectionAsync(mediator);
    }
}
