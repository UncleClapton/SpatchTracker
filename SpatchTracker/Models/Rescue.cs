using System;
using System.Collections.Generic;

namespace SpatchTracker.Models
{
    public class Rescue
    {
        public string UUID { get; set; } = "UNKNOWN";
        public string ClientNick { get; set; } = "";
        public bool Active { get; set; } = true;
        public string ClientName { get; set; } = "";
        public bool CodeRed { get; set; } = false;
        public Platform Platform { get; set; } = Platform.PC;
        public List<string> Quotes { get; set; } = new List<string>();
        public string System { get; set; } = "";
        public string Title { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<RescueRat> AssignedRats { get; set; } = new List<RescueRat>();
        public bool Epic { get; set; } = false;

        public Rescue () {}
    }
}
