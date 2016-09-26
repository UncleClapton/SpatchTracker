using Clapton.Extensions;
using Livet;
using SpatchTracker.Models;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        //              case#, Rescue Info
        public Dictionary<int, Rescue> CurrentRescues { get; set; }

        public RatBoard()
        {
            CurrentRescues = new Dictionary<int, Rescue>();
        }

        #region AddRescue
        public void AddRescue(Rescue newRescue)
        {
            //reject the new case if the ID conflicts, Better handling with a "conflict holding done" will be done eventually. Notify the user of it and log the error.
            if (CurrentRescues.Where(x => x.Key == newRescue.BoardID).Count() > 0)
            {
                LoggingService.Current.Log(nameof(RatBoard), $"Recieived Conflicting Case ID, (Case #{newRescue.BoardID}) Rejecting new case until the old case is clear.",  LogLevel.Error);
                StatusService.Current.Notify($"Recieived Conflicting Case ID (Case #{newRescue.BoardID})! New case has been rejected.");
                return;
            }

            CurrentRescues.Add(newRescue.BoardID, newRescue);
            this.RaisePropertyChanged(nameof(CurrentRescues));
            LoggingService.Current.Log(nameof(RatBoard), $"New rescue arrived! Case #{newRescue.BoardID} added to the board.", LogLevel.Info);
            StatusService.Current.Notify($"A new rescue has arrived!");
        }
        #endregion

        #region ClearRescue
        public void ClearRescue(int caseID)
        {
            if(CurrentRescues.ContainsKey(caseID))
            {
                string clientName = CurrentRescues[caseID].ClientName;
                CurrentRescues.Remove(caseID);
                this.RaisePropertyChanged(nameof(CurrentRescues));
                LoggingService.Current.Log(nameof(RatBoard), $"{clientName} (Case #{caseID}) has been cleared.", LogLevel.Info);
                StatusService.Current.Notify($"{clientName}'s rescue has been cleared!");
            }
        }

        public void ClearRescue(string clientName)
        {
            //TODO
        }
        #endregion

        #region AssignRat
        private void AssignRat(Rescue rescue, RescueRat newRat)
        {
            if(CurrentRescues.Contains(rescue))
            {
                CurrentRescues.Where(x => x == rescue).First().

                this.RaisePropertyChanged(nameof(CurrentRescues));
                LoggingService.Current.Log(nameof(RatBoard), $"Rat {newRat.CmdrName} was assigned to {CurrentRescues[index].ClientName}", LogLevel.Info);
                StatusService.Current.Notify($"{newRat.CmdrName} was assigned to a case!");
            }
        }

        public void AssignRat(int boardIndex, RescueRat newRat)
        {

        }

        #endregion

    }
}
