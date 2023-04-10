using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthSound.Client.ViewModels;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services;
using NorthSound.Infrastructure.Services.AudioPlayer;
using NorthSound.Infrastructure.Services.AudioPlayer.Base;
using NorthSound.Infrastructure.Services.Base;
using NorthSound.Infrastructure.Services.Import;
using NorthSound.Infrastructure.Services.Import.Base;
using NorthSound.Infrastructure.Services.Web;
using System.Windows;

namespace NorthSound.Client;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        var storageService = new SongStorageService();
        var webLibrary = new WebLibrary();

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services
                    .AddScoped<MainWindow>()
                    .AddScoped<ApplicationViewModel>()
                    .AddScoped<PlayerViewModel>()
                    .AddScoped<LibraryCollectionViewModel>()
                    .AddScoped<OnlineLibraryViewModel>();

                services
                    .AddSingleton<IPlayer, AudioPlayer>()
                    .AddScoped<IImportService, ImportFacade>()
                    .AddScoped<ILocator, ImportFacade>()
                    .AddTransient<IFileImportService, FileImportService>();

                services
                    .AddSingleton<ICollectionObserver<SongModel>>(storageService)
                    .AddSingleton<IObservableStorage<SongModel>>(storageService);

                services
                    .AddScoped<IWebLibraryService, WebLibrary>()
                    .AddScoped<IWebStreamService, WebLibrary>();
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
