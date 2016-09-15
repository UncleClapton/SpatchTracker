using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatchTracker.Models.Data
{
    public class Rescue
    {
        private string uuid;
        private bool active;
        private string clientName;
        private bool codeRed;
        private List<DataItem> data;
        private bool open;
        private string notes;
        private Platform platform;
        private List<string> quotes;
        private bool successful;
        private string system;
        private string title;
        private List<string> unidentifiedRats;
        private DateTime createdAt;
        private DateTime updatedAt;
        private List<Rat> assignedRats;
        private bool epic;
        private string firstLimpet;
    }


}
