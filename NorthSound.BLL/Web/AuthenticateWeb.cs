using NorthSound.BLL.Facades.Base;
using NorthSound.BLL.Other;
using NorthSound.BLL.Tokens;
using NorthSound.Domain.POCO;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NorthSound.BLL.Facades;

public class AuthenticateWeb : IAuthenticateWeb, IDisposable
{
    private readonly ITokenHandler _tokenHandler;
    private readonly IServerInfo _serverInfo;
    private readonly HttpClient _httpClient;

    public AuthenticateWeb(
        ITokenHandler tokenHandler, 
        IServerInfo serverInfo)
    {
        _httpClient = new HttpClient();
        _tokenHandler = tokenHandler;
        _serverInfo = serverInfo;
    }

    public async Task<Response<JwtToken>> LoginAsync(string username, string password)
    {
        AddJsonHeaders();

        var url = _serverInfo.GetLoginUrl();
        var loginModel = new LoginModel()
        {
            Username = username,
            Password = password,
        };

        var json = JsonSerializer.Serialize(loginModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode is false)
            return Response<JwtToken>.Failed($"Ошибка! {response.StatusCode}");

        var token = new JwtToken()
        {
            Token = await response.Content.ReadAsStringAsync()
        };

        return Response<JwtToken>.Success(token);
    }

    public Task<Response<JwtToken>> RegisterAsync(string username, string email, string password)
    {
        throw new NotImplementedException();
    }

    private void AddJsonHeaders()
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
