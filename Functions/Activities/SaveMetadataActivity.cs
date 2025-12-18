using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureFunctions.Models;
using AzureFunctions_Triggers.Services;

public class SaveMetadataActivity
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger _logger;

    public SaveMetadataActivity(IEmployeeService employeeService, ILoggerFactory loggerFactory)
    {
        _employeeService = employeeService;
        _logger = loggerFactory.CreateLogger<SaveMetadataActivity>();
    }

    [Function("SaveMetadataActivity")]
    public async Task Run([ActivityTrigger] DocumentMetadata metadata)
    {
        await _employeeService.SaveOrUpdateDocumentAsync(metadata);
        _logger.LogInformation("Document metadata upserted. Id: {Id}, Status: {Status}", metadata.id, metadata.Status);
    }

}
