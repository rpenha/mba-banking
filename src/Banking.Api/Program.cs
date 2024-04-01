using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Banking.Api.Endpoints;
using Banking.Application;
using Banking.Application.EntityFramework;
using Banking.Application.EntityFramework.Repositories;
using Banking.Application.Features.Customers;
using Banking.Core.Accounts;
using Banking.Core.Customers;
using Banking.Core.Transactions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

#endregion

#region Repositories

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

#endregion


#region Authentication/Authorization

var keycloakSettings = new KeycloakSettings();
builder.Configuration.GetSection(KeycloakSettings.ConfigSectionName).Bind(keycloakSettings);
var (authority, requireHttpsMetadata) = keycloakSettings;

builder.Services
       .AddAuthentication(options =>
       {
           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       })
       .AddJwtBearer(options =>
       {
           options.Authority = authority;
           options.RequireHttpsMetadata = requireHttpsMetadata;
           options.TokenValidationParameters = new TokenValidationParameters
                                               {
                                                   ValidateAudience = false
                                               };
       });

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

#endregion

builder.Services.AddHealthChecks();
builder.Services.AddControllers();

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/healthz");
app.MapFallbackToFile("index.html");
app.MapGroup("/api/customers").MapCustomersApi();
app.MapGroup("/api/accounts").MapTransactionsApi();

app.Run();