﻿using Microsoft.Extensions.Configuration;

namespace AzureDDNSClient.Options;

internal class AzureOptions
{
    public static string Key => "AZURE_OPTIONS";

    [ConfigurationKeyName("SUBSCRIPTION_ID")]
    public string SubscriptionId { get; set; } = string.Empty;
    [ConfigurationKeyName("RESOURCE_GROUP_NAME")]
    public string ResourceGroupName { get; set; } = string.Empty;
    [ConfigurationKeyName("DNS_ZONE_NAME")]
    public string DnsZoneName { get; set; } = string.Empty;
}