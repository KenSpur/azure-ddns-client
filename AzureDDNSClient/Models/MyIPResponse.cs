using System.Text.Json.Serialization;

namespace AzureDDNSClient.Models;

internal class MyIPResponse
{
    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; set; } = string.Empty;
}
