var builder = DistributedApplication.CreateBuilder(args);

var proxy = builder.AddProject<Projects.Onlyoffice_ProxyServer>("r7-proxy-server");
var feedback = builder.AddProject<Projects.Cardmngr_FeedbackService>("feedback");

builder.AddProject<Projects.Cardmngr_Server>("webapp")
    .WithExternalHttpEndpoints()
    .WithReference(proxy)
    .WithReference(feedback);

builder.Build().Run();
