using DiagnostykaItemsAdministrationService.Api;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;

[SetUpFixture]
public class TestsStartup
{
    private static IConfiguration _configuration { get; set; }
    private static IServiceScopeFactory _scopeFactory { get; set; }
    private static Checkpoint _checkpoint { get; set; }
    public static AppDbContext DbContext { get; set; }

    [OneTimeSetUp]
    public async Task Setup()
    {
        _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

        var webHostMock = Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "DiagnostykaItemsAdministrationService.Api");

        var startup = new Startup(_configuration, webHostMock);

        var services = new ServiceCollection();

        services.AddSingleton(webHostMock);

        startup.ConfigureServices(services);

        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
        };

        await EnsureDatabaseAsync();
    }

    public static async Task EnsureDatabaseAsync()
    {
        using var scope = _scopeFactory.CreateAsyncScope();

        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }

    public static async Task ResetState()
    {
        await _checkpoint.Reset(_configuration.GetConnectionString("SqlDatabase"));
    }

    public static AppDbContext GetDbContext() => _scopeFactory.CreateAsyncScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public static ISender GetMediator() => _scopeFactory.CreateAsyncScope().ServiceProvider.GetRequiredService<ISender>();
}