using System.Net.Sockets;

namespace PortPandemoniumDemo.Cli.EvilClient;

internal sealed class EvilRequestHandler(
    string url)
    : IRequestHandler
{
    private readonly string _url = url;

    public async Task<HandlerResult> MakeRequest(int id)
    {
        using var httpClient = new HttpClient();

        try
        {
            _ = await httpClient.GetAsync(_url);
        }
        catch (Exception e)
        {
            return e.HandleException(id);
        }

        return new HandlerResult(id, true);
    }
}
