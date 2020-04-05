using System;
using System.Threading.Tasks;
using WebServer.HTTP;

namespace MainApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpServer server = new HttpServer(10220);
            await server.StartAsync();
        }
    }
}
