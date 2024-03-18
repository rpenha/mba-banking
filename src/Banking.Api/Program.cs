using System.Text.Json.Serialization;
using Banking.Api.Endpoints;
using Banking.Application;
using Banking.Application.EntityFramework;
using Banking.Application.EntityFramework.Repositories;
using Banking.Application.Features.Customers;
using Banking.Core.Accounts;
using Banking.Core.Customers;
using Banking.Core.Transactions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region MediatR

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCustomerHandler>());
    
#endregion

#region EntityFramework

builder.Services.AddDbContext<BankingDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseNpgsql(connectionString)
           .UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped<IUnitOfWorkFactory, EntityFrameworkUnitOfWorkFactory<BankingDbContext>>();

#region Repositories

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

#endregion


#region Swagger

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#endregion

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


var app = builder.Build();

#region Swagger

app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

#endregion

app.MapGroup("/v1/customers").MapCustomersApi();
app.MapGroup("/v1/accounts").MapTransactionsApi();

app.Run();