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
            if (CurrentRescues.Find(x => x.BoardID == newRescue.BoardID) == null)
            {
                CurrentRescues.Add(newRescue);
                this.RaisePropertyChanged(nameof(CurrentRescues));
            }
        }
        #endregion

        #region ClearRescue
        public void ClearRescue(Rescue rescue)
        {
            if(CurrentRescues.Contains(rescue))
            {
                CurrentRescues.Remove(rescue);
                this.RaisePropertyChanged(nameof(CurrentRescues));
            }
        }
        public void ClearRescue(int caseID)
        {
            Rescue curRescue = CurrentRescues.Find(x => x.BoardID == caseID);
            if (curRescue != null)
            {
                CurrentRescues.Remove(curRescue);
                this.RaisePropertyChanged(nameof(CurrentRescues));
            }
        }
        public void ClearRescue(string clientName)
        {
            Rescue curRescue = CurrentRescues.Find(x => clientName.EqualsIgnoreCase(x.ClientName, x.ClientNick));
            if (curRescue != null)
            {
                CurrentRescues.Remove(curRescue);
                this.RaisePropertyChanged(nameof(CurrentRescues));
            }
        }
        #endregion

    }
}
