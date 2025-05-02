using FileProcessor.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileProcessor.Services;

public class FileMonitorService
{
    private readonly string _incomingFolder;
    private readonly string _processedFolder;
    private readonly IMessageQueueService _queueService;
    private readonly ILogger<FileMonitorService> _logger;

    public FileMonitorService(IConfiguration config, IMessageQueueService queueService, ILogger<FileMonitorService> logger)
    {
        _incomingFolder = config["AppSettings:IncomingFolder"]!;
        _processedFolder = config["AppSettings:ProcessedFolder"]!;
        _queueService = queueService;
        _logger = logger;
    }

    /*   public async Task MonitorAndProcessAsync()
       {
           if (!Directory.Exists(_incomingFolder)) Directory.CreateDirectory(_incomingFolder);
           if (!Directory.Exists(_processedFolder)) Directory.CreateDirectory(_processedFolder);

           var files = Directory.GetFiles(_incomingFolder);
           foreach (var file in files)
           {
               var content = await File.ReadAllTextAsync(file);
               await _queueService.SendMessageAsync(content);
               var dest = Path.Combine(_processedFolder, Path.GetFileName(file));
               File.Move(file, dest, true);
               _logger.LogInformation($"Processed and moved file: {file}");
           }
       }*/

    /* public async Task MonitorAndProcessAsync()
     {
         if (!Directory.Exists(_incomingFolder)) Directory.CreateDirectory(_incomingFolder);
         if (!Directory.Exists(_processedFolder)) Directory.CreateDirectory(_processedFolder);

         var files = Directory.GetFiles(_incomingFolder);
         _logger.LogInformation($"Found {files.Length} file(s) in {_incomingFolder}");

         foreach (var file in files)
         {
             _logger.LogInformation($"Processing file: {file}");
             var content = await File.ReadAllTextAsync(file);
             await _queueService.SendMessageAsync(content);
             var dest = Path.Combine(_processedFolder, Path.GetFileName(file));
             File.Move(file, dest, true);
             _logger.LogInformation($"Processed and moved file: {file} to {dest}");
         }
     }*/

    public async Task MonitorAndProcessAsync()
    {
        if (!Directory.Exists(_incomingFolder)) Directory.CreateDirectory(_incomingFolder);
        if (!Directory.Exists(_processedFolder)) Directory.CreateDirectory(_processedFolder);

        _logger.LogInformation("Starting continuous file monitoring...");

        while (true)
        {
            var files = Directory.GetFiles(_incomingFolder);

            foreach (var file in files)
            {
                try
                {
                    _logger.LogInformation($"Processing file: {file}");
                    var content = await File.ReadAllTextAsync(file);
                    await _queueService.SendMessageAsync(content);

                    var dest = Path.Combine(_processedFolder, Path.GetFileName(file));
                    File.Move(file, dest, true);
                    _logger.LogInformation($"Moved file to: {dest}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing file: {file}");
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
