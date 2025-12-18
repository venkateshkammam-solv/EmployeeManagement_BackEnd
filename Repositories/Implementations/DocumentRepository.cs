using AzureFunctions_Triggers.Repositories.Constants;
using AzureFunctions.Models;
using AzureFunctions_Triggers.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using AzureFunctions_Triggers.Models;

namespace AzureFunctions_Triggers.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly CosmosClient _client;
        private readonly string _databaseId;
        private readonly string _containerId;

        private Database _db;
        private Container _container;

        public DocumentRepository(CosmosClient client, string databaseId, string containerId)
        {
            _client = client;
            _databaseId = databaseId;
            _containerId = containerId;
        }

        private async Task InitializeAsync()
        {
            if (_db == null)
                _db = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);

            if (_container == null)
            {
                _container = await _db.CreateContainerIfNotExistsAsync(
                    new ContainerProperties
                    {
                        Id = _containerId,
                        PartitionKeyPath = "/id"
                    });
            }
        }

        public async Task VerifyCreatedAsync() => await InitializeAsync();

        public async Task<IEnumerable<DocumentData>> GetAllEmployeeVerificationDocuments()
        {
            await InitializeAsync();  

            var query = new QueryDefinition(CosmosQueries.GetEmployeeDocuments);

            var iterator = _container.GetItemQueryIterator<DocumentData>(query);

            var results = new List<DocumentData>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}
