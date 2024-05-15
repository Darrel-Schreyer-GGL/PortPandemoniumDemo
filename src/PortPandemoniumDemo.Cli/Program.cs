using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortPandemoniumDemo.Cli;
using PortPandemoniumDemo.Cli.BadClient;
using PortPandemoniumDemo.Cli.BestClient;
using PortPandemoniumDemo.Cli.EvilClient;
using PortPandemoniumDemo.Cli.ForceClient;

const string url = "http://localhost:5001";
int numberOfRequests = 10;

var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddHttpClient();
            })
            .Build();

Console.Clear();

var httpClientFactory = host.Services.GetRequiredService<IHttpClientFactory>();

var handlers = new Dictionary<int, IRequestHandler>
{
    {1, new EvilRequestHandler(url) },
    {2, new ForceRequestHandler(url) },
    {3, new BetterRequestHandler(url) },
    {4, new BestRequestHandler(url, httpClientFactory) },
};


while(true)
{
    Console.WriteLine("""
        Choose your poison:
        1. Evil Client
        2. Force Client (simulates a port exhaustion)
        3. Better Client
        4. Best Client

        Type #XXXX to adjust the number of requests. 
        E.g. #25 will set the number of requests to 25.
        Default is 10.

        Type 'exit' to quit
        """);

    var choice = Console.ReadLine()!;

    var isChoice = int.TryParse(choice, out var choiceNumber);

    if(isChoice && handlers.TryGetValue(choiceNumber, out IRequestHandler? handler))
    {
        var handerResults = await Runner(handler.MakeRequest);
        if(handerResults.Where(hr => !hr.Success).ToList().Count > 0)
        {
            Console.WriteLine("Results:");
            foreach (var result in handerResults)
            {
                Console.WriteLine(result);
            }

            Console.WriteLine("Press any key to continue...");
            choice = Console.ReadLine()!;

            return;
        }
    }
    else if (choice.StartsWith('#'))
    {
        _ = int.TryParse(choice[1..], out numberOfRequests);
    }
    else if (choice == "exit")
    {
        break;
    }
    else
    {
        Console.Clear();
        Console.WriteLine("Invalid choice!");
        continue;
    }

    Console.Clear();
}

async Task<HandlerResult[]> Runner(Func<int, Task<HandlerResult>> makeRequest)
{
    var results = new List<HandlerResult>();
    for (int i = 0; i < numberOfRequests; i++)
    {
        var handlerResult = await makeRequest(i);
        results.Add(handlerResult);
    }
    return [.. results];
}