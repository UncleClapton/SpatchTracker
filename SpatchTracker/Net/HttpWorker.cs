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
    public class HttpWorker : IDisposable
    {
        public HttpServer ListenerServer { get; private set; }
        private List<MethodInfo> MessageTypesInfo { get; set; }

        //TODO properly implement POST requests for production.
        /// <summary>
        /// Creates a new <see cref="HttpWorker"/> object.
        /// </summary>
        /// <param name="listeningPort">Port to listen for requests on</param>
        /// <param name="RequestHandlers">Type to scan for RequestHandlers</param>
        public HttpWorker(int listeningPort, Type RequestHandlers)
        {
            ListenerServer = new HttpServer(new HttpRequestProvider());

            MessageTypesInfo = RequestHandlers.GetMethods().Where(x => x.GetCustomAttributes(false).OfType<RequestHandler>().Count() > 0).ToList();

            try
            {
                LoggingService.Current.Log(nameof(HttpWorker), "Starting Chat Event Receiver.", LogLevel.Verbose);
                ListenerServer.Use(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, listeningPort)));
            }
            catch (Exception e)
            {
                LoggingService.Current.Log(nameof(HttpWorker), "Unable to start Chat Event Receiver. Possible port conflict! Check error logs.", LogLevel.Error);
                Clapton.Exceptions.ExceptionHandling.ReportException(this, e);
            }

            //TODO Make this a bit more generic for the sake of expandability. See Issue#2 for the main problem with doing so.
            ListenerServer.Use((context, next) =>
            {
                if (context.Request.Method == HttpMethods.Post)
                {
                    var messageType = context.Request.GetQueryStringProperty("mt") ?? "none";
                    
                    Task.Run(() =>
                    {
                        try
                        {
                            MethodInfo method = MessageTypesInfo.Where(x => x.GetCustomAttribute<RequestHandler>(false).messageCode.ToLower() == messageType.ToLower()).First();
                            method.Invoke(null, new object[] { context.Request.GetQueryStringProperty("msg") });
                        }
                        catch (InvalidOperationException)
                        {
                            LoggingService.Current.Log(nameof(HttpWorker), $"Recieved a message, but it is of invalid message type. mt={messageType}",  LogLevel.Error);
                        }
                    });

                    context.Response = HttpResponse.CreateWithMessage(HttpResponseCode.Ok, "Message recieved.", false);
                    return next();
                }
                else
                {
                    LoggingService.Current.Log(nameof(HttpWorker), "Recieved a message, but it is of invalid request method.",  LogLevel.Info);
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

    public class RequestHandler : Attribute
    {
        public string messageCode { get; private set; }

        public RequestHandler(string mt)
        {
            messageCode = mt;
        }
    }


}
