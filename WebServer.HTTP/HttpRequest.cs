using System.Collections.Generic;
using System.Text;

namespace WebServer.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string request)
        {
            if (string.IsNullOrWhiteSpace(request))
                return;

            request = request.Trim();

            var lines = request.Split(new string[] { $"{HttpConstants.NEW_LINE}" }, System.StringSplitOptions.None);

            if (lines.Length < 3)
                throw new HttpServerException("Invalid header part");

            var firstLine = lines[0].Split(new string[] { " " }, System.StringSplitOptions.None);

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
                "HTTP/1.1" => HttpVersion.HTTP11,
                "HTTP/1.2" => HttpVersion.HTTP12,
                "HTTP/2.0" => HttpVersion.HTTP20,
                _ => HttpVersion.UNDEFINED,
            };

            var bodyBuilder = new StringBuilder();
            int lastHeaderLine = 0;
            Headers = new List<HttpHeader>();

            for (var index = 1; index < lines.Length; index++)
            {
                var line = lines[index];

                if (string.IsNullOrWhiteSpace(line))
                {
                    lastHeaderLine = index;
                    break;
                }
                var splitted = line.Split(':');
                Headers.Add(new HttpHeader { Name = splitted[0], Value = splitted[1] });
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

        public string Path { get; set; }

        public HttpVersion HttpVersion { get; set; }

        public IList<HttpHeader> Headers;

        public string Body { get; set; }

        public override string ToString()
        {
            return $"{MethodType} {Path}";
        }
    }
}