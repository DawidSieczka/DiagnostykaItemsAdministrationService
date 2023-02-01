using DiagnostykaItemsAdministrationService.Domain.Entities;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.CreateColor;

public class CreateColorCommand : IRequest<int>
{
    public string Name { get; set; }
}

public class CreateColorCommandHandler : IRequestHandler<CreateColorCommand, int>
{
    private readonly AppDbContext _dbContext;

    public CreateColorCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CreateColorCommand request, CancellationToken cancellationToken)
    {
        var entityEntry = await _dbContext.Colors.AddAsync(new Color()
        {
            Name = request.Name,
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entityEntry.Entity.Id;
    }
}