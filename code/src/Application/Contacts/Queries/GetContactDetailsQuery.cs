using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Contacts.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contacts.Queries;

public class GetContactDetailsQuery : IRequest<ContactDto>
{
    public int Id { get; set; }
}

public class GetContactDetailsQueryHandler : IRequestHandler<GetContactDetailsQuery, ContactDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContactDto> Handle(GetContactDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Contact), request.Id);

        return _mapper.Map<ContactDto>(entity);
    }
}