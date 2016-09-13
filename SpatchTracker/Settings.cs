using Clapton.Xml;
using Livet;
using System;
using System.Collections.Generic;
using System.IO;

namespace GoodTimesBot.Models
{
    [Serializable]
    public class Settings : NotificationObject
    {
        #region Static Members

        private static readonly string CurrentSettingsVersion = "2";

        #region FilePath
        public static readonly string defaultFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GoodTimesBot",
        #if DEBUG
            "Development",
            App.ProductInfo.VersionString.Replace('.', '_').Replace(' ', '_'),
#endif
            "Settings.xml");
        public static string FilePath
        {
            get
            {
                return string.IsNullOrWhiteSpace(Properties.Settings.Default.SettingsFilePath) ? defaultFilePath : Properties.Settings.Default.SettingsFilePath;
            }
            set
            {
                Properties.Settings.Default.SettingsFilePath = value;
            }
        }
        #endregion

        public static Settings Current { get; private set; }
        public static void Load()
        {
            try
            {
                Current = FilePath.ReadXml<Settings>();

                if (Current.Settings_FileVersion != CurrentSettingsVersion)
                {
                    Current = new Settings();
                    Current.SaveToFile();
                }
            }
            catch (Exception ex)
            {
                Current = new Settings();
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
        #endregion

        #region Properties

        #region Settings - FileVersion
        private string _Settings_FileVersion;

        public string Settings_FileVersion
        {
            get { return this._Settings_FileVersion; }
            set
            {
                if (this._Settings_FileVersion != value)
                {
                    this._Settings_FileVersion = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region TmiClient - Username
        private string _Username;
        public string TmiClient_Username
        {
            get { return _Username; }
            set
            {
                if (_Username != value.ToLower())
                {
                    _Username = value.ToLower();
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region TmiClient - Password
        private string _Password;
        public string TmiClient_Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = "oauth:" + value.Replace("oauth:", "");
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region TmiClient - Channel
        private string _Channel;
        public string TmiClient_Channel
        {
            get { return _Channel; }
            set
            {
                if (_Channel != value.ToLower())
                {
                    _Channel = "#" + value.Replace("#", "").ToLower();
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region CommandListener - AdminList
        private List<string> _CommandListener_AdminList;
        public List<string> CommandListener_AdminList
        {
            get { return _CommandListener_AdminList; }
            set
            {
                if (_CommandListener_AdminList != value)
                {
                    _CommandListener_AdminList = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region CommandListener - RegularList
        private List<string> _CommandListener_RegularList;
        public List<string> CommandListener_VoiceList
        {
            get { return _CommandListener_RegularList; }
            set
            {
                if(_CommandListener_RegularList != value)
                {
                    _CommandListener_RegularList = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Application - MinimizeToSysTray
        private bool _Application_MinimizeToSysTray;
        public bool Application_MinimizeToSysTray
        {
            get { return _Application_MinimizeToSysTray; }
            set
            {
                if (_Application_MinimizeToSysTray != value)
                {
                    _Application_MinimizeToSysTray = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region LoggingService - LogLevel
        private int _LoggingService_LogLevel;
        public int LoggingService_LogLevel
        {
            get { return _LoggingService_LogLevel; }
            set
            {
                if(_LoggingService_LogLevel != value)
                {
                    if (value < 0)
                        _LoggingService_LogLevel = 0;
                    else if (value > 5)
                        _LoggingService_LogLevel = 5;
                    else
                        _LoggingService_LogLevel = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public Settings()
        {
            Settings_FileVersion = CurrentSettingsVersion;
            TmiClient_Password = "";
            TmiClient_Username = "goodtimesbot";
            TmiClient_Channel = "";
            CommandListener_AdminList = new List<string>();
            CommandListener_VoiceList = new List<string>();
            Application_MinimizeToSysTray = true;

            this.PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveToFile();
        }

        #endregion

        #region Methods
        public void SaveToFile()
        {
            try
            {
                this.WriteXml(defaultFilePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            
        }
        #endregion
    }
}

