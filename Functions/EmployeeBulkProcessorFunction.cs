using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureFunctionPet.Models;
using AzureFunctionPet.Repositories;
using AzureFunctions_Triggers.Models;
using Shared.Services;
using ExcelDataReader;
using System.Data;
using AzureFunctionPet.Constants;

namespace AzureFunctionPet.Functions
{
    public class EmployeeDocumentProcessorFunction
    {
        private readonly ILogger<EmployeeDocumentProcessorFunction> _logger;
        private readonly EmailService _emailService;
        private readonly IEmployeeRepository _cosmosRepo;
        private readonly IdGenerator _codeGenerator;

        public EmployeeDocumentProcessorFunction(
            ILogger<EmployeeDocumentProcessorFunction> logger,
            EmailService emailService,
            IdGenerator codeGenerator,
            IEmployeeRepository cosmosRepo)
        {
            _logger = logger;
            _emailService = emailService;
            _cosmosRepo = cosmosRepo;
            _codeGenerator = codeGenerator;
        }

        [Function("EmployeeDocumentProcessor")]
        public async Task RunAsync(
            [BlobTrigger("uploads/{name}", Connection = "AzureWebJobsStorage")] Stream blobStream,
            string name)
        {
            _logger.LogInformation("EmployeeDocumentProcessor triggered for file");

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using var excelReader = ExcelReaderFactory.CreateReader(blobStream);
                var result = excelReader.AsDataSet();
                var table = result.Tables[0];

                _logger.LogInformation("Total records found: {RecordCount}", table.Rows.Count - 1);

                for (int i = 1; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    string empCode = await _codeGenerator.GenerateidAsync("");

                    var employee = new AddEmployeeRequest
                    {
                        id = empCode,
                        FullName = row[0]?.ToString(),
                        Email = row[1]?.ToString(),
                        PhoneNumber = row[2]?.ToString(),
                        Department = row[3]?.ToString(),
                        Role = row[4]?.ToString(),
                        Age = Convert.ToInt32(row[5]),
                        DateOfBirth = DateTime.Parse(row[6]?.ToString()),
                        DateOfJoining = DateTime.Parse(row[7]?.ToString()),
                        EmploymentType = row[8]?.ToString(),
                        ReportingManager = row[9]?.ToString(),
                        Location = row[10]?.ToString()
                    };

                    await _cosmosRepo.AddAsync(employee);
                    _logger.LogInformation("Employee record added to database successfully.");

                    string subject = $"Welcome to the Company, {employee.FullName}!";
                    string body = EmailTemplates.EmployeeOnboarding
                        .Replace("{{EmployeeName}}", employee.FullName)
                        .Replace("{{EmployeeCode}}", employee.id)
                        .Replace("{{Department}}", employee.Department)
                        .Replace("{{DateOfJoining}}", employee.DateOfJoining.ToString("yyyy-MM-dd"));

                    await _emailService.SendEmailAsync(employee.Email, subject, body);

                    _logger.LogInformation("Onboarding email sent successfully.");
                }

                _logger.LogInformation("Excel processing completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during Excel data processing.");
                throw; 
            }
        }
    }
}
