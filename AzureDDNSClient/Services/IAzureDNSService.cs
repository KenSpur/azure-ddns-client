using System.Net;

namespace AzureDDNSClient.Services;

internal interface IAzureDNSService
{
    public Task UpdateIpAddressAsync(IPAddress ipAddress);
}