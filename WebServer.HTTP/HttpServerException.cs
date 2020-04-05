using System;

namespace WebServer.HTTP
{
    public class HttpServerException : Exception
    {
        public HttpServerException(string message)
            : base(message)
        {
        }
    }
}