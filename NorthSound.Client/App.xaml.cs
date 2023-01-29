using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthSound.Client;
using NorthSound.Client.ViewModels;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services;
using NorthSound.Infrastructure.Services.Base;
using NorthSound.Infrastructure.Services.Import;
using NorthSound.Infrastructure.Services.Import.Base;
using System.Windows;
using System.Windows.Input;

namespace NorthSound.Client;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        var repository = new SongRepository();

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<MainWindow>()
                    .AddSingleton<ApplicationViewModel>()
                    .AddSingleton<SongViewModel>()
                    .AddSingleton<ISongImporter, SongImporter>()
                    .AddSingleton<IFileImportService, FileImportService>()
                    .AddSingleton<IRepository<Song>>(repository)
                    .AddSingleton<IObservableStorage<Song>>(repository);
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
