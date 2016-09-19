using System.Net;
using uhttpsharp;

namespace SpatchTracker.Extensions
{
    public static class IHttpRequestExtensions
    {
        public static string GetQueryStringProperty(this IHttpRequest request, string queryStringName)
        {
            var propertyValue = "";
            request.QueryString.TryGetByName(queryStringName, out propertyValue);
            return WebUtility.UrlDecode(propertyValue);
        }
    }
}
