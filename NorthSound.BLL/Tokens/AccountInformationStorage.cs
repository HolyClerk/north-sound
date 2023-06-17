using NorthSound.Domain;

namespace NorthSound.BLL.Tokens;

public class AccountInformationStorage : IAccountInformationStorage
{
    public AccountInformation? Account
    {
        get; private set;
    }

    public void Update(AccountInformation account)
    {
        Account = account;
    }
}
