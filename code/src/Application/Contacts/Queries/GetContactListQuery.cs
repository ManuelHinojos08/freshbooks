using Application.Common.Interfaces;
using Application.Contacts.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contacts.Queries;

public class GetContactListQuery : IRequest<IList<ContactDto>>
{
}
public class GetContactListQueryHandler : IRequestHandler<GetContactListQuery, IList<ContactDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<ContactDto>> Handle(GetContactListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .OrderBy(x => x.Name)
            .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
