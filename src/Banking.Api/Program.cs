using Banking.Api.Endpoints;
using Banking.Application.EntityFramework;
using Banking.Application.EntityFramework.Repositories;
using Banking.Application.Features.Customers;
using Banking.Core.Accounts;
using Banking.Core.Customers;
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

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapGroup("/v1/customers").MapCustomerApi();
app.MapGroup("/v1/checking-accounts").MapChekingAccountApi();

app.Run();