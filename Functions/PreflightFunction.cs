using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

public class PreflightFunction
{
    [Function("Preflight")]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "{*any}")] HttpRequestData req)
    {
        var res = req.CreateResponse(HttpStatusCode.OK);

        res.Headers.Add("Access-Control-Allow-Origin", "*");
        res.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        res.Headers.Add("Access-Control-Allow-Headers", "*");
        res.Headers.Add("Access-Control-Max-Age", "3600");

        return res;
    }
}
