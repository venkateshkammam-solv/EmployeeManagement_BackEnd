using AzureFunctionPet.Models;
using AzureFunctionPet.Repositories;
using AzureFunctions.Models;
using AzureFunctions_Triggers.Models;
using Shared.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureFunctionPet.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _cosmosRepo;
        private readonly IQueueRepository _queueRepo;
        private readonly IdGenerator _codeGenerator;

        public EmployeeService(IEmployeeRepository cosmosRepo, IQueueRepository queueRepo, IdGenerator codeGenerator)
        {
            _cosmosRepo = cosmosRepo;
            _queueRepo = queueRepo;
            _codeGenerator = codeGenerator;
        }

        public async Task<AddEmployeeRequest> HandleIncomingEmployeeEventAsync(AddEmployeeRequest addEmployeeRequest)
        {
            var created = await _cosmosRepo.AddAsync(addEmployeeRequest);

            var queuePayload = new
            {
                id = created.id,
                FullName = created.FullName,
                Email = created.Email,
                Department = created.Department,
                DateOfJoining = created.DateOfJoining
            };
            await _queueRepo.EnqueueAsync(JsonSerializer.Serialize(queuePayload));

            return created;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var checkEmail = await _cosmosRepo.IsEmailExistsAsync(email);

            if (!checkEmail) 
            {
                return false;
            }
            return true;
        }


        public async Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            var checkPhoneNumber = await _cosmosRepo.IsPhoneNumberExistsAsync(phoneNumber);
            if (!checkPhoneNumber) {
                return false;
            }
            return true;
        }

        public async Task<string> HandleAddDocumentAsync(DocumentMetadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }
            metadata.id = await _codeGenerator.GenerateidAsync("");
            await _cosmosRepo.AddDocumentDataAsync(metadata);

            return "Document created";
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
