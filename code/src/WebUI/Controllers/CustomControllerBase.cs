using Application.Contacts.Commands;
using Application.Contacts.Models;
using AutoMapper;
using Infrastructure.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[TypeFilter(typeof(BaseExceptionFilter))]
public abstract class CustomControllerBase : Controller
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    /*We will place here all mappings that are not directly needed by application layer, but for presentation
     purposes, like mapping dto for detail display to update command populated with the current value for the given entity*/
    private IMapper _mapper;
    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>()!;

    public CustomControllerBase()
    {
        var config = new MapperConfiguration(cfg => {
            /*Please add a new line with the same syntax when additional mapping is needed.
            cfg.CreateMap<SourceClass, DestinationClass>();
            Be sure all source properties have the same name as destination one, or to include
            the proper individual member configurations */
            cfg.CreateMap<ContactDto, UpdateContactCommand>();
        });
        _mapper = config.CreateMapper();
    }

    public new IActionResult NotFound() => View();

    public new IActionResult Forbid() => View();
}