using NorthSound.BLL.Facades.Base;
using NorthSound.BLL.Other;
using NorthSound.BLL.Tokens;
using NorthSound.Domain.POCO;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Facades;

public class AuthenticateWeb : IAuthenticateWeb, IDisposable
{
    private readonly ITokenHandler _tokenHandler;
    private readonly IRemoteAccountRepository _remoteRepository;

    public AuthenticateWeb(
        ITokenHandler tokenHandler,
        IRemoteAccountRepository remoteRepository)
    {
        _tokenHandler = tokenHandler;
        _remoteRepository = remoteRepository;
    }

    public async Task<Response<JwtToken>> LoginAsync(string username, string password)
    {
        var loginModel = new LoginModel
        {
            Username = username,
            Password = password,
        };

        var response = await _remoteRepository.GetJwtTokenAsync(loginModel);

        if (response.Status is not ResponseStatus.Success)
            return Response<JwtToken>.Failed($"Ошибка! {response.Details}");

        _tokenHandler.UpdateToken(response.Data);
        return response;
    }

    public async Task<Response<JwtToken>> RegisterAsync(string username, string email, string password)
    {
        var registerModel = new RegisterModel
        {
            Username = username,
            Email = email,
            Password = password
        };

        var response = await _remoteRepository.PostNewAccount(registerModel);

        if (response.Status is not ResponseStatus.Success)
            return Response<JwtToken>.Failed($"Ошибка! {response.Details}");

        _tokenHandler.UpdateToken(response.Data);
        return response;
    }

    public void Dispose()
    {
        _remoteRepository.Dispose();
        GC.SuppressFinalize(this);
    }
}
