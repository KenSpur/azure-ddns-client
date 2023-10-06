using Microsoft.Extensions.Configuration;

namespace AzureDDNSClient.Options;

internal class WhatIsMyIpServerOptions
{
    public static string Key => "WHAT_IS_MY_IP_SERVER_OPTIONS";

    [ConfigurationKeyName("BASE_ADDRESS")]
    public string BaseAddress { get; set; } = string.Empty;
}