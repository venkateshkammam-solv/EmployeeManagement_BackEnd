using System.Threading.Tasks;
using AzureFunctionPet.Repositories;

namespace Shared.Services
{
    public class IdGenerator 
    {
        private readonly ICosmosRepository _cosmosRepository;

        public IdGenerator (ICosmosRepository cosmosRepository)
        {
            _cosmosRepository = cosmosRepository;
        }

        public async Task<string> GenerateidAsync()
        {
            //latest employee code
            string lastCode = await _cosmosRepository.GetLatestidAsync();

            if (string.IsNullOrEmpty(lastCode))
                return "emp001";

            int nextNumber = int.Parse(lastCode.Substring(3)) + 1;
            return $"emp{nextNumber:D3}";
        }
    }
}
