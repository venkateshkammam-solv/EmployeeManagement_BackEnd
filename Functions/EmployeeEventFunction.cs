using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using AzureFunctionPet.Services;
using AzureFunctionPet.Models;
using Newtonsoft.Json;
using AzureFunctions_Triggers.Models;

namespace AzureFunctionPet.Functions
{
    public class EmployeeEventFunction
    {
        private readonly IEmployeeService _service;
        private readonly ILogger<EmployeeEventFunction> _logger;

        public EmployeeEventFunction(IEmployeeService service, ILogger<EmployeeEventFunction> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Function("GetEvents")]
        public async Task<HttpResponseData> GetAll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employeeDetails")] HttpRequestData req)
        {
            _logger.LogInformation("Fetching all employee records.....");
            var data = await _service.GetEmployeeEventsAsync();
            _logger.LogInformation("Employee data fetched successfully.");

            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(data);
            return resp;
        }

        [Function("GetEventById")]
        public async Task<HttpResponseData> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employeeDetails/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation("Fetching employee record by ID.");
            var employee = await _service.GetEmployeeEventByIdAsync(id);

            if (employee == null)
            {
                _logger.LogWarning("Record not found for given ID.");
                var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteStringAsync("Record not found for given Id");
                return notFound;
            }

            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(employee);
            _logger.LogInformation("Record retrieved successfully.");
            return resp;
        }

        [Function("UpdateEvent")]
        public async Task<HttpResponseData> UpdateEvent(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updateEmployeeDetailsById/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation("Updating employee record...");
            var json = await req.ReadAsStringAsync();
            var updated = JsonConvert.DeserializeObject<EmployeeDetailsDto>(json);

            if (updated == null)
            {
                _logger.LogWarning("Invalid request body for update operation.");
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Invalid request body");
                return badRequest;
            }

            updated.id = id;
            await _service.UpdateEmployeeAsync(updated);

            _logger.LogInformation("Employee details updated successfully.");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { message = "Employee details updated successfully", id });

            return response;
        }

        [Function("DeleteEvent")]
        public async Task<HttpResponseData> DeleteEvent(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteEmployeeDataById/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation("Deleting employee record...");
            await _service.DeleteEmployeeAsync(id);
            _logger.LogInformation("Employee record deleted successfully.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { message = "Deleted successfully", id });

            return response;
        }
    }
}
