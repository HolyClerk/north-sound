using NorthSound.Domain;

namespace NorthSound.BLL.Tokens;

public interface IAccountInformationStorage
{
    AccountInformation? Account { get; }
    void Update(AccountInformation account);
}