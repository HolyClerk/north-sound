using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Facades.Base;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.POCO;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace NorthSound.Client.ViewModels.Auth;

class AuthenticateViewModel : ViewModelBase
{
    private readonly IAuthenticateWeb _authenticateService;

    public AuthenticateViewModel(IAuthenticateWeb authenticate)
    {
        _authenticateService = authenticate;
    }

    private string _login;
    public string Login
    {
        get => _login;
        set => Set(ref _login, value);
    }

    private string _password;
    public string Password
    {
        get => _password;
        set => Set(ref _password, value);
    }

    private bool _isLoggedIn;
    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set => Set(ref _isLoggedIn, value);
    }

    public AsyncRelayCommand AuthenticateCommand
    {
        get => new(async execute =>
        {
            await Authenticate();

        }, canExecute => true);
    }

    private async Task Authenticate()
    {
        var response = await _authenticateService.LoginAsync(Login, Password);

        if (response.Status is ResponseStatus.Failed)
            MessageBox.Show("Неудачный вход!");

        var haveRights = await _authenticateService.HaveAccesssRights();

        if (haveRights is false)
        {
            MessageBox.Show("Не удалось корректно использовать токен. Ошибка прав доступа!");
        }
        else
        {
            IsLoggedIn = true;
            MessageBox.Show("Успешный вход в аккаунт!");
        }
    }
}
