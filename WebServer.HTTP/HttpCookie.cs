using System;

namespace WebServer.HTTP
{
    public class HttpCookie
    {
        public HttpCookie(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

    }
}