using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using AzureFunctionPet.Services;
using AzureFunctions_Triggers.Models;
using Shared.Services;
using MyAzureFunctionApp.Shared;
using Microsoft.Extensions.Logging;

namespace AzureFunctionPet.Functions
{
    public class EmployeeRequest
    {
        private readonly IEmployeeService _service;
        private readonly IdGenerator _codeGenerator;
        private readonly ILogger _logger;

        public EmployeeRequest(
            IEmployeeService service,
            IdGenerator codeGenerator,
            ILoggerFactory loggerFactory)
        {
            _service = service;
            _codeGenerator = codeGenerator;
            _logger = loggerFactory.CreateLogger<EmployeeRequest>();
        }

        [Function("EmployeeEventFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddEmployeeDetails")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("AddEmployeeDetails request received");

                var body = await new StreamReader(req.Body).ReadToEndAsync();
                var addEmployeeRequest = JsonSerializer.Deserialize<AddEmployeeRequest>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (addEmployeeRequest == null)
                {
                    _logger.LogWarning("Invalid request body.");
                    var bad = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    await bad.WriteStringAsync("Invalid request body.");
                    return bad;
                }

                if (!addEmployeeRequest.DateOfBirth.HasValue)
                {
                    _logger.LogWarning("Date of Birth is required.");
                    var bad = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    await bad.WriteStringAsync("Date of Birth is required.");
                    return bad;
                }

                if (!ValidationHelper.IsDobMatchingAge(addEmployeeRequest.DateOfBirth.Value, addEmployeeRequest.Age))
                {
                    _logger.LogWarning("DOB does not match provided Age.");
                    var bad = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    await bad.WriteStringAsync($"Date of Birth does not match Age {addEmployeeRequest.Age}.");
                    return bad;
                }

                addEmployeeRequest.id = await _codeGenerator.GenerateidAsync();
                var created = await _service.HandleIncomingEmployeeEventAsync(addEmployeeRequest);

                _logger.LogInformation("Employee record created successfully.");

                var resp = req.CreateResponse(System.Net.HttpStatusCode.Created);
                await resp.WriteAsJsonAsync(created);
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing AddEmployeeDetails request.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Internal server error.");
                return errorResponse;
            }
        }
    }
}
