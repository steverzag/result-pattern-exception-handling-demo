var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ResultPatternExceptionHandlingDemo_API>("resultpatternexceptionhandlingdemo-api");

builder.Build().Run();
