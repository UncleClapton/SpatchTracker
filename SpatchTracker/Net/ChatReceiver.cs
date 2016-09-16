using SpatchTracker.Models;
using SpatchTracker.Services;
using System;
using System.Text.RegularExpressions;

namespace SpatchTracker.Net
{
    public class ChatReceiver
    {
        public ChatReceiver()
        {
            try
            {
                LoggingService.Current.Log("Starting Chat Event Receiver.", LogType.Info, LogLevel.Verbose);
            }
            catch (Exception e)
            {
                LoggingService.Current.Log("Unable to start Chat Event Receiver. Possible port conflict!", LogType.Error, LogLevel.Error);
                LoggingService.Current.Log($"Error while initilizing WS Listener.\n------ Start Stack Trace ------\n{e.Message}\n{e.StackTrace}\n------ End stack trace ------", LogType.Error, LogLevel.Verbose);
                Clapton.Exceptions.ExceptionHandling.ReportException(this, e);
            }
        }

        private void HandleIncomingRsignal()
        {
            //EX: RATSIGNAL - CMDR A Client - System: SystemName - Platform: PC - O2: OK - Language: English (en-US) - IRC Nickname: A_Client (Case #1)
            var message = "";

            string cmdr = Regex.Match(message, @"CMDR (.*?) -", RegexOptions.IgnoreCase).Value;
            string system = Regex.Match(message, @"System: (.*?) -", RegexOptions.IgnoreCase).Value;
            Platform platform = Regex.Match(message, @"Platform: (XB|PC) -", RegexOptions.IgnoreCase).Value == "XB" ? Platform.XB : Platform.PC;
            bool codeRed = Regex.Match(message, @"O2: ((?:NOT )?OK) -", RegexOptions.IgnoreCase).Value == "NOT OK" ? true : false;
            string language = Regex.Match(message, @"Language: (.*?) -", RegexOptions.IgnoreCase).Value;
            string ircNick = Regex.Match(message, @"IRC Nickname: (.*?) \(", RegexOptions.IgnoreCase).Value ?? cmdr;
            int boardIndex = int.Parse(Regex.Match(message, @"\(Case #(.*)\)", RegexOptions.IgnoreCase).Value);

            LoggingService.Current.Log($"Incoming Client:\nCMDR: {cmdr}\nSystem: {system}\nPlatform : {platform.ToString()}\nCR: {codeRed.ToString()}\nLang: {language}\nIRC: {ircNick}\nCase #{boardIndex}", LogType.Incoming, LogLevel.Debug);
        }


    }
}
