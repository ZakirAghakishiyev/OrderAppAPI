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

var logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Change to Debug or Information
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

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new OrderAppModule());
});       

builder.Services.AddAutoMapper(typeof(Automapper).Assembly);
// builder.AddLoggerConfigs();

builder.Services.AddControllers();
//builder.Services.AddAutoWrapper();
// var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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


// builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


var loggerFactory = new SerilogLoggerFactory(Log.Logger);
var appLogger = loggerFactory.CreateLogger<Program>();

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);

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

builder.Services.AddInfrastructure();
builder.Services.AddHttpContextAccessor();


builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
builder.Services.AddTransient<IAuthorizationHandler, RoutePermissionAuthorizationHandler>(); 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RoutePermissionPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.ShortSchemaNames = true;
                })
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                ;


try
{
    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
    {
        ShowStatusCode = true,
        UseCustomSchema = false,
    }); ;

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
