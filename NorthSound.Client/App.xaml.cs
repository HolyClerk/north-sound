using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthSound.Client.ViewModels;
using NorthSound.Domain.Models;
using NorthSound.BLL.Facades;
using NorthSound.BLL.Services.AudioPlayer;
using NorthSound.BLL.Services.AudioPlayer.Base;
using NorthSound.BLL.Services.Import;
using NorthSound.BLL.Services.Import.Base;
using NorthSound.BLL.Services.Storage;
using NorthSound.BLL.Services.Storage.Base;
using System.Windows;
using NorthSound.BLL.Facades.Base;
using NorthSound.BLL.Tokens;
using NorthSound.BLL.Other;
using NorthSound.BLL.Common.Import;
using NorthSound.BLL.Common.Import.Base;
using NorthSound.Infrastructure;
using NorthSound.Client.ViewModels.Auth;
using NorthSound.BLL.Web;
using NorthSound.BLL.Web.Base;

namespace NorthSound.Client;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        var storageService = new SongStorageService();

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddScoped<MainWindow>()
                    .AddScoped<ApplicationViewModel>()
                    .AddScoped<PlayerViewModel>()
                    .AddScoped<LibraryCollectionViewModel>()
                    .AddScoped<AuthenticateViewModel>()
                    .AddScoped<OnlineLibraryViewModel>()
                    .AddScoped<ChatViewModel>();

                services.AddSingleton<IPlayer, AudioPlayer>()
                    .AddScoped<IImportService, ImportService>()
                    .AddTransient<IFileImporter, FileImporter>();

                services.AddSingleton<ICollectionObserver<SongModel>>(storageService)
                    .AddSingleton<IObservableStorage<SongModel>>(storageService);

                services.AddScoped<IRemoteSongRepository, RemoteSongRepository>()
                    .AddScoped<IRemoteAccountRepository, RemoteAccountRepository>()
                    .AddScoped<IMediaReader, MediaReader>()
                    .AddScoped<IAuthenticateWeb, AuthenticateWeb>()
                    .AddScoped<IServerInfo, ServerInfo>()
                    .AddSingleton<ITokenStorage, TokenStorage>()
                    .AddScoped<ISongsWebService, SongsWebService>();

                services.AddScoped<IChatConnection, ChatConnection>()
                    .AddScoped<IChatService, ChatService>();
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.DataContext = _host.Services.GetRequiredService<ApplicationViewModel>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}
