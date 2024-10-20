var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.EventsApp_AuthorisationService>("eventsapp-authorisationservice");

builder.AddProject<Projects.EventsApp_EventsService>("eventsapp-eventsservice");

builder.AddProject<Projects.EventsApp_ApiGateway>("eventsapp-apigateway");

builder.Build().Run();
