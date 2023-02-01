using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.DeleteColor;

public class DeleteColorCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteColorCommandHandler : IRequestHandler<DeleteColorCommand>
{
    private readonly AppDbContext _dbContext;

    public DeleteColorCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
    {
        var colorEntity = await _dbContext.Colors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (colorEntity is null)
            throw new NotFoundException($"Color of id: {request.Id} not found");

        _dbContext.Colors.Remove(colorEntity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}