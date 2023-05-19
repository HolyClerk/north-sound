using NorthSound.BLL.Other;
using NorthSound.Domain.POCO;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;

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

    public async Task<Response<string>> GetJwtTokenAsync(LoginModel loginModel)
    {
        SetHeaders();

        var json = JsonSerializer.Serialize(loginModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_serverInfo.GetLoginUrl(), content);

        if (response.IsSuccessStatusCode is false)
            return Response<string>.Failed(response.StatusCode.ToString());

        var token = await response.Content.ReadAsStringAsync();
        return Response<string>.Success(token);
    }

    public async Task<Response<string>> PostNewAccount(RegisterModel registerModel)
    {
        SetHeaders();

        var json = JsonSerializer.Serialize(registerModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_serverInfo.GetRegisterUrl(), content);

        if (response.IsSuccessStatusCode is false)
            return Response<string>.Failed(response.StatusCode.ToString());

        var token = await response.Content.ReadAsStringAsync();
        return Response<string>.Success(token);
    }

    public async Task<bool> HaveAccessRights(JwtToken token)
    {
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var response = await _httpClient.GetAsync(_serverInfo.GetPermissionsCheckUrl());
        return response.IsSuccessStatusCode;
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
