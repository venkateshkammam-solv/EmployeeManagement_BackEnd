using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureFunctions_Triggers.Shared.Services
{
    public static class HttpResponseHelper
    {
        public static async Task<HttpResponseData> CreateErrorResponse(HttpRequestData req, HttpStatusCode code, string message)
        {
            var response = req.CreateResponse(code);
            await response.WriteStringAsync(message);
            return response;
        }
    }
}
