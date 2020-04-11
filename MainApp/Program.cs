using System.Collections.Generic;
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

            HttpServer server = new HttpServer(13365, routeTable);
            await server.StartAsync();
        }

        private static HttpResponse Index(HttpRequest request)
        {
            return new HtmlResponse("<h1>Index form</h1>");
        }

        private static HttpResponse LoginPage(HttpRequest request)
        {
            return new HtmlResponse("<h1>Login 1 form</h1>");
        }

        private static HttpResponse DoLogin(HttpRequest request)
        {
            return new HtmlResponse("<h1>Login 2 form</h1>");
        }

        private static HttpResponse Contacts(HttpRequest request)
        {
            return new HtmlResponse("<h1>Contact form</h1>");
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {
            return new HtmlResponse("<h1>Fav form</h1>");
        }
    }
}