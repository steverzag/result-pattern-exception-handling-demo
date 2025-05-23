using Microsoft.EntityFrameworkCore;
using ResultPatternExceptionHandlingDemo.API.Data;
using ResultPatternExceptionHandlingDemo.API.Endpoints.Configuration;
using ResultPatternExceptionHandlingDemo.API.Services;
using ResultPatternExceptionHandlingDemo.API.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDBContext>(options =>
	options.UseInMemoryDatabase("ResultPatternExceptionHandlingDemo"));

builder.Services.AddOpenApi();
builder.Services.AddScoped<UserService>();

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.RegisterEndpoints();
app.UseExceptionHandler();

app.Run();

