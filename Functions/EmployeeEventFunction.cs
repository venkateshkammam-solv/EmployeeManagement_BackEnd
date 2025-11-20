using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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
        private readonly DataLog _dataLog;

        public EmployeeEventFunction(IEmployeeService service, DataLog dataLog)
        {
            _service = service;
            _dataLog = dataLog;
        }

        [Function("GetEvents")]
        public async Task<HttpResponseData> GetAll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employeeDetails")] HttpRequestData req)
        {
            var data = await _service.GetEmployeeEventsAsync();
            await _dataLog.LogInfoAsync("Employee Data Fetched Successfully");

            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(data);

            return resp;
        }

        [Function("GetEventById")]
        public async Task<HttpResponseData> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employeeDetails/{id}")] HttpRequestData req,
            string id)
        {
            var addEmployeeRequest  = await _service.GetEmployeeEventByIdAsync(id);

            if (addEmployeeRequest  == null)
            {
                await _dataLog.LogInfoAsync($"No record found for ID: {id}");

                var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteStringAsync("Event not found");
                return notFound;
            }

            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(addEmployeeRequest );

            return resp;
        }

        [Function("UpdateEvent")]
        public async Task<HttpResponseData> UpdateEvent(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updateEmployeeDetailsById/{id}")] HttpRequestData req,
            string id)
        {
            var json = await req.ReadAsStringAsync();
            var updated = JsonConvert.DeserializeObject<EmployeeDetailsDto>(json);

            if (updated == null)
            {
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Invalid request body");
                return badRequest;
            }

            updated.id = id;
            await _service.UpdateEmployeeAsync(updated);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { message = "Employee details updated successfully", id });

            return response;
        }

        [Function("DeleteEvent")]
        public async Task<HttpResponseData> DeleteEvent(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "DeleteEmployeeDataById/{id}")] HttpRequestData req,
            string id)
        {
            await _service.DeleteEmployeeAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { message = "Deleted successfully", id });

            return response;
        }
    }
}
