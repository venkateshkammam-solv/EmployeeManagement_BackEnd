using AzureFunctions_Triggers.Models;
using AzureFunctions.Models;

namespace AzureFunctions_Triggers.Services
{
    public interface IEmployeeService
    {
        Task<AddEmployeeRequest> HandleIncomingEmployeeEventAsync(AddEmployeeRequest addEmployeeRequest);
        Task SaveOrUpdateDocumentAsync(DocumentMetadata documentMetaData);
        Task<IEnumerable<EmployeeDetailsDto>> GetEmployeeEventsAsync();
        Task<EmployeeDetailsDto?> GetEmployeeEventByIdAsync(string id);
		Task UpdateEmployeeAsync(EmployeeDetailsDto model);
		Task DeleteEmployeeAsync(string id);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);

    }
}
