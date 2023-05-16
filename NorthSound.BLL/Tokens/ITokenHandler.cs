using NorthSound.Domain.POCO;

namespace NorthSound.BLL.Tokens;

public interface ITokenHandler
{
    public JwtToken? ActualToken { get; }

    void UpdateToken(JwtToken token);
}