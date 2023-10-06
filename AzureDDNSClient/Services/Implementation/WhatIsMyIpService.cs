using AzureDDNSClient.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace AzureDDNSClient.Services.Implementation;

internal class WhatIsMyIpService : IWhatIsMyIpService
{
    private readonly ILogger _logger;
    private readonly HttpClient _client;

    public WhatIsMyIpService(HttpClient client, ILogger<WhatIsMyIpService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IPAddress> GetMyIpAddressAsync()
    {
        var response = await _client.GetAsync(string.Empty);

        if (response.StatusCode != HttpStatusCode.OK) throw new InvalidOperationException(response.StatusCode.ToString());

        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"Response: {content}");

        var myIp = JsonSerializer.Deserialize<MyIPResponse>(content) ?? new();

        if(!IPAddress.TryParse(myIp.IpAddress, out var ipAddress)) throw new InvalidOperationException("Failed to parse Ip Address!");

        return ipAddress;
    }
}