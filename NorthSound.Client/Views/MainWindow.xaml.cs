using System.Windows;

namespace NorthSound.Client;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void BorderMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
        {
            this.DragMove();
        }
    }

    private void Minimize(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void Maximize(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            return;
        }

        WindowState = WindowState.Maximized;
    }
}