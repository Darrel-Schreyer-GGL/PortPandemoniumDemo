namespace PortPandemoniumDemo.Cli.BadClient;

internal sealed class BetterRequestHandler(
    string url)
    : IRequestHandler, IDisposable
{
    private static HttpClient _httpClient = new();
    private readonly string _url = url;

    public async Task<HandlerResult> MakeRequest(int id)
    {
        try
        {
            _ = await _httpClient.GetAsync(_url);
        }
        catch (Exception e)
        {
            return e.HandleException(id);
        }

        return new HandlerResult(id, true);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
