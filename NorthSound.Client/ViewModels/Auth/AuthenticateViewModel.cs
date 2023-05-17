﻿using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Facades.Base;
using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.POCO;
using System.Threading.Channels;
using System.Windows;

namespace NorthSound.Client.ViewModels;

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

    public AsyncRelayCommand AuthenticateCommand
    {
        get => new(async execute =>
        {
            var response = await _authenticateService.LoginAsync(Login, Password);

            if (response.Status is ResponseStatus.Failed)
            {
                MessageBox.Show("Неудачный вход!");
            }

        }, canExecute => !string.IsNullOrWhiteSpace(_login) && !string.IsNullOrWhiteSpace(_password));
    }
}