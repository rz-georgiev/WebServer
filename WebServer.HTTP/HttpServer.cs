using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.HTTP
{
    public class HttpServer : IHttpServer
    {
        private TcpListener _listener;
        private readonly int _port;
        private readonly IList<Route> _routeTable;

        public HttpServer(int port, IList<Route> routeTable)
        {
            _port = port;
            _routeTable = routeTable;
        }

        public async Task StartAsync()
        {
            NetworkStream stream = null;
            try
            {
                _listener = new TcpListener(IPAddress.Loopback, _port);
                _listener.Start();

                while (true)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    stream = client.GetStream();

                    byte[] buffer = new byte[4096];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    string requestString = Encoding.UTF8.GetString(buffer);
                    HttpRequest request = new HttpRequest(requestString);
                    Console.WriteLine(request.ToString());

                    HttpResponse response;
                    var route = _routeTable.SingleOrDefault(x => x.Type == request.MethodType && x.Path == request.Path);
                    if (route == null)
                    {
                        response = new HttpResponse(HttpResponseCode.NotFound, new byte[0]);
                    }
                    else
                    {
                        response = route.Action(request);
                    }

                    response.Headers.Add(new HttpHeader { Name = "Server:", Value = "Kazan/1.0" });
                    response.ResponseCookies.Add
                    (
                        new HttpResponseCookie("sid", Guid.NewGuid().ToString()) { HttpOnly = false, MaxAge = 3600, Secure = true }
                    );

                    var responseBytes = Encoding.UTF8.GetBytes(response.ToString());


                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    await stream.WriteAsync(response.Body, 0, response.Body.Length);
                    await stream.FlushAsync();
                    stream.Dispose();
                    stream = null;
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponse(HttpResponseCode.InternalServerError, Encoding.UTF8.GetBytes(ex.ToString()));
                errorResponse.Headers.Add(new HttpHeader { Name = "Content-type", Value = "text/plain" });

                var errorResponseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());

                await stream.WriteAsync(errorResponseBytes, 0, errorResponseBytes.Length);
                await stream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);
                await stream.FlushAsync();
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