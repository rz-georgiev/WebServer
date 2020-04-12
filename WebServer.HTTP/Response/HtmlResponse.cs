using System.Text;

namespace WebServer.HTTP.Response
{
    public class HtmlResponse : HttpResponse
    {
        public HtmlResponse(string html)
            : base()
        {
            ResponseCode = HttpResponseCode.OK;

            var bytes = Encoding.UTF8.GetBytes(html);
            Body = bytes;

            Headers.Add(new HttpHeader { Name = "Content-Type:", Value = "text/html" });
            Headers.Add(new HttpHeader { Name = "Content-Length:", Value = Body.Length.ToString() });
        }
    }
}