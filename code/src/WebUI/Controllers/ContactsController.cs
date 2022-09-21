using Application.Contacts.Queries;
using Microsoft.AspNetCore.Mvc;

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
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: ContactsController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: ContactsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: ContactsController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: ContactsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: ContactsController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: ContactsController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
