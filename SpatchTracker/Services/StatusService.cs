using Livet;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace SpatchTracker.Services
{
    /// <summary>
    /// Handles statusline messages.
    /// </summary>
    public class StatusService : NotificationObject
    {
        #region Static Members
        public static StatusService Current { get; private set; }

        /// <summary>
        /// Loads a new instance
        /// </summary>
        public static void Load(string status)
        {
            if (Current == null)
            {
                Current = new StatusService(status);
            }
        }
        #endregion

        #region StatusMessage Property
        private string _PersistentMessage = "";
        private string _NotificationMessage;

        /// <summary>
        /// Returns the active status message;
        /// </summary>
        public string StatusMessage
        {
            get { return this._NotificationMessage ?? this._PersistentMessage; }
            private set
            {
                if (this._PersistentMessage != value)
                {
                    this._PersistentMessage = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Constructor
        internal StatusService(string status) { StatusMessage = status; }
        #endregion

        #region Functions
        /// <summary>
        /// Sets the persistant status message.
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        public void Set(string message)
        {
            this.StatusMessage = message;
        }

        /// <summary>
        /// Temporarily sets a status message.
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        public void Notify(string message)
        {

            _NotificationMessage = message;
            this.RaisePropertyChanged(nameof(StatusMessage));

            Task.Run(() =>
            {
                Thread.Sleep(5000);
                _NotificationMessage = null;
                RaisePropertyChanged(nameof(StatusMessage));
            });

            return;
        }
        #endregion
    }
}
