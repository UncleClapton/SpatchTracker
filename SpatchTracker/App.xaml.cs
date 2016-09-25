using Clapton.Exceptions;
using Livet;
using SpatchTracker.Services;
using System;
using System.Windows;

namespace SpatchTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ProductInfo ProductInfo { get; private set; }
        private Net.ChatReceiver ListenerReceiver { get; set; }
        
        
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => ExceptionHandling.ReportException(sender, args.ExceptionObject as Exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); 

            //Record unhandled exceptions, save to file.
            Dispatcher.UnhandledException += (sender, args) => ExceptionHandling.ReportException(sender, args.Exception);
            DispatcherHelper.UIDispatcher = Dispatcher;

            //Initalize Product Info.
            ProductInfo = new ProductInfo();

            Settings.Load();

            StatusService.Load("Starting up...");
            StatusService.Current.Notify("Welcome to SpatchTracker " + ProductInfo.VersionString);

            LoggingService.Load();
            LoggingService.Current.Log(nameof(App), "AppSessionBegin - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LogLevel.Info);

            MainWindow = new MainWindow();
            MainWindow.Show();

            MainWindow.Closing += MainWindow_Closing;

            ListenerReceiver = new Net.ChatReceiver();

            StatusService.Current.Set("Ready.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Settings.Current.SaveToFile();
            LoggingService.Current.Log(nameof(App), "AppSessionEnd - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LogLevel.Info);
            ListenerReceiver.Dispose();
        }

        #region MainWindow_Closing
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RequestShutdown();
        }
        #endregion

        #region RequestShutdown
        public void RequestShutdown(int exitCode = 0)
        {
            Shutdown(exitCode);
        }
        #endregion




    }
}
