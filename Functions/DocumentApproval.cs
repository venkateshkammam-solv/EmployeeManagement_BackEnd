
using System.Net;
using AzureFunctions_Triggers.Models;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using MyAzureFunctionApp.Shared;
using AzureFunctions_Triggers.Shared.Constants;

namespace AzureFunctions_Triggers.Functions
{
    public class DocumentApproval
    {
        [Function("DocumentApproval")]
    public async Task<HttpResponseData> DocumentApproveOrReject([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
      [DurableClient] DurableTaskClient client)
        {
            var approval = await req.ReadFromJsonAsync<ApprovalResult>();

            await client.RaiseEventAsync(
                approval.InstanceId,
                "DocumentApprovalEvent",
                approval);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new
            {
                message = Messages.ApprovedMessage,
                instanceId = approval.InstanceId
            });
            return response;
        }

    }
}
