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
        private Dictionary<string, IDictionary<string, string>> _sessions;
        private readonly IList<Route> _routeTable;
        private readonly int _port;

        public HttpServer(int port, IList<Route> routeTable)
        {
            _port = port;
            _routeTable = routeTable;
            _sessions = new Dictionary<string, IDictionary<string, string>>();
        }

        public async Task StartAsync()
        {
            _listener = new TcpListener(IPAddress.Loopback, _port);
            _listener.Start();

            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                using NetworkStream stream = client.GetStream();

                try
                {
                    byte[] buffer = new byte[4096];
                    await stream.ReadAsync(buffer, 0, buffer.Length);

                    string requestString = Encoding.UTF8.GetString(buffer);

                    if (string.IsNullOrWhiteSpace(requestString))
                        continue;

                    HttpRequest request = new HttpRequest(requestString);
                    Console.WriteLine(request.ToString());

                    HttpResponseCookie responseCookie = null;

                    var cookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.COOKIE_NAME);
                    if (cookie == null || !_sessions.ContainsKey(cookie.Value))
                    {
                        var sessionId = Guid.NewGuid().ToString();
                        responseCookie = new HttpResponseCookie(HttpConstants.COOKIE_NAME, sessionId) { HttpOnly = false, MaxAge = 30 * 24 * 3600, Secure = false };
                        var dictionary = new Dictionary<string, string>();

                        request.SessionData = dictionary;
                        _sessions.Add(sessionId, dictionary);
                    }
                    else
                    {
                        request.SessionData = _sessions[cookie.Value];
                    }

                    HttpResponse response;
                    var route = _routeTable.SingleOrDefault(x => x.Type == request.MethodType && x.Path == request.Path);
                    if (route == null)
                        response = new HttpResponse(HttpResponseCode.NotFound, new byte[0]);
                    else
                        response = route.Action(request);

                    if (responseCookie != null)
                        response.ResponseCookies.Add(responseCookie);

                    response.Headers.Add(new HttpHeader { Name = "Server:", Value = "Kazan/1.0" });

                    var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    await stream.WriteAsync(response.Body, 0, response.Body.Length);
                    await stream.FlushAsync();
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