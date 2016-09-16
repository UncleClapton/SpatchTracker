using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace SpatchTracker.Models
{
    public class Rat : NotificationObject
    {
        public string UUID { get; set; } = "N/A";
        public string CmdrName { get; set; } = "N/A";
        public Platform CmdrPlatform { get; set; } = Platform.PC;

        public Rat () { }

        public Rat (string uuid, string cmdrName, Platform cmdrPlatform)
        {
            UUID = uuid;
            CmdrName = cmdrName;
            CmdrPlatform = cmdrPlatform;
        }

        
    }

    public enum Platform { PC = 0, XB = 1, Unknown = 2 }
}
