using NorthSound.BLL.Facades.Base;
using NorthSound.BLL.Other;
using NorthSound.BLL.Tokens;
using NorthSound.Domain;
using NorthSound.Domain.POCO;
using System;
using System.Threading.Tasks;

namespace NorthSound.BLL.Facades;

public class AuthenticateWeb : IAuthenticateWeb, IDisposable
{
    private readonly ITokenStorage _tokenStorage;
    private readonly IRemoteAccountRepository _remoteRepository;
    private readonly IAccountInformationStorage _accountInfo;

    public AuthenticateWeb(
        ITokenStorage tokenHandler,
        IRemoteAccountRepository remoteRepository,
        IAccountInformationStorage accountInfo)
    {
        _tokenStorage = tokenHandler;
        _remoteRepository = remoteRepository;
        _accountInfo = accountInfo;
    }

    public async Task<bool> HaveAccesssRights()
    {
        if (_tokenStorage.ActualToken is null)
            return false;

        return await _remoteRepository.HaveAccessRights(_tokenStorage.ActualToken);
    }

    public async Task<Response<JwtToken>> LoginAsync(string username, string password)
    {
        var loginModel = new LoginModel
        {
            Username = username,
            Password = password,
        };

        var response = new Response<string>();

        try
        {
            response = await _remoteRepository.GetJwtTokenAsync(loginModel);
        }
        catch (Exception)
        {
            return Response<JwtToken>.Failed($"Ошибка! {response.Details}");
        }

        if (response.Status is not ResponseStatus.Success)
        {
            return Response<JwtToken>.Failed($"Ошибка! {response.Details}");
        }

        UpdateAccountInformation(username);

        return SaveToken(response.Data);
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

        return SaveToken(response.Data);
    }

    private Response<JwtToken> SaveToken(string untrimmedToken)
    {
        var trimmedToken = untrimmedToken.Trim(new char[] { ' ', '/', '\\', '"' });

        var jwtToken = new JwtToken
        {
            Token = trimmedToken
        };

        _tokenStorage.UpdateToken(jwtToken);

        return Response<JwtToken>.Success(jwtToken);
    }

    private void UpdateAccountInformation(string username)
    {
        var accountInfo = new AccountInformation(username);
        _accountInfo.Update(accountInfo);
    }

    public void Dispose()
    {
        _remoteRepository.Dispose();
        GC.SuppressFinalize(this);
    }
}
