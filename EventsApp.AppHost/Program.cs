var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.EventsApp_ApiGateway>("eventsapp-apigateway");

builder.AddProject<Projects.AuthorisationService_API>("authorisationservice-api");

builder.AddProject<Projects.EventsService_API>("eventsservice-api");

builder.Build().Run();
