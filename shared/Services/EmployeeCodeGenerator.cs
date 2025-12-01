
using AzureFunctionPet.Repositories;

namespace Shared.Services
{
    public class IdGenerator 
    {
        private readonly IEmployeeRepository _cosmosRepository;

        public IdGenerator (IEmployeeRepository cosmosRepository)
        {
            _cosmosRepository = cosmosRepository;
        }

        public async Task<string> GenerateidAsync(string codeType)
        {
            //latest employee code
            string lastCode = await _cosmosRepository.GetLatestidAsync();
            if (codeType == "doc")
            {
                if (string.IsNullOrEmpty(lastCode))
                    return "empDoc001";

                int nextNumber = int.Parse(lastCode.Substring(3)) + 1;
                return $"empDoc{nextNumber:D3}";
            }
            else
            {
                if (string.IsNullOrEmpty(lastCode))
                    return "emp001";

                int nextNumber = int.Parse(lastCode.Substring(3)) + 1;
                return $"emp{nextNumber:D3}";
            }
        }
    }
}
