using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureFunctions.Models;
using AzureFunctionPet.Services;

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

        await _employeeService.HandleAddDocumentAsync(metadata);
        _logger.LogInformation("Document metadata saved successfully");
    }
}
