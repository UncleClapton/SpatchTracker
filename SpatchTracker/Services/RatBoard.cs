using Clapton.Extensions;
using Livet;
using SpatchTracker.Models;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

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
            //reject the new case if the ID conflicts, Better handling with a "conflict holding list" will be done eventually. Notify the user of it, log the error with case info.
            if (CurrentRescues.Where(x => x.Key == newRescue.BoardID).Count() > 0)
            {
                LoggingService.Current.Log(nameof(RatBoard), $"Recieived Conflicting Case ID, (Case #{newRescue.BoardID}) Rejecting new case until the old case is clear.",  LogLevel.Error);
                if (Settings.Current.LoggerLevel < Convert.ToInt32(LogLevel.Debug)) LoggingService.Current.Log(nameof(RatBoard), "Rejected case info: CMDR {newRescue.ClientName} | System: {newRescue.System} | Platform : {newRescue.Platform.ToString()} | CR: {newRescue.CodeRed.ToString()} | Lang: {newRescue.Language} | IRC: {newRescue.ClientNick} | Case #{newRescue.BoardID}", LogLevel.Debug);
                StatusService.Current.Notify($"Recieived Conflicting Case ID (Case #{newRescue.BoardID}) New case has been rejected. Check logs.");
                return;
            }

            CurrentRescues.Add(newRescue.BoardID, newRescue);
            this.RaisePropertyChanged(nameof(CurrentRescues));
            LoggingService.Current.Log(nameof(RatBoard), $"Rescue added: {newRescue.ClientName} #{newRescue.BoardID} added to the board.", LogLevel.Info);
            StatusService.Current.Notify($"A new rescue has arrived.");
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
                StatusService.Current.Notify($"{clientName}'s rescue has been cleared.");
            }
        }

        public void ClearRescue(string clientName)
        {
            int? caseID = CurrentRescues.Where(x => clientName.EqualsIgnoreCase(x.Value.ClientName, x.Value.ClientNick)).First().Key;
            if (caseID.HasValue)
                ClearRescue(caseID.Value);
            return;
        }
        #endregion

        #region AssignRat
        public void AssignRat(int caseID, RescueRat newRat)
        {
            if (CurrentRescues.ContainsKey(caseID))
            {
                CurrentRescues[caseID].AssignedRats.Add(newRat.CmdrName, newRat);
                this.RaisePropertyChanged(nameof(CurrentRescues));
                CurrentRescues[caseID].UpdatedAt = DateTime.Now;
                LoggingService.Current.Log(nameof(RatBoard), $"Rat {newRat.CmdrName} was assigned to {CurrentRescues[caseID].ClientName}.", LogLevel.Info);
                StatusService.Current.Notify($"{newRat.CmdrName} was assigned to {CurrentRescues[caseID].ClientName}.");
            }
        }

        public void AssignRat(string clientName, RescueRat newRat)
        {
            int? caseID = CurrentRescues.Where(x => clientName.EqualsIgnoreCase(x.Value.ClientName, x.Value.ClientNick)).First().Key;
            if (caseID.HasValue)
                AssignRat(caseID.Value, newRat);
            return;
        }
        #endregion

        #region RemoveRat
        public void RemoveRat(int caseID, string ratName)
        {
            if (CurrentRescues.ContainsKey(caseID) && CurrentRescues[caseID].AssignedRats.ContainsKey(ratName))
            {
                CurrentRescues[caseID].AssignedRats.Remove(ratName);
                this.RaisePropertyChanged(nameof(CurrentRescues));
                CurrentRescues[caseID].UpdatedAt = DateTime.Now;
                LoggingService.Current.Log(nameof(RatBoard), $"Rat {ratName} was removed from {CurrentRescues[caseID].ClientName}'s case.", LogLevel.Info);
                StatusService.Current.Notify($"{ratName} was removed from {CurrentRescues[caseID].ClientName}'s case.");
            }
        }

        public void RemoveRat(string clientName, string ratName)
        {
            int? caseID = CurrentRescues.Where(x => clientName.EqualsIgnoreCase(x.Value.ClientName, x.Value.ClientNick)).First().Key;
            if (caseID.HasValue)
                RemoveRat(caseID.Value, ratName);
            return;
        }
        #endregion


    }
}
