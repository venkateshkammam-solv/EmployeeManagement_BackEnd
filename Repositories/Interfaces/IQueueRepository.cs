using System.Threading.Tasks;

namespace AzureFunctions_Triggers.Repositories
{
    public interface IQueueRepository
    {
        Task EnqueueAsync(string message);
    }
}
