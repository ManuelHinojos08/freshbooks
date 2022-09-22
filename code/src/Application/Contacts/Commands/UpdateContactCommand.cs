using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Contacts.Abstractions;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contacts.Commands;

public class UpdateContactCommand : ContactBaseDto, IRequest
{

}

public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
{
    public UpdateContactCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(256)
            .NotEmpty();

        RuleFor(v => v.Email)
            .EmailAddress()
            .MaximumLength(256)
            .NotEmpty();

        RuleFor(v => v.Phone)
            .MaximumLength(256)
            .NotEmpty();
    }
}

public class UpdateContactCommandCommandHandler : IRequestHandler<UpdateContactCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactCommandCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Contact), request.Id);
        
        entity.Name = request.Name;
        entity.Email = request.Email;
        entity.Phone = request.Phone;

        _context.Contacts.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}