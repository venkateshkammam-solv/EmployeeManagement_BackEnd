using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using AzureFunctions_Triggers.Services;
using AzureFunctions_Triggers.Models;
using Shared.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using AzureFunctions_Triggers.Shared.Constants;
using MyAzureFunctionApp.Shared;

namespace AzureFunctions_Triggers.Functions
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddEmployeeDetails")]
            HttpRequestData req)
        {
            try
            {
                _logger.LogInformation(Messages.RequestReceived);

                var body = await new StreamReader(req.Body).ReadToEndAsync();
                var addEmployeeRequest = JsonSerializer.Deserialize<AddEmployeeRequest>(
                    body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (addEmployeeRequest == null)
                {
                    _logger.LogWarning(Messages.InvalidRequest);
                    var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                    await bad.WriteStringAsync(Messages.InvalidRequest);
                    return bad;
                }
                if (!addEmployeeRequest.DateOfBirth.HasValue)
                {
                    _logger.LogWarning(Messages.DateOfBirthRequired);
                    var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                    await bad.WriteStringAsync(Messages.DateOfBirthRequired);
                    return bad;
                }

                if (!ValidationHelper.IsDobMatchingAge(
                        addEmployeeRequest.DateOfBirth.Value,
                        addEmployeeRequest.Age))
                {
                    _logger.LogWarning(Messages.DobAgeMismatch);
                    var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                    await bad.WriteStringAsync(Messages.DobAgeMismatch);
                    return bad;
                }
                var emailExists = await _service.IsEmailExistsAsync(addEmployeeRequest.Email);
                var phoneExists = await _service.IsPhoneNumberExistsAsync(addEmployeeRequest.PhoneNumber);

                var errors = new List<string>();

                if (emailExists)
                    errors.Add(Messages.EmailAlreadyExists);

                if (phoneExists)
                    errors.Add(Messages.PhoneAlreadyExists);

                if (errors.Any())
                {
                    var bad = req.CreateResponse(HttpStatusCode.Conflict);
                    await bad.WriteAsJsonAsync(new { errors });
                    return bad;
                }
                addEmployeeRequest.id = await _codeGenerator.GenerateidAsync(" ");
                var created = await _service.HandleIncomingEmployeeEventAsync(addEmployeeRequest);

                _logger.LogInformation(Messages.EmployeeCreated);
                var resp = req.CreateResponse(HttpStatusCode.Created);
                await resp.WriteAsJsonAsync(created);
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.ProcessingError);
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync(Messages.InternalServerError);
                return errorResponse;
            }
        }
    }
}
