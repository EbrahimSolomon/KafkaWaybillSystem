namespace FileProcessor.Interfaces;

public interface IMessageQueueService
{
    Task SendMessageAsync(string message);
}
