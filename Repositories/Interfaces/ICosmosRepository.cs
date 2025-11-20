using AzureFunctionPet.Models;
using AzureFunctions_Triggers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureFunctionPet.Repositories
{
    public interface ICosmosRepository
    {
        Task VerifyCreatedAsync();
        Task<AddEmployeeRequest> AddAsync(AddEmployeeRequest addEmployeeRequest );
        Task<IEnumerable<EmployeeDetailsDto>> GetAllEmployeesAsync();
        Task<EmployeeDetailsDto?> GetEmployeeByIdAsync(string id);
        Task UpdateEmployeeAsync(EmployeeDetailsDto model);
        Task DeleteEmployeeAsync(string id);
        Task<string> GetLatestidAsync();
    }
}
