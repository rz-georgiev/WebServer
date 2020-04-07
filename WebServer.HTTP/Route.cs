using System;

namespace WebServer.HTTP
{
    public class Route
    {
        public Route(string path, HttpMethodType type, Func<HttpRequest, HttpResponse> action)
        {
            Path = path;
            Type = type;
            Action = action;
        }
        public string Path { get; set; }

        public HttpMethodType Type { get; set; }

        public Func<HttpRequest, HttpResponse> Action { get; set; }
    }
}