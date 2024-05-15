namespace PortPandemoniumDemo.Cli;

internal interface IRequestHandler
{
    Task<HandlerResult> MakeRequest(int id);
}
