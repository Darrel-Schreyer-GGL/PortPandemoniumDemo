using System.Net;
using System.Net.Sockets;

namespace PortPandemoniumDemo.Cli.ForceClient;

internal sealed class ForceRequestHandler(
    string url)
    : IRequestHandler
{
    private readonly string _url = url;

    public async Task<HandlerResult> MakeRequest(int id)
    {
        await Task.Delay(50); // wait a bit...

        try
        {
            var client = CreateHttpClient(50000);
            _ = await client.GetAsync(_url);
        }
        catch (Exception e)
        {
            return e.HandleException(id);
        }

        return new HandlerResult(id, true);
    }


    private static HttpClient CreateHttpClient(int localPort)
    {
        var handler = new SocketsHttpHandler
        {
            ConnectCallback = async (context, token) =>
            {
                var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Loopback, localPort));
                await socket.ConnectAsync(context.DnsEndPoint, token);
                return new NetworkStream(socket, ownsSocket: true);
            }
        };

        return new HttpClient(handler);
    }
}
