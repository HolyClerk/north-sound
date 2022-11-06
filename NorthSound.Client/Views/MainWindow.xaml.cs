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
}