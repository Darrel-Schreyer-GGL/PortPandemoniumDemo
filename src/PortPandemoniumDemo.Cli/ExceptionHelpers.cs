using System.Net.Sockets;

namespace PortPandemoniumDemo.Cli;

internal static class ExceptionHelpers
{
    public static HandlerResult HandleException(this Exception e, int id)
    {
        string? message;
        if(e is SocketException se)
        {
            message = $@"""
                Request {id} failed: SocketException {se.SocketErrorCode}
                """;
        }
        else if (e is HttpRequestException hre && hre.InnerException is SocketException ise)
        {
            message = $@"""
                Request {id} failed: Inner SocketException {ise.Message}
                """;
        }
        else
        {
            message = $@"""
                Request {id} failed: {e.Message}
                    {e.InnerException?.Message}
                """;
        }

        return new HandlerResult(id, false, message);
    }
}
