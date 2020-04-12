using System.IO;
using System.Text;

namespace WebServer.HTTP.Response
{
    public class FileResponse : HttpResponse
    {
        public FileResponse(string path, string type)
            : base()
        {
            ResponseCode = HttpResponseCode.OK;

            var bytes = File.ReadAllBytes(path);
            Body = bytes;

            Headers.Add(new HttpHeader { Name = "Content-Type:", Value = type });
            Headers.Add(new HttpHeader { Name = "Content-Length:", Value = Body.Length.ToString() });
        }
    }
}