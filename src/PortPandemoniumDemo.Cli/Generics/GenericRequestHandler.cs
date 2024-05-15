namespace PortPandemoniumDemo.Cli.Generics;

internal sealed class GenericRequestHandler(
    string url)
    : IRequestHandler
{
    private readonly string _url = url;

    public async Task<HandlerResult> MakeRequest(int id)
    {
        var handlerA1 = new GenericRequestHandler<TypeA>(_url);
        var handlerA2 = new GenericRequestHandler<TypeA>(_url);
        var handlerA3 = new GenericRequestHandler<TypeA>(_url);
        _ = await handlerA1.MakeRequest(id);
        _ = await handlerA2.MakeRequest(id);
        _ = await handlerA3.MakeRequest(id);

        var handlerB1 = new GenericRequestHandler<TypeB>(_url);
        _ = await handlerB1.MakeRequest(id);

        return new HandlerResult(id, true);
    }
}

internal sealed record TypeA
{
}

internal sealed record TypeB
{
}

internal sealed class GenericRequestHandler<T>(
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
