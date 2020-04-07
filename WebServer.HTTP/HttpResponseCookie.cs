using System;
using System.Text;

namespace WebServer.HTTP
{
    public class HttpResponseCookie : HttpCookie
    {
        public HttpResponseCookie(string name, string value)
            : base(name, value)
        {
            Path = "/";
            SameSite = SameSite.None;
            Expires = DateTime.UtcNow.AddDays(30);
        }

        public string Domain { get; set; }

        public string Path { get; set; }

        public DateTime? Expires { get; set; }

        public long? MaxAge { get; set; }

        public bool Secure { get; set; }

        public bool HttpOnly { get; set; }

        public SameSite SameSite { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"{Name}={Value}");

            if (Expires.HasValue)
                builder.Append($"; Expires={Expires.Value:R}");
           
            else if (MaxAge.HasValue)
                builder.Append($"; MaxAge={MaxAge.Value}");

            if (!string.IsNullOrWhiteSpace(Domain))
                builder.Append($"; Domain={Domain}");

            if (!string.IsNullOrWhiteSpace(Path))
                builder.Append($"; Path={Path}");

            if (Secure)
                builder.Append($"; Secure");

            if (HttpOnly)
                builder.Append("$; HttpOnly");

            builder.Append($"; SameSite={SameSite}");

            return builder.ToString();
        }
    }
}