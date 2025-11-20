using System;
namespace AzureFunctionPet.Repositories.Constants
{
    public static class CosmosQueries
    {
        public const string GetAllEmployees = "SELECT * FROM c";

        public const string GetEmployeeById = "SELECT * FROM c WHERE c.id = @id";

        public const string GetLatestEmployeeId = "SELECT VALUE c.id FROM c ORDER BY c.id DESC OFFSET 0 LIMIT 1";
    }
}
