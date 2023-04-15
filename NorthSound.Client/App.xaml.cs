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
using NorthSound.DAL;
using NorthSound.DAL.Base;
using System.Windows;
using NorthSound.BLL.Facades.Base;

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
                services
                    .AddScoped<MainWindow>()
                    .AddScoped<ApplicationViewModel>()
                    .AddScoped<PlayerViewModel>()
                    .AddScoped<LibraryCollectionViewModel>()
                    .AddScoped<OnlineLibraryViewModel>();

                services
                    .AddSingleton<IPlayer, AudioPlayer>()
                    .AddScoped<IImportService, ImportService>()
                    .AddTransient<IFileImportService, FileImportService>();

                services
                    .AddSingleton<ICollectionObserver<SongModel>>(storageService)
                    .AddSingleton<IObservableStorage<SongModel>>(storageService);

                services
                    .AddScoped<IWebRepository, WebRepository>()
                    .AddScoped<IWebService, WebService>();
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
