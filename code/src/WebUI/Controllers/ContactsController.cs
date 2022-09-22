using Application.Contacts.Commands;
using Application.Contacts.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebUI.Controllers;

public class ContactsController : CustomControllerBase
{
    // GET: ContactsController
    public async Task<IActionResult> Index()
    {
        var contacts = await Mediator.Send(new GetContactListQuery());
        return View(contacts);
    }

    // GET: ContactsController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var contact = await Mediator.Send(new GetContactDetailsQuery() { Id = id });
        return View(contact);
    }

    // GET: ContactsController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: ContactsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateContactCommand command)
    {
        await Mediator.Send(command);
        TempData[WebConsts.SavedKey] = $"{true}".ToLower();
        return RedirectToAction(nameof(Index));
    }

    // GET: ContactsController/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var contact = await Mediator.Send(new GetContactDetailsQuery() { Id = id });
        var model = Mapper.Map<UpdateContactCommand>(contact);
        return View(model);
    }

    // POST: ContactsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateContactCommand command)
    {
        await Mediator.Send(command);
        TempData[WebConsts.SavedKey] = $"{true}".ToLower();
        return RedirectToAction("Index");
    }

    // GET: ContactsController/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteContactCommand() { Id = id });
        TempData[WebConsts.SavedKey] = $"{true}".ToLower();
        return RedirectToAction("Index");
    }
}
