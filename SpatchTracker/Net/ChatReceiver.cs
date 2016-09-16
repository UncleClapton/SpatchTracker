using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;
using System.Net;
using SpatchTracker.Services;
using System.Text.RegularExpressions;
using SpatchTracker.Models;

namespace SpatchTracker.Net
{
    public class ChatReceiver
    {
        public HttpServer ListenerServer { get; private set; }

        public ChatReceiver()
        {
            using (ListenerServer = new HttpServer(new HttpRequestProvider()))
            {
                try
                {
                    LoggingService.Current.Log("Starting Chat Event Receiver.", LogType.Info, LogLevel.Verbose);
                    ListenerServer.Use(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, Settings.Current.ReceiverPort)));
                }
                catch (Exception e)
                {
                    LoggingService.Current.Log("Unable to start Chat Event Receiver. Possible port conflict!", LogType.Error, LogLevel.Error);
                    LoggingService.Current.Log($"Error while initilizing HTTP Server.\n------ Start Stack Trace ------\n{e.Message}\n{e.StackTrace}\n------ End stack trace ------", LogType.Error, LogLevel.Verbose);
                    Clapton.Exceptions.ExceptionHandling.ReportException(this, e);
                }

                ListenerServer.Use((context, next) =>
                {
                    context.Response = HttpResponse.CreateWithMessage(HttpResponseCode.Ok, "Received", false);

                    var messageType = GetQueryStringProperty(context.Request, "mt");

                    switch (messageType)
                    {
                        case "RSIG":
                            HandleIncomingRsignal(context);
                            break;
                        default:
                            LoggingService.Current.Log($"ChatReceiver has recieved a message, is of invalid type. MT = {messageType}", LogType.Info, LogLevel.Info);
                            break;
                    }


                    return Task.Factory.GetCompleted();
                });
            }
        }

        private static string GetQueryStringProperty(IHttpRequest request, string queryStringName)
        {
            var propertyValue = "";
            request.QueryString.TryGetByName(queryStringName, out propertyValue);
            return WebUtility.UrlDecode(propertyValue);
        }

        private void HandleIncomingRsignal(IHttpContext context)
        {
            //EX: RATSIGNAL - CMDR A Client - System: SystemName - Platform: PC - O2: OK - Language: English (en-US) - IRC Nickname: A_Client (Case #1)
            var message = GetQueryStringProperty(context.Request, "msg");

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
