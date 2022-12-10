using System.Net;
using DirectoryService.Api.Extensions;
using DirectoryService.Api.Middleware;
using DirectoryService.Api.Middleware.Authentication;
using DirectoryService.Api.Policies;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Config;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;

// ReSharper disable CommentTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable global StringLiteralTypo

namespace DirectoryService.Api;

public class Startup
{
    private ILogger<Startup>? _logger;
    private IConfiguration ConfigRoot { get; }

    public Startup(IConfiguration configuration)
    {
        _logger = null;
        ConfigRoot = configuration;
    }

    /// <summary>
    /// Configure Services
    /// </summary>
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(o =>
                o.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy());

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddLogging(opt =>
            {
                opt.AddSystemdConsole();
                opt.AddCustomFormatter();
            })
            .Configure<LoggerFilterOptions>(c => c.MinLevel = LogLevel.Information);
        
        builder.Services.RegisterServices();

        SetupConfiguration();

        var config = ServicesConfigContainer.Config;

        builder.Services.AddFluentEmail(
                ServicesConfigContainer.Config.Smtp.SenderEmail,
                ServicesConfigContainer.Config.Smtp.SenderName
            )
            .AddRazorRenderer()
            .AddSmtpSender(
                ServicesConfigContainer.Config.Smtp.Host ?? "localhost",
                ServicesConfigContainer.Config.Smtp.Port,
                ServicesConfigContainer.Config.Smtp.Username ?? "",
                ServicesConfigContainer.Config.Smtp.Password ?? "");

        // Setup Kestrel with http/https options provided in configuration
        builder.WebHost.UseKestrel((hostingContext, options) =>
        {
            options.Listen(IPAddress.Any, config.Server.HttpPort);
            if (config.Server.UseHttps)
            {
                options.Listen(IPAddress.Any, config.Server.HttpsPort, listenOptions =>
                {
                    if (config.Server.HttpsCertPassword is not null &&  config.Server.HttpsCertPassword != "")
                    {
                        if (config.Server.HttpsCertFile is null || !File.Exists(config.Server.HttpsCertFile))
                            throw new Exception("Https Certificate not found");
                        
                        listenOptions.UseHttps(config.Server.HttpsCertFile!,
                            config.Server.HttpsCertPassword);
                    }
                    else
                    {
                        if (config.Server.HttpsCertFile is null || !File.Exists(config.Server.HttpsCertFile))
                            throw new Exception("Https Certificate not found");
                        listenOptions.UseHttps(config.Server.HttpsCertFile!);
                    }
                });
            }
        });
    }

    /// <summary>
    /// Configure Application
    /// </summary>
    public void Configure(WebApplication app)
    {
        _logger = app.Services.GetRequiredService<ILogger<Startup>>();

        ConfigureForwardHeaders(app);

        app.UseMiddleware<AuthMiddleware>();
        app.UseStatusCodeExceptionHandler();
        app.UseApiExceptionHandler();
        app.MapControllers();
        
        if (!DatabaseMigrator.MigrateDatabase(_logger))
            return;

        // If Development then truncate data and seed dev data
        if (app.Environment.IsDevelopment())
        {
            var dbContext = app.Services.GetRequiredService<DbContext>();
            _logger.LogWarning("Development Mode: Truncating all data in 5 seconds. Terminate application NOW if using production data.");
            Thread.Sleep(5000);
            dbContext.RunScript("truncateAll.sql");
            _logger.LogInformation("Database truncated. Seeding dev data");
            dbContext.RunScript("devSeed.sql");
        }
        app.Run();
    }

    /// <summary>
    /// Separating appsettings.json from service configuration to prevent
    /// mishaps that could break the way Kestrel operates
    /// </summary>
    private static void SetupConfiguration()
    {
        using var r = new StreamReader("./config/serviceConfig.json");
        var json = r.ReadToEnd();
        
        ServicesConfigContainer.Config = JsonConvert.DeserializeObject<ServiceConfiguration>(json)!;
    }

    /// <summary>
    /// If behind reverse proxies (such as Cloudflare) getting requester IP
    /// will not be simple, so providing known proxy IP addresses will help accuracy
    /// </summary>
    /// <param name="app"></param>
    private void ConfigureForwardHeaders(IApplicationBuilder app)
    {
        
        if (ServicesConfigContainer.Config.Server.KnownProxies!.Count == 0)
            return;

        _logger!.LogInformation("Configuring Forwarded Headers");
        var opts = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All,
            RequireHeaderSymmetry = false,
            ForwardLimit = null,
        };
        foreach (var proxy in ServicesConfigContainer.Config.Server.KnownProxies)
        {
            opts.KnownProxies.Add(IPAddress.Parse(proxy));
        }

        app.UseForwardedHeaders(opts);
    }
}