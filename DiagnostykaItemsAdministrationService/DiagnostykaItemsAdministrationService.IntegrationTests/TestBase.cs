using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using NUnit.Framework;

namespace DiagnostykaItemsAdministrationService.IntegrationTests;

using static TestsStartup;

public class TestBase
{
    protected AppDbContext _dbContext { get; set; }
    protected ISender _sender { get; set; }
    [SetUp]
    public async Task SetUp()
    {
        await ResetState();
        _dbContext = GetDbContext();
        _sender = GetMediator();
    }
}