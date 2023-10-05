using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Dns;
using Azure.ResourceManager.Dns.Models;
using AzureDDNSClient.Options;
using Microsoft.Extensions.Options;
using System.Net;

namespace AzureDDNSClient.Services.Implementation;

internal class AzureDNSService : IAzureDNSService
{
    private readonly AzureOptions _options;
    private readonly ArmClient _armClient;

    public AzureDNSService(ArmClient armClient, IOptions<AzureOptions> options)
    {
        _armClient = armClient;
        _options = options.Value;

    }

    public async Task UpdateIpAddressAsync(IPAddress ipAddress)
    {
        var subscription = (await _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_options.SubscriptionId}")).GetAsync()).Value;

        var resourceGroup = (await subscription.GetResourceGroupAsync(_options.ResourceGroupName)).Value;

        var dnsZone = (await resourceGroup.GetDnsZoneAsync(_options.DnsZoneName)).Value;

        var aRecord = (await dnsZone.GetDnsARecordAsync("@")).Value;

        var aRecordData = new DnsARecordData();

        aRecordData.DnsARecords.Add(new DnsARecordInfo { IPv4Address = ipAddress });

        await aRecord.UpdateAsync(aRecordData);
    }
}