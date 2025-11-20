using AzureFunctionPet.Models;
using AzureFunctions_Triggers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureFunctionPet.Services
{
    public interface IEmployeeService
    {
        Task<AddEmployeeRequest> HandleIncomingEmployeeEventAsync(AddEmployeeRequest addEmployeeRequest );
        Task<IEnumerable<EmployeeDetailsDto>> GetEmployeeEventsAsync();
        Task<EmployeeDetailsDto?> GetEmployeeEventByIdAsync(string id);
		Task UpdateEmployeeAsync(EmployeeDetailsDto model);
		Task DeleteEmployeeAsync(string id);
	}
}
