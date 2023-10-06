using Microsoft.Extensions.Configuration;

namespace AzureDDNSClient.Options;

internal class AzureOptions
{
    public static string Key => "AZURE_OPTIONS";

    [ConfigurationKeyName("TENANT_ID")]
    public string TenantId { get; set; } = string.Empty;
    [ConfigurationKeyName("CLIENT_ID")]
    public string ClientId { get; set; } = string.Empty;
    [ConfigurationKeyName("CLIENT_SECRET")]
    public string ClientSecret { get; set; } = string.Empty;
    [ConfigurationKeyName("SUBSCRIPTION_ID")]
    public string SubscriptionId { get; set; } = string.Empty;
    [ConfigurationKeyName("RESOURCE_GROUP_NAME")]
    public string ResourceGroupName { get; set; } = string.Empty;
    [ConfigurationKeyName("DNS_ZONE_NAME")]
    public string DnsZoneName { get; set; } = string.Empty;
}