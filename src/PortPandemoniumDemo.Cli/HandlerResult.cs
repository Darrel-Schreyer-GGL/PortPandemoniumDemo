namespace PortPandemoniumDemo.Cli;

internal sealed record HandlerResult(
    int Id,
    bool Success,
    string? Message = null);
