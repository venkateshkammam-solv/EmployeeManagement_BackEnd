using AzureFunctionPet.Models;
using AzureFunctionPet.Repositories;
using AzureFunctions_Triggers.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureFunctionPet.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICosmosRepository _cosmosRepo;
        private readonly IQueueRepository _queueRepo;

        public EmployeeService(ICosmosRepository cosmosRepo, IQueueRepository queueRepo)
        {
            _cosmosRepo = cosmosRepo;
            _queueRepo = queueRepo;
        }

        public async Task<AddEmployeeRequest> HandleIncomingEmployeeEventAsync(AddEmployeeRequest addEmployeeRequest )
        {
            var created = await _cosmosRepo.AddAsync(addEmployeeRequest );
            await _queueRepo.EnqueueAsync(JsonSerializer.Serialize(new { id= created.id, eventType = created.Email }));
            return created;
        }

        public Task<IEnumerable<EmployeeDetailsDto>> GetEmployeeEventsAsync() {
            return _cosmosRepo.GetAllEmployeesAsync();
        }

        public Task<EmployeeDetailsDto?> GetEmployeeEventByIdAsync(string id)
        {
            return _cosmosRepo.GetEmployeeByIdAsync(id);
        }
        public Task UpdateEmployeeAsync(EmployeeDetailsDto model)
        {
           return  _cosmosRepo.UpdateEmployeeAsync(model);
        }

        public Task DeleteEmployeeAsync(string id)
        {
            return _cosmosRepo.DeleteEmployeeAsync(id);
        }
    }
}
