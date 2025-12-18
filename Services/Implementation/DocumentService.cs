
using AzureFunctions.Models;
using AzureFunctions_Triggers.Models;
using AzureFunctions_Triggers.Repositories.Interfaces;
using AzureFunctions_Triggers.Services.Interfaces;

namespace AzureFunctions_Triggers.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        public async Task<IEnumerable<DocumentData>> GetAllEmployeeDocuments()
        {
            return await _documentRepository.GetAllEmployeeVerificationDocuments();
        }
    }
}
