using Clapton.Extensions;
using SpatchTracker.Models;
using SpatchTracker.Services;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;

namespace SpatchTracker.Net
{
    public class ChatReceiver : IDisposable
    {
        public HttpServer ListenerServer { get; private set; }

        public ChatReceiver()
        {
            ListenerServer = new HttpServer(new HttpRequestProvider());

            try
            {
                LoggingService.Current.Log("Starting Chat Event Receiver.", LogType.Info, LogLevel.Verbose);
                ListenerServer.Use(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, 4378)));
            }
            catch (Exception e)
            {
                LoggingService.Current.Log("Unable to start Chat Event Receiver. Possible port conflict!", LogType.Error, LogLevel.Error);
                LoggingService.Current.Log($"Error while initilizing HTTP Server.\n------ Start Stack Trace ------\n{e.Message}\n{e.StackTrace}\n------ End stack trace ------", LogType.Error, LogLevel.Verbose);
                Clapton.Exceptions.ExceptionHandling.ReportException(this, e);
            }

            ListenerServer.Use((context, next) =>
            {
                if (context.Request.Method == HttpMethods.Post)
                {
                    var messageType = GetQueryStringProperty(context.Request, "mt") ?? "none";

                    switch (messageType.ToLower())
                    {
                        case "rsig":
                            context.Response = HttpResponse.CreateWithMessage(HttpResponseCode.Ok, "Rsignal recieived, Updated board.", false, "Do note, I am in fact a teapot.");
                            Task.Run(() => HandleIncomingRatsignal(context));
                            break;
                        case "test":
                            context.Response = HttpResponse.CreateWithMessage(HttpResponseCode.Ok, "Test recieved and noted.", false, "Test request, please ignore");
                            Task.Run(() => HandleIncomingTest(context));
                            break;
                        default:
                            context.Response = HttpResponse.CreateWithMessage(HttpResponseCode.BadRequest, "Bad Message Type (mt). Throwing out request.", false);
                            LoggingService.Current.Log($"ChatReceiver has recieved a message, but it is of invalid message type.", LogType.Info, LogLevel.Info);
                            break;
                    }
                    return next();
                }
                else
                {
                    LoggingService.Current.Log($"ChatReceiver has recieved a message, but it is of invalid request method.", LogType.Info, LogLevel.Info);
                    context.Response = HttpResponse.CreateWithMessage(HttpResponseCode.BadRequest, "Bad method. This server only accepts POSTs", false);
                    return next();
                }
            });

            ListenerServer.Start();
        }

        private static string GetQueryStringProperty(IHttpRequest request, string queryStringName)
        {
            var propertyValue = "";
            request.QueryString.TryGetByName(queryStringName, out propertyValue);
            return WebUtility.UrlDecode(propertyValue);
        }

        private void HandleIncomingRatsignal(IHttpContext context)
        {
            //EX: RATSIGNAL - CMDR A Client - System: SystemName - Platform: PC - O2: OK - Language: English (en-US) - IRC Nickname: A_Client (Case #1)
            var message = GetQueryStringProperty(context.Request, "msg");

            string cmdr = Regex.Match(message, @"CMDR (.*?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "Unknown";
            string system = Regex.Match(message, @"System: (.*?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "Unknown";
            Platform platform = (Regex.Match(message, @"Platform: (XB|PC) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "PC") == "XB" ? Platform.XB : Platform.PC;
            bool codeRed = (Regex.Match(message, @"O2: ((?:NOT )?OK) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "OK") == "NOT OK" ? true : false;
            string language = Regex.Match(message, @"Language: (.*?) -", RegexOptions.IgnoreCase).Groups[1].Value ?? "Unknown";
            string ircNick = Regex.Match(message, @"IRC Nickname: (.*?) \(", RegexOptions.IgnoreCase).Groups[1].Value ?? cmdr ?? "Unknown";
            int? boardIndex = (Regex.Match(message, @"\(Case #(.*)\)", RegexOptions.IgnoreCase).Groups[1].Value ?? "0").ToNullableInt() ?? 0;
            LoggingService.Current.Log($"Incoming Client: CMDR {cmdr} | System: {system} | Platform : {platform.ToString()} | CR: {codeRed.ToString()} | Lang: {language} | IRC: {ircNick} | Case #{boardIndex}", LogType.Incoming, LogLevel.Debug);
        }

        private void HandleIncomingTest(IHttpContext context)
        {
            LoggingService.Current.Log($"Test Signal recieved with message:{GetQueryStringProperty(context.Request,"msg")}", LogType.Incoming, LogLevel.Debug);
        }

        public void Dispose()
        {
            ListenerServer.Dispose();
        }
    }
}
