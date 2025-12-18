using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using AzureFunctions_Triggers.Services;
using Newtonsoft.Json;
using AzureFunctions_Triggers.Models;
using AzureFunctions_Triggers.Shared.Constants;

namespace AzureFunctions_Triggers.Functions
{
    public class EmployeeEventFunction
    {
        private readonly IEmployeeService _service;
        private readonly ILogger<EmployeeEventFunction> _logger;

        public EmployeeEventFunction(
            IEmployeeService service,
            ILogger<EmployeeEventFunction> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Function("GetEvents")]
        public async Task<HttpResponseData> GetAll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employeeDetails")]
            HttpRequestData req)
        {
            _logger.LogInformation(Messages.FetchingAllEmployeeRecords);

            var data = await _service.GetEmployeeEventsAsync();

            _logger.LogInformation(Messages.EmployeeDataFetched);
            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(data);
            return resp;
        }

        [Function("GetEventById")]
        public async Task<HttpResponseData> GetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employeeDetails/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation(Messages.FetchingEmployeeById);

            var employee = await _service.GetEmployeeEventByIdAsync(id);

            if (employee == null)
            {
                _logger.LogWarning(Messages.EmployeeRecordNotFound);
                var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteStringAsync(Messages.EmployeeRecordNotFound);
                return notFound;
            }

            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(employee);

            _logger.LogInformation(Messages.EmployeeRecordRetrieved);
            return resp;
        }

        [Function("UpdateEvent")]
        public async Task<HttpResponseData> UpdateEvent(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updateEmployeeDetailsById/{id}")]
            HttpRequestData req, string id)
        {
            _logger.LogInformation(Messages.UpdatingEmployee);

            var json = await req.ReadAsStringAsync();
            var updated = JsonConvert.DeserializeObject<EmployeeDetailsDto>(json);

            if (updated == null)
            {
                _logger.LogWarning(Messages.InvalidUpdateRequest);
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync(Messages.InvalidUpdateRequest);
                return badRequest;
            }

            updated.id = id;
            await _service.UpdateEmployeeAsync(updated);

            _logger.LogInformation(Messages.EmployeeUpdated);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new
            {
                message = Messages.EmployeeUpdated,
                id
            });

            return response;
        }

        [Function("DeleteEvent")]
        public async Task<HttpResponseData> DeleteEvent(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteEmployeeDataById/{id}")]
            HttpRequestData req, string id)
        {
            await _service.DeleteEmployeeAsync(id);

            _logger.LogInformation(Messages.EmployeeDeleted);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new
            {
                message = Messages.DeleteSuccessMessage,
                id
            });

            return response;
        }
    }
}
