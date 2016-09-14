using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatchTracker.Models.Data
{
    public class Rat
    {
        public string UUID { get; set; }
        public string CmdrName { get; set; }
        public List<DataItem> Data { get; set; }
        public DateTime Joined { get; set; }
        public Platform Platform { get; set; }
        public DateTime CreatedAt { get; set; }

        public Rat ()
        {

        }

        public Rat (string newUUID, string newCmdrName, List<DataItem> data, DateTime joined, Platform platform, DateTime createdAt)
        {

        }


    }
}
