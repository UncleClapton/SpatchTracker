using Clapton.Xml;
using Livet;
using System;
using System.IO;

namespace SpatchTracker.Services
{
    [Serializable]
    public class Settings : NotificationObject
    {
        #region Static Members

        private static readonly string settings_file_version = "1";

        #region FilePath
        public static string SETTINGS_FILE_PATH
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Properties.Settings.Default.AppConfigFilePath) ? Properties.Settings.Default.AppConfigFilePath : Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SpatchTracker",
#if DEBUG
            "Development",
            App.ProductInfo.VersionString.Replace('.', '_').Replace(' ', '_'),
#endif
            "AppConfig.xml");
            }
            set
            {
                Properties.Settings.Default.AppConfigFilePath = value;
            }
        }
        #endregion

        public static Settings Current { get; private set; }
        public static void Load()
        {
            try
            {
                Current = Clapton.Xml.IO.ReadXml<Settings>(SETTINGS_FILE_PATH);

                if (Current.SettingsFileVersion != settings_file_version)
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
        private string _SettingsFileVersion;
        public string SettingsFileVersion
        {
            get { return this._SettingsFileVersion; }
            set
            {
                if (this._SettingsFileVersion != value)
                {
                    this._SettingsFileVersion = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Logger - LogLevel
        private int? _LoggerLevel;
        public int LoggerLevel
        {
            get { return _LoggerLevel ?? 4; }
            set
            {
                if (this._LoggerLevel != value)
                {
                    this._LoggerLevel = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Logger - LogLevel
        private int? _receiverPort;
        public int ReceiverPort
        {
            get { return _receiverPort ?? 4378; }
            set
            {
                if (this._receiverPort != value)
                {
                    this._receiverPort = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion


        #endregion

        #region Constructor

        public Settings()
        {
            SettingsFileVersion = settings_file_version;

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
                this.WriteXml(SETTINGS_FILE_PATH);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            
        }
        #endregion
    }
}

