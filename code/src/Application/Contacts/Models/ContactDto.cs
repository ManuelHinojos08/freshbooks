using Application.Common.Mappings;
using Application.Contacts.Abstractions;
using Domain.Entities;

namespace Application.Contacts.Models;

public class ContactDto: ContactBaseDto, IMapFrom<Contact>
{

}
