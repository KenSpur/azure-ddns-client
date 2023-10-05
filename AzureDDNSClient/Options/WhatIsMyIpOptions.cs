namespace AzureDDNSClient.Options;

internal class WhatIsMyIpOptions
{
    public static string Key => nameof(WhatIsMyIpOptions);
    
    public string BaseAddress { get; set; } = string.Empty;
}