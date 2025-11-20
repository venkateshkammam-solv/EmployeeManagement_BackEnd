using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using AzureFunctionPet.Models;
using AzureFunctionPet.Repositories;
using AzureFunctions_Triggers.Models;
using Shared.Services;

namespace AzureFunctionPet.Functions
{
    public class EmployeeDocumentProcessorFunction
    {
        private readonly ILogger<EmployeeDocumentProcessorFunction> _logger;
        private readonly DataLog _dataLog;
        private readonly EmailService _emailService;
        private readonly ICosmosRepository _cosmosRepo;
        private readonly idGenerator _codeGenerator;

        public EmployeeDocumentProcessorFunction(
            ILogger<EmployeeDocumentProcessorFunction> logger,
            DataLog dataLog,
            EmailService emailService,
            idGenerator codeGenerator,
            ICosmosRepository cosmosRepo)
        {
            _logger = logger;
            _dataLog = dataLog;
            _emailService = emailService;
            _cosmosRepo = cosmosRepo;
            _codeGenerator = codeGenerator;
        }

        [Function("EmployeeDocumentProcessor")]
        public async Task RunAsync(
            [BlobTrigger("employee-documents/{name}", Connection = "AzureWebJobsStorage")] Stream blobStream,
            string name)
        {
            _logger.LogInformation($"Employee document uploaded: {name}");
            await _dataLog.LogInfoAsync($"Processing employee document: {name}");

            using var reader = new StreamReader(blobStream);
            string content = await reader.ReadToEndAsync();

            // Extract Full Name
            var nameMatch = Regex.Match(content, @"Name\s*[:\-]?\s*(.*)");
            string fullName = nameMatch.Success ? nameMatch.Groups[1].Value.Trim() : "Unknown";

            // Extract Department
            var deptMatch = Regex.Match(content, @"Department\s*[:\-]?\s*(.*)");
            string department = deptMatch.Success ? deptMatch.Groups[1].Value.Trim() : "Unknown";

            // Auto-generate EmpCode
            string empCode = await _codeGenerator.GenerateidAsync();

            // Building the Model for Employee Details
            var employee = new AddEmployeeRequest
            {
                
                id = empCode,
                FullName = fullName,
                Department = department,
                Email = "",
                PhoneNumber = "",
                Role = "",
                DateOfJoining = DateTime.UtcNow,
                EmploymentType = "Unknown",
                ReportingManager = "",
                Location = "",
                
            };

            // Saving the Data to cosmus
            await _cosmosRepo.AddAsync(employee);

            await _dataLog.LogInfoAsync(
                $"Employee saved to Cosmos. EmpCode: {empCode}, Name: {fullName}"
            );

            // Send Email
            string subject = $"Employee Document Processed ({empCode})";
            string body =
                $"A new employee document was processed.\n\n" +
                $"Employee Code: {empCode}\n" +
                $"Name: {fullName}\n" +
                $"Department: {department}\n" +
                $"File: {name}\n" +
                $"Uploaded On: {DateTime.UtcNow}";

            await _emailService.SendEmailAsync("hr-notify@company.com", subject, body);

            _logger.LogInformation("HR notification email sent.");
        }
    }
}
