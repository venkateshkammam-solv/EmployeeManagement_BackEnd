
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using AzureFunctionPet.Services;
using AzureFunctions_Triggers.Models;
using Shared.Services;
using MyAzureFunctionApp.Shared;

namespace AzureFunctionPet.Functions
{
    public class EmployeeRequest
    {
        private readonly IEmployeeService _service;
        private readonly DataLog _dataLog;
        private readonly IdGenerator  _codeGenerator;

        public EmployeeRequest(
            IEmployeeService service,
            DataLog dataLog,
            IdGenerator  codeGenerator)
        {
            _service = service;
            _dataLog = dataLog;
            _codeGenerator = codeGenerator;
        }

        [Function("EmployeeEventFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddEmployeeDetails")] HttpRequestData req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            await _dataLog.LogInfoAsync($"Request received: {body}");
            var addEmployeeRequest  = JsonSerializer.Deserialize<AddEmployeeRequest>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (addEmployeeRequest  == null)
            {
                var bad = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await _dataLog.LogErrorAsync("Invalid Request Body");
                await bad.WriteStringAsync("Invalid request body.");
                return bad;
            }
            if (!addEmployeeRequest .DateOfBirth.HasValue)
            {
                var bad = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await _dataLog.LogErrorAsync("Date of Birth is required.");
                await bad.WriteStringAsync("Date of Birth is required.");
                return bad;
            }

            if (!ValidationHelper.IsDobMatchingAge(addEmployeeRequest .DateOfBirth.Value, addEmployeeRequest .Age))
            {
                var bad = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await _dataLog.LogErrorAsync($"DOB {addEmployeeRequest .DateOfBirth.Value.ToShortDateString()} does not match Age {addEmployeeRequest .Age}.");
                await bad.WriteStringAsync($"Date of Birth does not match Age {addEmployeeRequest .Age}.");
                return bad;
            }
            addEmployeeRequest.id = await _codeGenerator.GenerateidAsync();
            var created = await _service.HandleIncomingEmployeeEventAsync(addEmployeeRequest );

            var resp = req.CreateResponse(System.Net.HttpStatusCode.Created);
            await resp.WriteAsJsonAsync(created);
            return resp;
        }
    }
}
