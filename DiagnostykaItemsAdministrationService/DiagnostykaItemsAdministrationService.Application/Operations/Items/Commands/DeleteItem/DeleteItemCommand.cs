using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.DeleteItem;

public class DeleteItemCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
{
    private readonly AppDbContext _dbContext;

    public DeleteItemCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var itemEntity = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (itemEntity is null)
            throw new Exception($"Item of Id: {request.Id} can not be found and removed.");

        _dbContext.Items.Remove(itemEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}