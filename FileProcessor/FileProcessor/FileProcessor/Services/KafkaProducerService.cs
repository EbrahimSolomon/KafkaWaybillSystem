using Confluent.Kafka;
using FileProcessor.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileProcessor.Services;

public class KafkaProducerService : IMessageQueueService
{
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducerService(IConfiguration config, ILogger<KafkaProducerService> logger)
    {
        _logger = logger;
        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = config["AppSettings:Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
        _topic = config["AppSettings:Kafka:Topic"]!;
    }

    public async Task SendMessageAsync(string message)
    {
        try
        {
            var result = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
            _logger.LogInformation($"✅ Message delivered to {result.TopicPartitionOffset}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Failed to send message to Kafka: {ex.Message}");
        }
    }

}