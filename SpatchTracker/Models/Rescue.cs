using System;
using System.Collections.Generic;

namespace SpatchTracker.Models
{
    public class Rescue
    {
        public bool Active { get; set; } = true;
        public bool CodeRed { get; set; } = false;
        public bool Epic { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int BoardID { get; set; } = 100;
        //              Rat name, Rat info
        public Dictionary<string, RescueRat> AssignedRats { get; set; } = new Dictionary<string, RescueRat>();
        public List<string> Quotes { get; set; } = new List<string>();
        public Platform Platform { get; set; } = Platform.PC;
        public string ClientName { get; set; } = "";
        public string ClientNick { get; set; } = "";
        public string Language { get; set; } = "English (en-US)";
        public string System { get; set; } = "";
        public string Title { get; set; } = "";

        public Rescue () {}
    }
}
