using Livet;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoodTimesBot.Models
{
    /// <summary>
    /// Handles application logging
    /// </summary>
    public class LoggingService : NotificationObject
    {
        #region Static Members
        public static LoggingService Current { get; private set; }

        public static void Load()
        {
            Current = new LoggingService();
        }

        /// <summary>
        /// sends test messages to the current logger for debug purposes.
        /// </summary>
        public static void Test()
        {
            if (Current != null)
            {
                Current.Log("Incoming", LogType.Incoming, LogLevel.Info);
                Current.Log("Outgoing", LogType.Outgoing, LogLevel.Info);
                Current.Log("Info", LogType.Info, LogLevel.Info);
                Current.Log("Error", LogType.Error, LogLevel.Error);
                Current.Log("Alert", LogType.Alert, LogLevel.Error);
            }
        }
        #endregion

        #region Private Members
        private string _SessionLogFilePath;
        #endregion

        #region Properties
        #region LogBuffer Property
        private List<ListBoxItem> _LogBuffer;
        public List<ListBoxItem> LogBuffer
        {
            get { return _LogBuffer; }
            private set
            {
                if (_LogBuffer != value)
                {
                    _LogBuffer = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region LogBufferStringList Property
        public List<string> LogBufferStringList
        {
            get
            {
                List<string> textLogs = new List<string>();
                foreach (ListBoxItem item in LogBuffer) { textLogs.Add(item.Content.ToString()); }
                return textLogs;
            }
        }
        #endregion
        #endregion

        #region Constructor
        internal LoggingService()
        {
            _SessionLogFilePath = GetNewSessionLogFilePath();
            LogBuffer = new List<ListBoxItem>();
        }
        #endregion

        #region Methods

        #region Log
        public void Log(string message, LogType logType, LogLevel logLevel)
        {
            if (Settings.Current.LoggingService_LogLevel < Convert.ToInt32(logLevel)) return;

            ListBoxItem newLog = new ListBoxItem();

            string prefix;
            switch (logType)
            {
                case LogType.Incoming:
                    prefix = "<<";
                    newLog.Foreground = Brushes.LightGray;
                    break;
                case LogType.Outgoing:
                    prefix = ">>";
                    newLog.Foreground = Brushes.LightGray;
                    break;
                case LogType.Info:
                    prefix = "**";
                    newLog.Foreground = Brushes.DimGray;
                    break;
                case LogType.Error:
                    prefix = "XX";
                    newLog.Background = Brushes.Yellow;
                    newLog.Foreground = Brushes.Black;
                    break;
                case LogType.Alert:
                    prefix = "!!";
                    newLog.Background = Brushes.Red;
                    newLog.Foreground = Brushes.White;
                    newLog.BorderBrush = Brushes.Black;
                    break;
                default:
                    prefix = "??";
                    newLog.Foreground = Brushes.LightGray;
                    break;
            }

            newLog.Content = String.Format("{2:HH:mm:ss} {0} {1}", prefix, message, DateTime.Now);
            LogBuffer.Add(newLog);
            this.RaisePropertyChanged(nameof(LogBuffer));
        }
        #endregion

        #region SaveLogBuffer
        public void SaveLogBuffer()
        {
            string buffer = String.Join("\n", LogBufferStringList);
            if (buffer != null && buffer != "")
            {
                Clapton.IO.File.TryWriteToFile(_SessionLogFilePath, String.Join("\n", LogBufferStringList));
            }
        }
        #endregion

        #region ClearLogBuffer
        public void ClearLogBuffer() { ClearLogBuffer(false); }
        public void ClearLogBuffer(bool saveFirst)
        {
            if (saveFirst)
            {
                SaveLogBuffer();
            }
            LogBuffer.Clear();
            _SessionLogFilePath = GetNewSessionLogFilePath();
            this.RaisePropertyChanged(nameof(LogBuffer));
        }
        #endregion

        #region GetNewSessionLogFilePath
        private string GetNewSessionLogFilePath()
        {
            return "Log-" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".log";
        }
        #endregion

        #endregion
    }

    #region LogType enum
    public enum LogType
    {
        Incoming,
        Outgoing,
        Info,
        Error,
        Alert
    }
    #endregion

    #region LogLevel enum
    public enum LogLevel
    {
        /// <summary>
        /// Used for only Errors and Alerts
        /// </summary>
        Error = 1,
        /// <summary>
        /// Used for normal usage information
        /// </summary>
        Info = 2,
        /// <summary>
        /// Used for detailed information and raw messages.
        /// </summary>
        Verbose = 3,
        /// <summary>
        /// Used for developer debug messages
        /// </summary>
        Debug = 4,
    }
    #endregion
}
