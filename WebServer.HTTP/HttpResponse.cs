using System.Collections.Generic;
using System.Text;

namespace WebServer.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(HttpResponseCode responseCode, byte[] body)
        {
            Version = HttpVersion.HTTP11;
            ResponseCode = responseCode;
            Body = body;

            Headers = new List<HttpHeader>();

            if (body?.Length > 0)
                Headers.Add(new HttpHeader { Name = "Content-Length", Value = body.Length.ToString() });
        }

        public HttpVersion Version { get; set; }

        public HttpResponseCode ResponseCode { get; set; }

        public IList<HttpHeader> Headers { get; set; }

        public byte[] Body { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var version = Version switch
            {
                HttpVersion.HTTP10 => "HTTP/1.0",
                HttpVersion.HTTP11 => "HTTP/1.1",
                HttpVersion.HTTP20 => "HTTP/2.0",
                _ => "UNDEFINED"
            };

            builder.Append($"{version} {(int)ResponseCode} {HttpConstants.NEW_LINE}");
            foreach (var header in Headers)
            {
                builder.Append($"{header}{HttpConstants.NEW_LINE}");
            }

            return builder.ToString();
        }
    }
}