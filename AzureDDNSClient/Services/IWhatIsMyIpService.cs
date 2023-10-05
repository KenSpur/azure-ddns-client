using System.Net;

namespace AzureDDNSClient.Services;

internal interface IWhatIsMyIpService
{
    public Task<IPAddress> GetMyIpAsync();
}