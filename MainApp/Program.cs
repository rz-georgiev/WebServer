using System.Collections.Generic;
using System.Threading.Tasks;
using WebServer.HTTP;
using WebServer.HTTP.Response;

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
                new Route("/users/logout/", HttpMethodType.GET, LogoutPage),
                new Route("/users/login/", HttpMethodType.POST, DoLogin),
                new Route("/contacts", HttpMethodType.GET, Contacts),
                new Route("/favicon.ico", HttpMethodType.GET, FavIcon),
            };

            HttpServer server = new HttpServer(12220, routeTable);
            await server.StartAsync();
        }

        private static HttpResponse Index(HttpRequest request)
        {
            var username = request.SessionData.ContainsKey("Username") ? request.SessionData["Username"] : "Anonymous";
            return new HtmlResponse($"<h1>Hello, {username}</h1>");
        }

        private static HttpResponse LoginPage(HttpRequest request)
        {           
            request.SessionData["Username"] = "John_Travolta";
            request.SessionData["Language"] = "EN-en";

            return new HtmlResponse("<h1>Login 1 form</h1>");
        }

        private static HttpResponse LogoutPage(HttpRequest request)
        {
            request.SessionData.Remove("Username");
            request.SessionData.Remove("Language");

            return new HtmlResponse("<h1>Logout form</h1>");
        }

        private static HttpResponse DoLogin(HttpRequest request)
        {
            return new HtmlResponse("<h1>Login 2 form</h1>");
        }

        private static HttpResponse Contacts(HttpRequest request)
        {
            var response = new HtmlResponse("<h1>Contact form</h1>");
            return response;
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {
            var response = new FileResponse("wwwroot/favicon.ico", "image/x-icon");
            return response;
        }
    }
}