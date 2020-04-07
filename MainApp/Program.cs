using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServer.HTTP;

namespace MainApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var routeTable = new List<Route>
            {
                new Route("/", HttpMethodType.GET, Index),
                new Route("/users/login/", HttpMethodType.GET, LoginPage),
                new Route("/users/login/", HttpMethodType.POST, DoLogin),
                new Route("/contacts", HttpMethodType.GET, Contacts),
                new Route("/favicon.ico", HttpMethodType.GET, FavIcon),
            };

            HttpServer server = new HttpServer(12220, routeTable);
            await server.StartAsync();
        }

        private static HttpResponse Index(HttpRequest request)
        {
            var content = "<h1>Home page<h1>";
            var bytes = Encoding.UTF8.GetBytes(content);

            var response = new HttpResponse(HttpResponseCode.OK, bytes);

            response.Headers.Add(new HttpHeader { Name = "Content-Type:", Value = "text/html" });

            return response;
        }

        private static HttpResponse LoginPage(HttpRequest request)
        {
            var content = "<h1>Login page<h1>";
            var bytes = Encoding.UTF8.GetBytes(content);

            var response = new HttpResponse(HttpResponseCode.OK, bytes);

            response.Headers.Add(new HttpHeader { Name = "Content-Type:", Value = "text/html" });

            return response;
        }

        private static HttpResponse DoLogin(HttpRequest request)
        {
            var content = "<h1>Login form<h1>";
            var bytes = Encoding.UTF8.GetBytes(content);

            var response = new HttpResponse(HttpResponseCode.OK, bytes);

            response.Headers.Add(new HttpHeader { Name = "Content-Type:", Value = "text/html" });

            return response;
        }

        private static HttpResponse Contacts(HttpRequest request)
        {
            var content = "<h1>Contact form<h1>";
            var bytes = Encoding.UTF8.GetBytes(content);

            var response = new HttpResponse(HttpResponseCode.OK, bytes);

            response.Headers.Add(new HttpHeader { Name = "Content-Type:", Value = "text/html" });

            return response;
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {
            var content = "<h1>Fav form<h1>";
            var bytes = Encoding.UTF8.GetBytes(content);

            var response = new HttpResponse(HttpResponseCode.OK, bytes);

            response.Headers.Add(new HttpHeader { Name = "Content-Type:", Value = "text/html" });

            return response;
        }
    }
}