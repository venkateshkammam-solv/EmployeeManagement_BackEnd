using Microsoft.Azure.Cosmos;
using AzureFunctionPet.Models;
using AzureFunctions_Triggers.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionPet.Repositories.Constants;

namespace AzureFunctionPet.Repositories
{
	public class CosmosRepository : ICosmosRepository
	{
		private readonly CosmosClient _client;
		private readonly string _databaseId;
		private readonly string _containerId;

		private Database _db;
		private Container _container;

		public CosmosRepository(CosmosClient client, string databaseId, string containerId)
		{
			_client = client;
			_databaseId = databaseId;
			_containerId = containerId;
		}

		private async Task InitializeDatabaseAsync()
		{
			if (_db == null)
				_db = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);

			if (_container == null)
			{
				_container = await _db.CreateContainerIfNotExistsAsync(new ContainerProperties
				{
					Id = _containerId,
					PartitionKeyPath = "/id"
				});
			}
		}

		public async Task VerifyCreatedAsync() => await InitializeDatabaseAsync();

		public async Task<AddEmployeeRequest> AddAsync(AddEmployeeRequest addEmployeeRequest)
		{
			await InitializeDatabaseAsync();
			var response = await _container.CreateItemAsync(addEmployeeRequest, new PartitionKey(addEmployeeRequest.id));
			return response.Resource;
		}

		public async Task<IEnumerable<EmployeeDetailsDto>> GetAllEmployeesAsync()
		{
			await InitializeDatabaseAsync();
			var iterator = _container.GetItemQueryIterator<EmployeeDetailsDto>(new QueryDefinition(CosmosQueries.GetAllEmployees));
			var results = new List<EmployeeDetailsDto>();

			while (iterator.HasMoreResults)
			{
				var response = await iterator.ReadNextAsync();
				results.AddRange(response);
			}

			return results;
		}

		public async Task<EmployeeDetailsDto?> GetEmployeeByIdAsync(string employeeId)
		{
			await InitializeDatabaseAsync();
			var query = new QueryDefinition(CosmosQueries.GetEmployeeById)
				.WithParameter("@id", employeeId);

			var iterator = _container.GetItemQueryIterator<EmployeeDetailsDto>(query);

			if (iterator.HasMoreResults)
			{
				var response = await iterator.ReadNextAsync();
				return response.FirstOrDefault();
			}

			return null;
		}

		public async Task UpdateEmployeeAsync(EmployeeDetailsDto model)
		{
			await InitializeDatabaseAsync();
			await _container.UpsertItemAsync(model, new PartitionKey(model.id));
		}

		public async Task DeleteEmployeeAsync(string id)
		{
			await InitializeDatabaseAsync();
			await _container.DeleteItemAsync<EmployeeDetailsDto>(id, new PartitionKey(id));
		}

		public async Task<string?> GetLatestidAsync()
		{
			await InitializeDatabaseAsync();
			var iterator = _container.GetItemQueryIterator<string>(new QueryDefinition(CosmosQueries.GetLatestEmployeeId));

			if (iterator.HasMoreResults)
			{
				var response = await iterator.ReadNextAsync();
				return response.FirstOrDefault();
			}

			return null;
		}
	}
}
