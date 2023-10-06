using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Dns;
using Azure.ResourceManager.Dns.Models;
using AzureDDNSClient.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace AzureDDNSClient.Services.Implementation;

internal class AzureDNSService : IAzureDNSService
{
    private readonly ILogger _logger;
    private readonly AzureOptions _options;
    private readonly ArmClient _armClient;

    public AzureDNSService(ArmClient armClient, IOptions<AzureOptions> options, ILogger<AzureDNSService> logger)
    {
        _armClient = armClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task ValidateAndUpdateIpAddressAsync(IPAddress ipAddress)
    {
        var resourceId = FormatDnsZoneId(_options.SubscriptionId, _options.ResourceGroupName, _options.DnsZoneName);

        _logger.LogInformation($"Getting Dns Zone `{resourceId}` ...");
        var dnsZone = await _armClient.GetDnsZoneResource(new ResourceIdentifier(resourceId)).GetAsync();

        _logger.LogInformation("Validating `@` A Record ...");
        var records = dnsZone.Value.GetDnsARecords();
        var aRecord = await records.FirstOrDefaultAsync(r => r.Data.Name == "@");

        if (aRecord is null || !aRecord.Data.DnsARecords.Any())
        {
            _logger.LogInformation("No `@` A Record found, creating ...");
            await CreateARecordAsync(records, ipAddress);
            return;
        }

        if (aRecord.Data.DnsARecords.Count > 1)
            _logger.LogWarning($"More than one value found for A Record `@`");

        if (aRecord.Data.DnsARecords.Any(record => record.IPv4Address.Equals(ipAddress)))
        {
            _logger.LogInformation($"`@` A Record already points to {ipAddress}, skipping ...");
            return;
        }

        _logger.LogInformation($"`@` A Record does not point to {ipAddress}, updating ...");
        await UpdateARecordAsync(aRecord, ipAddress);
    }

    private string FormatDnsZoneId(string subscriptionId, string resourceGroupName, string dnsZoneName)
        => $"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Network/dnsZones/{dnsZoneName}";

    private async Task CreateARecordAsync(DnsARecordCollection records, IPAddress ipAddress)
    {
        var data = new DnsARecordData();

        data.TtlInSeconds = 3600;
        _logger.LogInformation($"Setting TTL to {data.TtlInSeconds} seconds");

        data.DnsARecords.Add(new DnsARecordInfo { IPv4Address = ipAddress });
        _logger.LogInformation($"Pointing `@` A Record to {ipAddress}");

        await records.CreateOrUpdateAsync(Azure.WaitUntil.Completed, "@", data);
    }

    private async Task UpdateARecordAsync(DnsARecordResource aRecord, IPAddress ipAddress)
    {
        aRecord.Data.DnsARecords.Clear();
        _logger.LogInformation($"Clearing `@` A Record values");

        aRecord.Data.DnsARecords.Add(new DnsARecordInfo { IPv4Address = ipAddress });
        _logger.LogInformation($"Pointing `@` A Record to {ipAddress}");

        await aRecord.UpdateAsync(aRecord.Data);
    }
}