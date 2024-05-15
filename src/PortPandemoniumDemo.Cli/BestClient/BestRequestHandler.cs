namespace PortPandemoniumDemo.Cli.BestClient;

internal sealed class BestRequestHandler(
    string url, 
    IHttpClientFactory httpClientFactory)
    : IRequestHandler
{
    private readonly string _url = url;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<HandlerResult> MakeRequest(int id)
    {
        using var httpClient = _httpClientFactory.CreateClient();
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