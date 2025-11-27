using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using Shared.Services;
using AzureFunctionPet.Repositories;
using AzureFunctionPet.Services;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, services) =>
    {
        var cfg = ctx.Configuration;

        // Cosmos DB Connection
        var cosmosClient = new CosmosClient(cfg["Cosmos_Endpoint"], cfg["Cosmos_Key"]);
        services.AddSingleton<IEmployeeRepository>(
            new EmployeeRepository(
            cosmosClient,
            cfg["Cosmos_Database"],
            cfg["Cosmos_Container"]
        ));

        // Blob Storage connection
        services.AddSingleton<BlobServiceClient>(_ =>
            new BlobServiceClient(cfg["AzureWebJobsStorage"])
        );

        services.AddSingleton(sp =>
            cfg["BlobContainerName"] ?? "uploaded-docs"
        );

        // Queue Storage
        var queueClient = new QueueClient(cfg["AzureWebJobsStorage"], cfg["QueueName"] ?? "employee-events");
        queueClient.CreateIfNotExists();
        services.AddSingleton<IQueueRepository>(new QueueRepository(queueClient));

        // App Services
        services.AddSingleton<DataLog>();
        services.AddSingleton<IEmployeeService, EmployeeService>();
        services.AddSingleton<IdGenerator >();
        services.AddSingleton<EmailService>();
    })
    .Build();

await host.RunAsync();
