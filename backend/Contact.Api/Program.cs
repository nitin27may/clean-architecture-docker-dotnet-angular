using Contact.Api.Core.Authorization;
using Contact.Api.Core.Middleware;
using Contact.Application;
using Contact.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddInfrastrcutureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var appSettings = builder.Configuration.GetSection("AppSettings").Get<Contact.Application.AppSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = appSettings.Issuer,
            ValidAudience = appSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret))
        };
    });

builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddControllers();

// Configure OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Contact API")
               .WithTheme(ScalarTheme.BluePlanet)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
               .WithPreferredScheme("Bearer")
               .WithHttpBearerAuthentication(bearer =>
               {
                   bearer.Token = "";
               });
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ActivityLoggingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
