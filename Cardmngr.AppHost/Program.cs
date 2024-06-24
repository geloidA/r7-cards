var builder = DistributedApplication.CreateBuilder(args);

var proxy = builder.AddProject<Projects.Onlyoffice_ProxyServer>("onlyoffice");
var feedback = builder.AddProject<Projects.Cardmngr_FeedbackService>("feedback");

builder.AddProject<Projects.Cardmngr_Server>("server")
    .WithExternalHttpEndpoints()
    .WithReference(proxy)
    .WithReference(feedback);

builder.Build().Run();