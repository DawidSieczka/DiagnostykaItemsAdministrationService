using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.UpdateItem;

public class UpdateItemCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ColorId { get; set; }
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
{
    private readonly AppDbContext _appDbContext;

    public UpdateItemCommandHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var itemEntity = await _appDbContext.Items.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (itemEntity is null)
            throw new Exception($"Item of Id: {request.Id} can not be found and removed.");

        var colorEntity = await _appDbContext.Colors.FirstOrDefaultAsync(x => x.Id == request.ColorId, cancellationToken);
        if (colorEntity is null)
            throw new Exception($"Color of Id {request.ColorId} not found");

        itemEntity.Id = request.Id;
        itemEntity.Name = request.Name;
        itemEntity.ColorId = request.ColorId;

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}