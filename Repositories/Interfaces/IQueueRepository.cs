using System.Threading.Tasks;

namespace AzureFunctionPet.Repositories
{
    public interface IQueueRepository
    {
        Task EnqueueAsync(string message);
    }
}
