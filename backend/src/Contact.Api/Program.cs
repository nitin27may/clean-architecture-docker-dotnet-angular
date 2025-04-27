using Contact.Api.Core.Authorization;
using Contact.Api.Core.Middleware;
using Contact.Application;
using Contact.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

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
        //options.Events = new JwtBearerEvents()
        //{
        //    OnAuthenticationFailed = c =>
        //    {
        //        c.NoResult();
        //        c.Response.StatusCode = 500;
        //        c.Response.ContentType = "text/plain";
        //        return c.Response.WriteAsync(c.Exception.ToString());
        //    },
        //    OnChallenge = context =>
        //    {
        //        context.HandleResponse();
        //        context.Response.StatusCode = 401;
        //        context.Response.ContentType = "application/json";
        //        var result = JsonSerializer.Serialize(new { Message = "You are not Authorized" });
        //        return context.Response.WriteAsync(result);
        //    },
        //    OnForbidden = context =>
        //    {
        //        context.Response.StatusCode = 403;
        //        context.Response.ContentType = "application/json";
        //        var result = JsonSerializer.Serialize(new { Message = "You are not authorized to access this resource" });
        //        return context.Response.WriteAsync(result);
        //    },
        //};
    });

builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ActivityLoggingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
