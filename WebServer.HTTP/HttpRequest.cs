using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebServer.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string request)
        {
            int lastHeaderLine = 0;
            StringBuilder bodyBuilder = new StringBuilder();

            Headers = new List<HttpHeader>();
            Cookies = new List<HttpCookie>();

            var lines = request.Split(new string[] { $"{HttpConstants.NEW_LINE}" }, System.StringSplitOptions.None);

            if (lines.Length < 1)
                throw new HttpServerException("Insufficient header parts");

            var firstLine = lines[0].Split(new string[] { " " }, System.StringSplitOptions.None);
            if (firstLine.Length < 3)
                throw new HttpServerException("Insufficient service part information");

            var methodType = firstLine[0];
            MethodType = methodType switch
            {
                "GET" => HttpMethodType.GET,
                "PUT" => HttpMethodType.PUT,
                "POST" => HttpMethodType.POST,
                "DELETE" => HttpMethodType.DELETE,
                _ => HttpMethodType.UNDEFINED
            };

            var path = firstLine[1];
            Path = path;

            var version = firstLine[2];
            HttpVersion = version switch
            {
                "HTTP/1.0" => HttpVersion.HTTP10,
                "HTTP/1.1" => HttpVersion.HTTP11,
                "HTTP/2.0" => HttpVersion.HTTP20,
                _ => HttpVersion.UNDEFINED,
            };

            for (var index = 1; index < lines.Length; index++)
            {
                var line = lines[index];

                if (string.IsNullOrWhiteSpace(line))
                {
                    lastHeaderLine = index;
                    break;
                }

                var splittedHeader = line.Split(new char[] { ':' }, 2, System.StringSplitOptions.None);
                Headers.Add(new HttpHeader { Name = splittedHeader[0], Value = splittedHeader[1] });

                if (splittedHeader[0] == "Cookie")
                {
                    var splittedCookie = splittedHeader[1].Split(new char[] { '=' }, 2, System.StringSplitOptions.None);
                    Cookies.Add(new HttpCookie(HttpConstants.COOKIE_NAME, splittedCookie[1]));
                }
            }

            for (var index = lastHeaderLine + 1; index < lines.Length; index++)
            {
                var line = lines[index];

                if (!string.IsNullOrWhiteSpace(line))
                    bodyBuilder.Append(lines[index]);
            }

            Body = bodyBuilder.ToString();
        }

        public HttpMethodType MethodType { get; set; }

        public HttpVersion HttpVersion { get; set; }

        public IList<HttpHeader> Headers;

        public IList<HttpCookie> Cookies { get; set; }

        public IDictionary<string, string> SessionData { get; set; }

        public string Path { get; set; }

        public string Body { get; set; }

        public override string ToString()
        {
            return $"{MethodType} {Path}";
        }
    }
}