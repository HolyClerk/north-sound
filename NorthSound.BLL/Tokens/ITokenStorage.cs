using NorthSound.Domain.POCO;

namespace NorthSound.BLL.Tokens;

public interface ITokenStorage
{
    public JwtToken? ActualToken { get; }

    void UpdateToken(JwtToken token);
}