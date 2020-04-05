using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.HTTP
{
    public class HttpServer : IHttpServer
    {
        private TcpListener _listener;
        private int _port;

        public HttpServer(int port)
        {
            _port = port;
        }

        public async Task StartAsync()
        {
            _listener = new TcpListener(IPAddress.Loopback, _port);
            _listener.Start();

            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                using NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1_000_000];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                string requestString = Encoding.UTF8.GetString(buffer);
                HttpRequest request = new HttpRequest(requestString);

                Console.WriteLine(request.ToString());

                //string responseText = "<h1>Kur</h1>";
                //string response = $"HTTP/1.1 200 OK{HttpConstants.NEW_LINE}" +
                //                  $"Server: NaMaikaTiPutkata/2.0{HttpConstants.NEW_LINE}" +
                //                  $"Content-Type: text/html{HttpConstants.NEW_LINE}" +
                //                  $"Content-Length: {responseText.Length}{HttpConstants.NEW_LINE}" +
                //                  $"Set-Cookie: Username=BatViRado; Max-Age={3600}; {HttpConstants.NEW_LINE}" +
                //                  $"Set-Cookie: SSID=BatViRado; Max-Age={3600}; {HttpConstants.NEW_LINE}{HttpConstants.NEW_LINE}" +
                //                  //$"Location: https://www.google.com/ {NEW_LINE}{NEW_LINE}" +
                //                  //$"Content-Disposition: attachment; filename=kur.html{NEW_LINE}{NEW_LINE}" +
                //                  $"{responseText}";

                //byte[] reponseBytes = Encoding.UTF8.GetBytes(response);
                //stream.Write(reponseBytes, 0, reponseBytes.Length);
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public async Task ResetAsync()
        {
            this.Stop();
            await this.StartAsync();
        }
    }
}