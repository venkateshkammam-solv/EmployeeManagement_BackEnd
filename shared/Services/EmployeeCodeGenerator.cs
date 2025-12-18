
using AzureFunctions_Triggers.Repositories;

namespace Shared.Services
{
    public class IdGenerator 
    {
        private readonly IEmployeeRepository _employeeRepository;

        public IdGenerator (IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<string> GenerateidAsync(string codeType)
        {
            string lastCode;

            if (codeType == "doc")
            {
                lastCode = await _employeeRepository.GetLatestDocIdAsync();

                if (string.IsNullOrEmpty(lastCode))
                    return "empDoc001";

                
                int nextNumber = int.Parse(lastCode.Substring(6)) + 1;

                return $"empDoc{nextNumber:D3}";
            }
            else 
            {
                lastCode = await _employeeRepository.GetLatestEmpidAsync();

                if (string.IsNullOrEmpty(lastCode))
                    return "emp001";

                // "emp" length = 3, numeric part starts from index 3
                int nextNumber = int.Parse(lastCode.Substring(3)) + 1;

                return $"emp{nextNumber:D3}";
            }
        }

    }
}
