namespace AzureDDNSClient.Options;

internal class AzureOptions
{
    public static string Key => nameof(AzureOptions);

    public string SubscriptionId { get; set; } = string.Empty;
    public string ResourceGroupName { get; set; } = string.Empty;
    public string DnsZoneName { get; set; } = string.Empty;
}