using NorthSound.Domain.POCO;
using System;

namespace NorthSound.BLL.Tokens;

public class TokenStorage : ITokenStorage
{
    public JwtToken? ActualToken { get; private set; }

    public event Action<JwtToken>? TokenUpdated;

    public void UpdateToken(JwtToken token)
    {
        ActualToken = token;
        TokenUpdated?.Invoke(token);
    }
}
