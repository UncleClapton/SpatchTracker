using MahApps.Metro.Controls;
using SpatchTracker.Services;
using Clapton.Extensions;

namespace SpatchTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            RatBoard.Current.Subscribe((sender, args) =>
            {
                if (args.PropertyName == nameof(RatBoard.CurrentRescues))
                {
                    Dispatcher.Invoke(() => { this.rescueListBox.Items.Refresh(); });
                }
            });
        }
    }
}
