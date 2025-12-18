using System;
namespace AzureFunctions_Triggers.Repositories.Constants
{
    public static class CosmosQueries
    {
        public const string GetAllEmployees = "SELECT * FROM c WHERE c.id NOT LIKE \"%empDoc%\"";

        public const string GetEmployeeById = "SELECT * FROM c WHERE c.id = @id";

        public const string GetLatestEmployeeId ="SELECT VALUE c.id FROM c WHERE STARTSWITH(c.id, 'emp') AND NOT STARTSWITH(c.id, 'empDoc') " + "ORDER BY c.id DESC OFFSET 0 LIMIT 1";

        public const string GetLatestDocId = "SELECT VALUE c.id FROM c WHERE STARTSWITH(c.id, 'empDoc') " + "ORDER BY c.id DESC OFFSET 0 LIMIT 1";

        public const string GetEmployeeDocuments = "SELECT * FROM c WHERE c.type = 'Document'";
    }
}
