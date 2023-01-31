using DiagnostykaItemsAdministrationService.Application.Common.Helpers.Interfaces;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using System.ComponentModel;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;

public class CreateItemCommand : IRequest<int>
{
    public string Name { get; set; }
    public int ColorId { get; set; }
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, int>
{
    private readonly AppDbContext _dbContext;
    private readonly IHashGenerator _hashGenerator;

    public CreateItemCommandHandler(AppDbContext dbContext, IHashGenerator hashGenerator)
    {
        _dbContext = dbContext;
        _hashGenerator = hashGenerator;
    }

    public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var entityEntry = await _dbContext.Items.AddAsync(new Domain.Entities.Item()
        {
            Name = request.Name,
            Code = "000000000000",
            ColorId = request.ColorId
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        //Temporary workaround
        var code = _hashGenerator.GenerateHash(entityEntry.Entity.Id);
        entityEntry.Entity.Code = code;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entityEntry.Entity.Id;
    }
}