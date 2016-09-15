using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatchTracker.Models.Data
{
    class RescueRat : Rat
    {
        private bool _isFriend;
        public bool IsFriend
        {
            get {return _isFriend; }
            set
            {
                if (_isFriend != value)
                {
                    _isFriend = value;
                    this.RaisePropertyChanged();
                    if (value)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private bool _isInSystem;
        public bool IsInSystem
        {
            get {return _isInSystem; }
            set
            {
                if (_isInSystem != value)
                {
                    _isInSystem = value;
                    this.RaisePropertyChanged();
                    if (value)
                    {
                        return;
                    }
                    else
                    {
                        HasBeaconVisual = false;
                    }
                }
            }
        }

        private bool _isInWing;
        public bool IsInWing
        {
            get {return _isInWing; }
            set
            {
                if (_isInWing != value)
                {
                    _isInWing = value;
                    this.RaisePropertyChanged();
                    if (value)
                    {
                        IsDisconnected = false;
                    }
                    else
                    {
                        HasBeaconVisual = false;
                    }
                }
            }
        }

        private bool _hasBeaconVisual;
        public bool HasBeaconVisual
        {
            get {return _hasBeaconVisual; }
            set
            {
                if (_hasBeaconVisual != value)
                {
                    _hasBeaconVisual = value;
                    this.RaisePropertyChanged();
                    if (value)
                    {
                        IsInWing = true;
                        IsInSystem = true;
                        IsDisconnected = false;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private bool _hasRefueled;
        public bool HasRefueled
        {
            get {return _hasRefueled; }
            set
            {
                if (_hasRefueled != value)
                {
                    _hasRefueled = value;
                    this.RaisePropertyChanged();
                    if (value)
                    {
                        HasBadInstance = false;
                        IsDisconnected = false;
                        IsInterdicted = false;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private bool _hasBadInstance;
        public bool HasBadInstance
        {
            get {return _hasBadInstance; }
            set
            {
                if (_hasBadInstance != value)
                {
                    _hasBadInstance = value;
                    this.RaisePropertyChanged();
                    if (value)
                    {
                        HasRefueled = false;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private bool _isDisconnected;
        public bool IsDisconnected
        {
            get {return _isDisconnected; }
            set
            {
                if (_isDisconnected != value)
                {
                    _isDisconnected = value;
                    this.RaisePropertyChanged();
                    if (value)
                    {
                        IsInWing = false;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private bool _isInterdicted;
        public bool IsInterdicted
        {
            get {return _isInterdicted; }
            set
            {
                if (_isInterdicted != value)
                {
                    _isInterdicted = value;
                    this.RaisePropertyChanged();
                    if (value)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}
