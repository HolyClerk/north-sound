using NorthSound.Domain.POCO;

namespace NorthSound.BLL.Tokens;

public class TokenStorage : ITokenStorage
{
    public JwtToken? ActualToken { get; private set; }

    public void UpdateToken(JwtToken token)
    {
        ActualToken = token;
    }
}
