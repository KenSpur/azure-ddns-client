using Azure.Identity;
using Azure.ResourceManager;
using AzureDDNSClient.Options;
using AzureDDNSClient.Services;
using AzureDDNSClient.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

services.AddLogging(builder => builder.AddConsole());

var serviceProvider = services.BuildServiceProvider();

// Get Services
var whatIsMyIpService = serviceProvider.GetRequiredService<IWhatIsMyIpService>();
var azureDnsService = serviceProvider.GetRequiredService<IAzureDNSService>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

// Run
logger.LogInformation("Getting my Ip Address ...");
var ipAddress = await whatIsMyIpService!.GetMyIpAddressAsync();

logger.LogInformation("Validate & Update Currently Configured Ip Address ...");
await azureDnsService.ValidateAndUpdateIpAddressAsync(ipAddress);

logger.LogInformation("Done!");