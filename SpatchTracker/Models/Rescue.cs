using Clapton.Collections;
using Clapton.Extensions;
using Livet;
using System;
using System.Collections.Generic;

namespace SpatchTracker.Models
{
    public class Rescue : NotificationObject
    {
        private bool active;
        public bool Active
        {
            get { return active; }
            set
            {
                if (active != value)
                {
                    active = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool codeRed;
        public bool CodeRed
        {
            get { return codeRed; }
            set
            {
                if (codeRed != value)
                {
                    codeRed = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool epic;
        public bool Epic
        {
            get { return epic; }
            set
            {
                if (epic != value)
                {
                    epic = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private DateTime createdAt;
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set
            {
                if (createdAt != value)
                {
                    createdAt = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private DateTime updatedAt;
        public DateTime UpdatedAt
        {
            get { return updatedAt; }
            set
            {
                if (updatedAt != value)
                {
                    updatedAt = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int boardID;
        public int BoardID
        {
            get { return boardID; }
            set
            {
                if (boardID != value)
                {
                    boardID = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private ObservableDictionary<string, RescueRat> assignedRats;
        public ObservableDictionary<string, RescueRat> AssignedRats
        {
            get { return assignedRats; }
            set
            {
                if (assignedRats != value)
                {
                    assignedRats = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private List<string> quotes;
        public List<string> Quotes
        {
            get { return quotes; }
            set
            {
                if (quotes != value)
                {
                    quotes = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private Platform platform;
        public Platform Platform
        {
            get { return platform; }
            set
            {
                if (platform != value)
                {
                    platform = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string clientName;
        public string ClientName
        {
            get { return clientName; }
            set
            {
                if (clientName != value)
                {
                    clientName = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string clientNick;
        public string ClientNick
        {
            get { return clientNick; }
            set
            {
                if (clientNick != value)
                {
                    clientNick = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string language;
        public string Language
        {
            get { return language; }
            set
            {
                if (language != value)
                {
                    language = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string system;
        public string System
        {
            get { return system; }
            set
            {
                if (system != value)
                {
                    system = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public Rescue ()
        {
            // Always update UpdatedAt when a property is changed, or when the collections have been updated.
            this.Subscribe((sender, args) =>
            {
                if (args.PropertyName != nameof(UpdatedAt))
                    UpdatedAt = DateTime.Now;
            });
            assignedRats.CollectionChanged += (sender, args) =>
            {
                UpdatedAt = DateTime.Now;
            };
        }
    }
}
