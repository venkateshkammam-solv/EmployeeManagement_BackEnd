using Microsoft.Extensions.Logging;

public class DataLog
{
    private readonly ILogger<DataLog> _logger;

    public DataLog(ILogger<DataLog> logger)
    {
        _logger = logger;
    }

    public async Task LogInfoAsync(string message)
    {
        _logger.LogInformation($"[INFO] {DateTime.UtcNow:o} - {message}");
        await Task.CompletedTask;
    }

    public async Task LogErrorAsync(string message, Exception ex = null)
    {
        _logger.LogError(ex, $"[ERROR] {DateTime.UtcNow:o} - {message}");
        await Task.CompletedTask;
    }
}
