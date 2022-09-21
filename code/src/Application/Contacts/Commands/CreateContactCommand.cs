using Application.Common.Interfaces;
using Application.Contacts.Abstractions;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Contacts.Commands;

public class CreateContactCommand : ContactBaseDto, IRequest<int>
{
}

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator()
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

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var entity = new Contact
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone
        };
        
        _context.Contacts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
