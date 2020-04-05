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

                string responseContent = null;
            
                if (request.Path == "/")
                    responseContent = "<h1>home page</h1>";
                else if(request.Path == "/users/login")
                    responseContent = "<h1>login page</h1>";

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    byte[] content = Encoding.UTF8.GetBytes(responseContent);

                    var response = new HttpResponse(HttpResponseCode.OK, content);
                    response.Headers.Add(new HttpHeader { Name = "Server", Value = "Kazan/1.0" });
                    response.Headers.Add(new HttpHeader { Name = "Content-Type", Value = "text/html" });

                    byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    await stream.WriteAsync(response.Body, 0, response.Body.Length);
                }  
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