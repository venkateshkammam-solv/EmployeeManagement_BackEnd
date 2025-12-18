using Azure.Storage.Queues;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions_Triggers.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly QueueClient _queueClient;

        public QueueRepository(QueueClient queueClient)
        {
            _queueClient = queueClient;
            _queueClient.CreateIfNotExists();
        }

        public async Task EnqueueAsync(string message)
        {
            await _queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
        }
    }
}
