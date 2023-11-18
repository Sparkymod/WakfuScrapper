using Serilog;
using System.Text.Json.Serialization.Metadata;
using WakfuScrapper.Api.Extensions;
using WakfuScrapper.Api.Features.ArmorFeature;
using WakfuScrapper.Api.Middleware;

var builder = WebApplication.CreateSlimBuilder(args);
var config = builder.Configuration;

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
});

// Set up logging
builder.Host.UseSerilog(SerilogExtensions.InitializeSerilog(config));

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddScoped<ArmorServices>();
builder.Services.AddScoped<ArmorScrapperService>();

var app = builder.Build();

// Middleware to catch exceptions
app.UseExceptionCatcherMiddleware();

// Set up Serilog request logging
app.UseSerilogRequestLogging();
app.MapApplicationEndpoints();

app.MapHealthChecks("/health");
app.Run();