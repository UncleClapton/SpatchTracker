using Clapton.Extensions;
using SpatchTracker.Models;
using SpatchTracker.Services;
using System.Text.RegularExpressions;
using uhttpsharp;

namespace SpatchTracker.Net
{
    public static class MessageTypes
    {
        [MessageType("rsig")]
        public static void HandleIncomingRatsignal(string message)
        {
            //EX: RATSIGNAL - CMDR A Client - System: SystemName - Platform: PC - O2: OK - Language: English (en-US) - IRC Nickname: A_Client (Case #1)

            string cmdr = Regex.Match(message, @"CMDR (.+?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "Unknown";
            string system = Regex.Match(message, @"System: (.+?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "Unknown";
            Platform platform = (Regex.Match(message, @"Platform: (XB|PC) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "PC") == "XB" ? Platform.XB : Platform.PC;
            bool codeRed = (Regex.Match(message, @"O2: ((?:NOT )?OK) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "OK") == "NOT OK" ? true : false;
            string language = Regex.Match(message, @"Language: (.+?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "Unknown";
            string ircNick = Regex.Match(message, @"IRC Nickname: (.+?) \(", RegexOptions.IgnoreCase).Groups[1].Value ?? cmdr ?? "Unknown";
            int? boardIndex = (Regex.Match(message, @"\(Case #(\d+?)\)", RegexOptions.IgnoreCase).Groups[1].Value ?? "0").ToNullableInt() ?? 0;
            LoggingService.Current.Log($"Incoming Client: CMDR {cmdr} | System: {system} | Platform : {platform.ToString()} | CR: {codeRed.ToString()} | Lang: {language} | IRC: {ircNick} | Case #{boardIndex}", LogType.Incoming, LogLevel.Debug);
        }

        [MessageType("inject")]
        public static void HandleIncomingAssign(string message)
        {

        }

        [MessageType("ping")]
        public static void HandleIncomingTest(string message)
        {
            LoggingService.Current.Log($"Ping signal recieved with message:{message}", LogType.Incoming, LogLevel.Debug);
        }
    }
}
