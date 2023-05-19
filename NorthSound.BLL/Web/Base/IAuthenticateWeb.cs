using NorthSound.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthSound.BLL.Facades.Base;

public interface IAuthenticateWeb
{
    Task<Response<JwtToken>> RegisterAsync(string username, string email, string password);
    Task<Response<JwtToken>> LoginAsync(string username, string password);
    Task<bool> HaveAccesssRights();
}
