using NorthSound.Domain.POCO;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NorthSound.BLL.Other;

public interface IRemoteAccountRepository : IDisposable
{
    Task<bool> HaveAccessRights(JwtToken token);
    Task<Response<string>> GetJwtTokenAsync(LoginModel loginModel);
    Task<Response<string>> PostNewAccount(RegisterModel registerModel);
}