using Azure.Identity;
using Azure.ResourceManager;
using AzureDDNSClient.Options;
using AzureDDNSClient.Services;
using AzureDDNSClient.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.NetworkInformation;

// Init Configuration
var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>().Build();

// Configure Services
var services = new ServiceCollection();

services.AddTransient<IConfiguration>(_ => configuration);

services.AddOptions<WhatIsMyIpOptions>().Configure<IConfiguration>(
    (options, config) => config.GetSection(WhatIsMyIpOptions.Key).Bind(options));

services.AddOptions<AzureOptions>().Configure<IConfiguration>(
    (options, config) => config.GetSection(AzureOptions.Key).Bind(options));

services.AddHttpClient<IWhatIsMyIpService, WhatIsMyIpService>(nameof(WhatIsMyIpService), (provider, client) =>
{
    var options = provider.GetRequiredService<IOptions<WhatIsMyIpOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseAddress);
});

services.AddScoped(_ => new ArmClient(new DefaultAzureCredential()));

services.AddTransient<IAzureDNSService, AzureDNSService>();

var serviceProvider = services.BuildServiceProvider();

// Get Services
var whatIsMyIpService = serviceProvider.GetService<IWhatIsMyIpService>();
var azureDnsService = serviceProvider.GetService<IAzureDNSService>();

// Run
Console.WriteLine("Getting my Ip ...");
var ipAddress = await whatIsMyIpService!.GetMyIpAsync();

Console.WriteLine($"My Ip is: {ipAddress}");

Console.WriteLine("Updating Azure Dns ...");
await azureDnsService!.UpdateIpAddressAsync(ipAddress);

Console.WriteLine("Done!");