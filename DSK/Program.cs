using AutoMapper;
using DSK.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);
// Validate scopes to ensure scoped and transient services are never injected in singletons.
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateOnBuild = true;
    options.ValidateScopes = true;
});

var isDevOrQA = builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("QA");

//builder.Services
//    .ConfigureInfrastructure(builder.Configuration, isDevOrQA)
//    .ConfigureApplication();
//builder.ConfigureWebApplication();

//builder.Services.AddServiceEndpointsRegistration(options =>
//{
//    options.AddServiceEndpoint(serviceName: "auth-provider-rapyd", serviceVersion: "1", new() { Address = "/api/v1/auth-processor", Timeout = 60000 });
//});

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

//await app.Services.RegisterServiceEndpointsAsync();

app.Run();