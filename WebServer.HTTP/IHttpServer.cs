using System.Threading.Tasks;

namespace WebServer.HTTP
{
    public interface IHttpServer
    {
        Task Start(int port);

        Task Stop();

        Task Reset();
    }
}