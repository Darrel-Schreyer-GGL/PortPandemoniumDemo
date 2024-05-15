using System.Net.Sockets;
using System;

const string url = "http://localhost:5001";
const int numberOfRequests = 100; // Number of concurrent requests


static async Task<bool> MakeRequestWithNewHttpClient(int id)
{
    using var client = new HttpClient();
    
    try
    {
        var response = await client.GetAsync(url);
        return true;
    }
    catch (SocketException se)
    {
        if (se.SocketErrorCode != SocketError.AddressAlreadyInUse)
        {
            Console.WriteLine($"Request {id} failed: SocketException {se.SocketErrorCode}");
        }
    }
    catch (HttpRequestException hre)
    {
        if (hre.InnerException is SocketException se && se.SocketErrorCode != SocketError.AddressAlreadyInUse)
        {
            Console.WriteLine($"Request {id} failed: Inner SocketException {se.Message}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"""
                Request {id} failed: {ex.Message}
                    {ex.InnerException?.Message}
                """);
    }

    return false;
}
