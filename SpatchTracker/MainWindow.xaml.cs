using MahApps.Metro.Controls;
using SpatchTracker.Services;
using Clapton.Extensions;

namespace SpatchTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public string CurrentStatusMsg { get; set; }


        public MainWindow()
        {
            this.InitializeComponent();
            this.CurrentStatusMsg = StatusService.Current.StatusMessage;
            StatusService.Current.Subscribe((sender, args) => { if(args.PropertyName == nameof(StatusService.Current.StatusMessage)) this.CurrentStatusMsg = StatusService.Current.StatusMessage; });
        }
    }
}
