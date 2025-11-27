using AzureFunctionPet.Models;
using AzureFunctions.Models;
using AzureFunctions_Triggers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureFunctionPet.Services
{
    public interface IEmployeeService
    {
        Task<AddEmployeeRequest> HandleIncomingEmployeeEventAsync(AddEmployeeRequest addEmployeeRequest);
        Task<string> HandleAddDocumentAsync(DocumentMetadata documentMetaData);
        Task<IEnumerable<EmployeeDetailsDto>> GetEmployeeEventsAsync();
        Task<EmployeeDetailsDto?> GetEmployeeEventByIdAsync(string id);
		Task UpdateEmployeeAsync(EmployeeDetailsDto model);
		Task DeleteEmployeeAsync(string id);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);

    }
}
