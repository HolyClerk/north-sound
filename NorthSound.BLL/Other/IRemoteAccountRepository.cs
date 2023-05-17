using NorthSound.Domain.POCO;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NorthSound.BLL.Other;

public interface IRemoteAccountRepository : IDisposable
{
    Task<Response<JwtToken>> GetJwtTokenAsync(LoginModel loginModel);
    Task<Response<JwtToken>> PostNewAccount(RegisterModel registerModel);
}