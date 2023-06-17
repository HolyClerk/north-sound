using NorthSound.Domain.POCO;
using System;

namespace NorthSound.BLL.Tokens;

public interface ITokenStorage
{
    public JwtToken? ActualToken { get; }
    event Action<JwtToken> TokenUpdated;

    void UpdateToken(JwtToken token);
}