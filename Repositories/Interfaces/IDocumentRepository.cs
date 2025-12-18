using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureFunctions.Models;
using AzureFunctions_Triggers.Models;

namespace AzureFunctions_Triggers.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<DocumentData>> GetAllEmployeeVerificationDocuments();
    }
}
