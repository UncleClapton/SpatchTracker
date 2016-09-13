using Livet;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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
        public static void Load()
        {
            if (Current == null)
            {
                Current = new StatusService();
            }
        }
        #endregion

        #region StatusMessage Property
        private readonly Subject<string> _Notifier;
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
        internal StatusService()
        {
            this._Notifier = new Subject<string>();
            this._Notifier.Do(x =>
            {
                this._NotificationMessage = x;
                this.RaisePropertyChanged(nameof(StatusMessage));
            })
            .Throttle(TimeSpan.FromMilliseconds(5000))
            .Subscribe(_ =>
            {
                this._NotificationMessage = null;
                this.RaisePropertyChanged(nameof(StatusMessage));
            });
        }
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
            this._Notifier.OnNext(message);
        }
        #endregion
    }
}
