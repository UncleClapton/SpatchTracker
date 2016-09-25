using Clapton.Extensions;
using SpatchTracker.Models;
using SpatchTracker.Services;
using System;
using System.Text.RegularExpressions;
using uhttpsharp;

namespace SpatchTracker.Net
{
    public static class MessageHandlers
    {
        [MessageType("rsig")]
        public static void HandleRatsignal(string message)
        {
            //EX: RATSIGNAL - CMDR A Client - System: SystemName - Platform: PC - O2: OK - Language: English (en-US) - IRC Nickname: A_Client (Case #1)

            Rescue newRescue = new Rescue();

            newRescue.ClientName = Regex.Match(message, @"CMDR (.+?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "Client";
            newRescue.System = Regex.Match(message, @"System: (.+?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "N/A";
            newRescue.Platform = (Regex.Match(message, @"Platform: (XB|PC) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "PC") == "XB" ? Platform.XB : Platform.PC;
            newRescue.CodeRed = (Regex.Match(message, @"O2: ((?:NOT )?OK) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "OK") == "NOT OK" ? true : false;
            newRescue.Language = Regex.Match(message, @"Language: (.+?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "English (en-US)";
            newRescue.ClientNick = Regex.Match(message, @"IRC Nickname: (.+?) \(", RegexOptions.IgnoreCase).Groups[1].Value ?? newRescue.ClientName ?? "A_Client";
            newRescue.BoardID = (Regex.Match(message, @"\(Case #(\d+?)\)", RegexOptions.IgnoreCase).Groups[1].Value ?? "0").ToNullableInt() ?? 0;
            newRescue.CreatedAt = DateTime.Now;
            newRescue.UpdatedAt = DateTime.Now;

            LoggingService.Current.Log(nameof(MessageHandlers), $"Incoming Client: CMDR {newRescue.ClientName} | System: {newRescue.System} | Platform : {newRescue.Platform.ToString()} | CR: {newRescue.CodeRed.ToString()} | Lang: {newRescue.Language} | IRC: {newRescue.ClientNick} | Case #{newRescue.BoardID}", LogLevel.Verbose);
            RatBoard.Current.AddRescue(newRescue);
        }

        [MessageType("assign")]
        public static void HandleAssign(string message)
        {
            if (message.StartsWith("!assign", "!go", "!add"))
            {

            }
        }

        [MessageType("ping")]
        public static void HandlePing(string message)
        {
            LoggingService.Current.Log($"Ping signal recieved with message:{message}", LogType.Incoming, LogLevel.Debug);
        }
    }
}
