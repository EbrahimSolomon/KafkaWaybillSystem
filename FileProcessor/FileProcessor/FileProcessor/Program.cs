using FileProcessor.Interfaces;
using FileProcessor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IMessageQueueService, KafkaProducerService>();
        services.AddSingleton<FileMonitorService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

var service = host.Services.GetRequiredService<FileMonitorService>();
await service.MonitorAndProcessAsync();
