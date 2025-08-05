using OrderApp.Web.Configurations;
using OrderApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FluentValidation.AspNetCore;
using OrderApp.Infrastructure.Configuration;
using AutoWrapper;
using OrderApp.Web.Orders.Mappers;
using Ardalis.GuardClauses;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using OrderApp.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OrderApp.Infrastructure;
using OrderApp.Infrastructure.Interceptors;
using Microsoft.AspNetCore.Authorization;
using OrderApp.Web.Security.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OrderApp.Core.MailSeting;
using OrderApp.Infrastructure.Messaging;
using OrderApp.Infrastructure.RealTime;
using Microsoft.AspNetCore.Http.Connections;
using OrderApp.Web.RealTime;
using Microsoft.Extensions.Caching.Distributed;
using OrderApp.Web.Login;
using OpenTelemetry.Trace;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.Uris;
using HealthChecks.UI.Client;
//using OpenTelemetry.Exporter.Prometheus;


var logger = new LoggerConfiguration()
    .MinimumLevel.Debug() 
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        fileSizeLimitBytes: 10_000_000,
        rollOnFileSizeLimit: true,
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1))
    .CreateLogger();

Log.Logger = logger;
logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args
});
builder.Host.UseSerilog(logger);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenTelemetry()
    .ConfigureResource(rb => rb.AddService("ObservabilityDemo"))
    .WithTracing(tracerProvider =>
    {
        tracerProvider
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddJaegerExporter(o =>
            {
                o.AgentHost = "localhost";
                o.AgentPort = 6832;
            });
    })
    .WithMetrics(metricsProvider =>
    {
        metricsProvider
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter();
    });
builder.Services.AddHealthChecks()
    // .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
    //               name: "SQL Server")     
    .AddUrlGroup(new Uri("https://localhost:57679/orders"), name: "Order API") 
    .AddUrlGroup(new Uri("https://localhost:57679/users"), name: "User API") 
    .AddCheck("Custom_Check", () =>
    {
        bool healthy = true; 
        return healthy ? 
            HealthCheckResult.Healthy("Ok") :
            HealthCheckResult.Unhealthy("Not Ok");
    });


builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new OrderAppModule());
});

builder.Services.AddAutoMapper(typeof(Automapper).Assembly);
builder.Services.AddHostedService<OrderConsumer>();
// builder.AddLoggerConfigs();

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Guard.Against.Null(connectionString, nameof(connectionString));

builder.Services.Configure<MySecretsOptions>(
    builder.Configuration.GetSection("MySecrets"));
builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("Smtp"));


builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{    
    var secrets = sp.GetRequiredService<IOptions<MySecretsOptions>>().Value;
    var interceptor = sp.GetRequiredService<AuditSaveChangesInterceptor>();
    options.UseSqlServer(secrets.ConnectionString);
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "redis-server";
});
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("JwtSettings"));



// builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


var loggerFactory = new SerilogLoggerFactory(Log.Logger);
var appLogger = loggerFactory.CreateLogger<Program>();

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .WithOrigins("http://127.0.0.1:5500")
              .WithOrigins("https://localhost:5500") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "OrderApp",
        ValidAudience = "Users",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyForJwtTokenGeneration123456"))
    };
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();



builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
builder.Services.AddTransient<IAuthorizationHandler, RoutePermissionAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RoutePermissionPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});
builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.ShortSchemaNames = true;
                })
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                ;
builder.Services.AddSignalR();

try
{
    
    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapPrometheusScrapingEndpoint();
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.UseCors();
    app.MapHub<ChatHub>("/chathub", options =>
    {
        options.Transports = HttpTransportType.WebSockets |
                            HttpTransportType.ServerSentEvents |
                            HttpTransportType.LongPolling;
    });
    app.MapHub<NotificationHub>("/notificationhub", options =>
    {
        options.Transports = HttpTransportType.WebSockets |
                            HttpTransportType.ServerSentEvents |
                            HttpTransportType.LongPolling;
    });

    app.MapGet("/", () => "RabbitMQ Consumer is running...");
    app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
    {
        ShowStatusCode = true,
        UseCustomSchema = false,
    }); 

    await app.UseAppMiddlewareAndSeedDatabase();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
