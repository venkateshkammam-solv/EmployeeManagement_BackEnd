using AzureFunctionPet.Models;
using AzureFunctions.Models;
using AzureFunctions_Triggers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureFunctionPet.Repositories
{
    public interface IEmployeeRepository
    {
        Task VerifyCreatedAsync();
        Task<AddEmployeeRequest> AddAsync(AddEmployeeRequest addEmployeeRequest );
        Task<DocumentMetadata> AddDocumentDataAsync(DocumentMetadata documentMetadata );
        Task<IEnumerable<EmployeeDetailsDto>> GetAllEmployeesAsync();
        Task<EmployeeDetailsDto?> GetEmployeeByIdAsync(string id);
        Task UpdateEmployeeAsync(EmployeeDetailsDto model);
        Task DeleteEmployeeAsync(string id);
        Task<string> GetLatestidAsync();
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
