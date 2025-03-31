using DSK.Api.Extensions;
using DSK.Application.Extensions;
using DSK.Application.UseCases.CreditProcessing.Commands;
using DSK.Infrastructure.Configurations;
using DSK.Infrastructure.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IHostBuilder host = Host.CreateDefaultBuilder();


builder.Services.AddControllers();

//var dbName = builder.Configuration.GetConnectionString("InMemoryDbName");
var connectionString = Path.Combine(
      Path.GetDirectoryName(Assembly.GetCallingAssembly().Location),
      "Files",
      "DSK.db");

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreditCommandHandler>();
    cfg.RegisterServicesFromAssemblyContaining<CreditSummaryCommanHandler>();
});

builder.Services.Configure<DatabaseSettings>(options =>
{
    options.ConnectionString = connectionString;
});

builder.Services
    .ConfigureInfrastructure(connectionString)
    .ConfigureApplication()
   .ConfigureWebApplication();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

