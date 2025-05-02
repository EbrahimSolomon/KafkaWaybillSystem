namespace MessageConsumer.Interfaces;

public interface IMessageProcessorService
{
    Task ProcessMessageAsync(string message);
}

