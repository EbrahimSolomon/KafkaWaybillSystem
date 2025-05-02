using Confluent.Kafka;
using MessageConsumer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessageConsumer.Services;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly IMessageProcessorService _processor;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(IConfiguration config, IMessageProcessorService processor, ILogger<KafkaConsumerService> logger)
    {
        _config = config;
        _processor = processor;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var kafkaConfig = new ConsumerConfig
        {
            BootstrapServers = _config["Kafka:BootstrapServers"],
            GroupId = _config["Kafka:GroupId"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(kafkaConfig).Build();
        consumer.Subscribe(_config["Kafka:Topic"]);

        _logger.LogInformation("Kafka consumer started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = consumer.Consume(stoppingToken);
                if (result != null)
                {
                    _logger.LogInformation($"Consumed message: {result.Message.Value}");

                    await _processor.ProcessMessageAsync(result.Message.Value);
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
            _logger.LogInformation("Kafka consumer shutting down...");
        }
    }
}
