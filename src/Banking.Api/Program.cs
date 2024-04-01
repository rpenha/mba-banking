using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json.Serialization;
using Banking.Api.Endpoints;
using Banking.Application;
using Banking.Application.EntityFramework;
using Banking.Application.EntityFramework.Repositories;
using Banking.Application.Features.Customers;
using Banking.Core.Accounts;
using Banking.Core.Customers;
using Banking.Core.Transactions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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


#region Authentication/Authorization

// const string realm = "banking";
// const string authority = $"http://localhost:8080/realms/{realm}";
// const bool requireHttpMetadata = false;
//const string clientId = "banking-webapp";

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

#region Swagger


builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     // c.AddSecurityDefinition("oauth2",
//     //                         new OpenApiSecurityScheme
//     //                         {
//     //                             Type = SecuritySchemeType.OAuth2,
//     //                             Name = "Bearer",
//     //                             In = ParameterLocation.Header,
//     //                             Flows = new OpenApiOAuthFlows
//     //                                     {
//     //                                         Password = new OpenApiOAuthFlow
//     //                                                    {
//     //                                                        TokenUrl = new Uri("http://localhost:8080/realms/banking/protocol/openid-connect/token", UriKind.Absolute),
//     //                                                        Scopes = new Dictionary<string, string>
//     //                                                                 {
//     //                                                                     // { "readAccess", "Access read operations" },
//     //                                                                     // { "writeAccess", "Access write operations" }
//     //                                                                 }
//     //                                                    }
//     //                                     }
//     //                         });
// });

#endregion

#endregion

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

#region Swagger

// app.UseSwagger();
// app.UseSwaggerUI(options =>
// {
//     options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//     options.RoutePrefix = string.Empty;
//
//     options.OAuthClientId(clientId);
//     //options.OAuthClientSecret(clientSecret);
//     options.OAuthRealm(realm);
//     //options.OAuthAppName("test-app");
//     //options.OAuthScopeSeparator(" ");
//     //options.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "foo", "bar" }});
//     //options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
// });

#endregion

app.MapHealthChecks("/healthz");
app.MapGroup("/api/customers").MapCustomersApi();
app.MapGroup("/api/accounts").MapTransactionsApi();

app.Run();