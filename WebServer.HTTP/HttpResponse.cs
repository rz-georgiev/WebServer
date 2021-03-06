﻿using System.Collections.Generic;
using System.Text;

namespace WebServer.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(HttpResponseCode responseCode, byte[] body)
             : this()
        {
            ResponseCode = responseCode;
            Body = body;

            if (body?.Length > 0)
            {
                Headers.Add(new HttpHeader
                {
                    Name = "Content-Length:",
                    Value = body.Length.ToString()
                });
            }
        }

        internal HttpResponse()
        {
            Version = HttpVersion.HTTP11;

            Headers = new List<HttpHeader>();
            ResponseCookies = new List<HttpResponseCookie>();
        }

        public HttpVersion Version { get; set; }

        public HttpResponseCode ResponseCode { get; set; }

        public IList<HttpHeader> Headers { get; set; }

        public IList<HttpResponseCookie> ResponseCookies { get; set; }

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

            builder.Append($"{version} {(int)ResponseCode} {ResponseCode} {HttpConstants.NEW_LINE}");
            foreach (var header in Headers)
            {
                builder.Append($"{header}{HttpConstants.NEW_LINE}");
            }

            foreach (var cookie in ResponseCookies)
            {
                builder.Append($"Set-Cookie: {cookie}{HttpConstants.NEW_LINE}");
            }

            // Notice: There should be always two empty rows on the bottom, so we can recognize the http body!
            builder.Append(HttpConstants.NEW_LINE);
            builder.Append(HttpConstants.NEW_LINE);

            return builder.ToString();
        }
    }
}