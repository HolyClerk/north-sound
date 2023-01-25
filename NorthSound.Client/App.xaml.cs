using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthSound.Client.ViewModels;
using NorthSound.Client.ViewModels.Interfaces;
using System.Windows;

namespace NorthSound.Client
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<MainWindow>()
                        .AddSingleton<ApplicationViewModel>()
                        .AddSingleton<ISongViewModel, SongViewModel>();
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
}
