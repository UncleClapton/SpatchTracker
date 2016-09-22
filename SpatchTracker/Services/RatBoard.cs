using Clapton.Extensions;
using Livet;
using SpatchTracker.Models;
using System.Linq;
using System.Collections.Generic;

namespace SpatchTracker.Services
{
    //This will probably be moved, but we will keep it here for now.
    public class RatBoard : NotificationObject
    {
        public static RatBoard Current { get; private set; }

        public static void Load()
        {
            if (Current == null)
            {
                Current = new RatBoard();
            }
        }

        private List<Rescue> CurrentRescues { get; set; }

        #region AddRescue
        public void AddRescue(Rescue newRescue)
        {
            //reject the new case if the ID conflicts, Better handling with a "conflict holding done" will be done eventually. Notify the user of it and log the error.
            if (CurrentRescues.Find(x => x.BoardID == newRescue.BoardID) != null)
            {
                LoggingService.Current.Log($"Recieived Conflicting Case ID, (Case #{newRescue.BoardID}) Rejecting new case until the old case is clear.", LogType.Error, LogLevel.Error);
                StatusService.Current.Notify($"Recieived Conflicting Case ID (Case #{newRescue.BoardID})! New case has been rejected.");
                return;
            }

            CurrentRescues.Add(newRescue);
            this.RaisePropertyChanged(nameof(CurrentRescues));
            StatusService.Current.Notify($"A new rescue has arrived!");
        }
        #endregion

        #region ClearRescue
        public void ClearRescue(Rescue rescue)
        {
            if(CurrentRescues.Contains(rescue))
            {
                CurrentRescues.Remove(rescue);
                this.RaisePropertyChanged(nameof(CurrentRescues));
                StatusService.Current.Notify($"{rescue.ClientName}'s rescue has been cleared!");
            }
        }
        public void ClearRescue(int caseID)
        {
            Rescue curRescue = CurrentRescues.Find(x => x.BoardID == caseID);
            if (curRescue != null)
            {
                ClearRescue(curRescue);
            }
        }
        public void ClearRescue(string clientName)
        {
            Rescue curRescue = CurrentRescues.Find(x => clientName.EqualsIgnoreCase(x.ClientName, x.ClientNick));
            if (curRescue != null)
            {
                ClearRescue(curRescue);
            }
        }
        #endregion

    }
}
