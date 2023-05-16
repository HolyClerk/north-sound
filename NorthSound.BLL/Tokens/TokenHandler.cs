using NorthSound.Domain.POCO;

namespace NorthSound.BLL.Tokens;

public class TokenHandler : ITokenHandler
{
    public JwtToken? ActualToken { get; private set; }

    public void UpdateToken(JwtToken token)
    {
        ActualToken = token;
    }
}
