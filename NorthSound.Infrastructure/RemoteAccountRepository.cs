using NorthSound.BLL.Other;
using NorthSound.Domain.POCO;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace NorthSound.Infrastructure;

public class RemoteAccountRepository : IRemoteAccountRepository
{
    private readonly HttpClient _httpClient;
    private readonly ServerInfo _serverInfo;

    public RemoteAccountRepository()
    {
        _httpClient = new HttpClient();
        _serverInfo = new ServerInfo();
    }

    public async Task<Response<JwtToken>> GetJwtTokenAsync(LoginModel loginModel)
    {
        SetHeaders();

        var json = JsonSerializer.Serialize(loginModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_serverInfo.GetLoginUrl(), content);

        if (response.IsSuccessStatusCode is false)
            return Response<JwtToken>.Failed(response.StatusCode.ToString());

        var token = new JwtToken()
        {
            Token = await response.Content.ReadAsStringAsync()
        };

        return Response<JwtToken>.Success(token);
    }

    public async Task<Response<JwtToken>> PostNewAccount(RegisterModel registerModel)
    {
        SetHeaders();

        var json = JsonSerializer.Serialize(registerModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_serverInfo.GetRegisterUrl(), content);

        if (response.IsSuccessStatusCode is false)
            return Response<JwtToken>.Failed(response.StatusCode.ToString());

        var token = new JwtToken()
        {
            Token = await response.Content.ReadAsStringAsync()
        };

        return Response<JwtToken>.Success(token);
    }

    public void SetHeaders()
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
