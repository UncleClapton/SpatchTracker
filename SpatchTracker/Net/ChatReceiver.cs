using SpatchTracker.Extensions;
using SpatchTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;

namespace SpatchTracker.Net
{
    public class ChatReceiver : IDisposable
    {
        public HttpServer ListenerServer { get; private set; }
        private List<MethodInfo> MessageTypesInfo { get; set; }

        public ChatReceiver()
        {
            ListenerServer = new HttpServer(new HttpRequestProvider());

            MessageTypesInfo = typeof(MessageHandlers).GetMethods().Where(x => x.GetCustomAttributes(false).OfType<MessageType>().Count() > 0).ToList();

            try
            {
                LoggingService.Current.Log(nameof(ChatReceiver), "Starting Chat Event Receiver.", LogLevel.Verbose);
                ListenerServer.Use(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, 4378)));
            }
            catch (Exception e)
            {
                LoggingService.Current.Log(nameof(ChatReceiver), "Unable to start Chat Event Receiver. Possible port conflict! Check error logs.", LogLevel.Error);
                Clapton.Exceptions.ExceptionHandling.ReportException(this, e);
            }

            ListenerServer.Use((context, next) =>
            {
                if (context.Request.Method == HttpMethods.Post)
                {
                    var messageType = context.Request.GetQueryStringProperty("mt") ?? "none";
                    
                    Task.Run(() =>
                    {
                        try
                        {
                            MethodInfo method = MessageTypesInfo.Where(x => x.GetCustomAttribute<MessageType>(false).messageCode.ToLower() == messageType.ToLower()).First();
                            method.Invoke(null, new object[] { context.Request.GetQueryStringProperty("msg") });
                        }
                        catch (InvalidOperationException)
                        {
                            LoggingService.Current.Log(nameof(ChatReceiver), $"ChatReceiver has recieved a message, but it is of invalid message type. mt={messageType}",  LogLevel.Error);
                        }
                    });

                    context.Response = HttpResponse.CreateWithMessage(HttpResponseCode.Ok, "Message recieved.", false);
                    return next();
                }
                else
                {
                    LoggingService.Current.Log(nameof(ChatReceiver), "ChatReceiver has recieved a message, but it is of invalid request method.",  LogLevel.Info);
                    context.Response = HttpResponse.CreateWithMessage(HttpResponseCode.BadRequest, "This server only accepts POST requests.", false);
                    return next();
                }
            });

            ListenerServer.Start();
        }

        public void Dispose()
        {
            ListenerServer.Dispose();
        }
    }

    public class MessageType : Attribute
    {
        public string messageCode { get; private set; }

        public MessageType(string mt)
        {
            messageCode = mt;
        }
    }


}
