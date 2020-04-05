using System;

namespace WebServer.HTTP
{
    public class HttpCookie
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Domain { get; set; }

        public string Path { get; set; }

        public DateTime? Expires { get; set; }

        public long Size { get; set; }

        public bool HttpOnly { get; set; }

        public bool Secure { get; set; }

        public SameSite SameSite { get; set; }
    }
}