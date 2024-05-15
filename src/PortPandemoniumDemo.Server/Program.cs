var builder = WebApplication.CreateBuilder(args);

if (args.Length > 0)
{
    var port = args[0]
        .Split('=')[1];

    builder.WebHost.UseUrls([$"http://localhost:{port}"]);
}

var app = builder.Build();

app.MapGet("/", async context =>
{
    var port = context.Request.Host.Port;
    await context.Response.WriteAsync($"Hello from port {port}");
});

app.Run();