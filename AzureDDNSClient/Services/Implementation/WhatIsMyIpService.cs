using AzureDDNSClient.Models;
using System.Net;
using System.Text.Json;

namespace AzureDDNSClient.Services.Implementation;

internal class WhatIsMyIpService : IWhatIsMyIpService
{
    private readonly HttpClient _client;

    public WhatIsMyIpService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IPAddress> GetMyIpAsync()
    {
        Console.WriteLine($"Sending Request to {_client.BaseAddress}");
        
        var response = await _client.GetAsync(string.Empty);

        Console.WriteLine(response.ToString());

        if (response.StatusCode != HttpStatusCode.OK) throw new InvalidOperationException(response.StatusCode.ToString());

        var myIp = JsonSerializer.Deserialize<MyIPResponse>(await response.Content.ReadAsStringAsync()) ?? new();

        if(!IPAddress.TryParse(myIp.IpAddress, out var ipAddress)) throw new InvalidOperationException("Failed to parse Ip Address!");

        return ipAddress;
    }
}