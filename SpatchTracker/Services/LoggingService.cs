using Livet;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SpatchTracker.Services
{
    /// <summary>
    /// Handles application logging
    /// </summary>
    public class LoggingService : NotificationObject
    {
        #region Static Members
        public static LoggingService Current { get; private set; }

        public static void Load(string logFilePath = null)
        {
            Current = new LoggingService(logFilePath);
        }

        /// <summary>
        /// sends test messages to the current logger for debug purposes.
        /// </summary>
        public static void Test()
        {
            if (Current != null)
            {
                Current.Log(nameof(LoggingService), "Incoming", LogLevel.Info);
                Current.Log(nameof(LoggingService), "Outgoing", LogLevel.Info);
                Current.Log(nameof(LoggingService), "Info", LogLevel.Info);
                Current.Log(nameof(LoggingService), "Error", LogLevel.Error);
                Current.Log(nameof(LoggingService), "Alert", LogLevel.Error);
            }
        }
        #endregion

        #region Private Members
        private string _SessionLogFilePath;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructs an instance of LoggingService.
        /// </summary>
        /// <param name="logFilePath">File path of the log to save to.</param>
        internal LoggingService(string logFilePath = null)
        {
            _SessionLogFilePath = logFilePath ?? GetLogPath();
        }
        #endregion

        #region Methods

        #region Log
        public void Log(string sender, string message, LogLevel logLevel)
        {
            if (Settings.Current.LoggerLevel < Convert.ToInt32(logLevel)) return;

            Clapton.IO.File.TryWriteToFile(_SessionLogFilePath, String.Format("{0:HH:mm:ss} | {1} | {2} | {3}",DateTime.Now, sender, logLevel.ToString(), message) + Environment.NewLine, true);
            return;
        }
        #endregion

        #region GetLogPath

        /// <summary>
        /// Returns the current log path based on Year and Month.
        /// </summary>
        private string GetLogPath()
        {
            return "debuglog-" + DateTime.Now.ToString("yyyyMM") + ".log";
        }
        #endregion

        #endregion
    }

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
