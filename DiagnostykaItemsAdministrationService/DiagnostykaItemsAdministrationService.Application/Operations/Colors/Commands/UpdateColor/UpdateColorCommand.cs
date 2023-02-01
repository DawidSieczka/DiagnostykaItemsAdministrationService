using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.UpdateColor;
public class UpdateColorCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class UpdateColorCommandHandler : IRequestHandler<UpdateColorCommand>
{
    private readonly AppDbContext _dbContext;

    public UpdateColorCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
    {
        var colorEntity = await _dbContext.Colors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (colorEntity is null)
            throw new NotFoundException($"Color of id: {request.Id} not found");

        colorEntity.Name = request.Name;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
