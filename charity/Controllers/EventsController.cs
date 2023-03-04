using charity.Models;
using Microsoft.AspNetCore.Mvc;

namespace charity.Controllers;

[AuthorizationFilter]
[Route("events")]
public class EventsController : Controller
{
    private ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // list event
    [HttpGet("")]
    public IActionResult Index()
    {
        var events = _context.Events.ToList();
        return View(events);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EventsModel model)
    {
        if (ModelState.IsValid)
        {
            _context.Events.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return View(model);
        }
    }

    // events detail
    [HttpGet("{id}")]
    public IActionResult EventsItem(int id)
    {
        // get current user
        var email = HttpContext.Session.GetString("UserEmail");
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        var joined = false;
        var events = _context.Events.Find(id);
        if (user != null)
        {
            joined = _context.UserEvents.Any(ue => ue.Event == events && ue.User == user);
        }

        ViewBag.joined = joined;
        return View(events);
    }

    //join event
    [HttpGet("join/{id}")]
    public IActionResult Join(int id)
    {
        var events = _context.Events.Find(id);
        var email = HttpContext.Session.GetString("UserEmail");
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user != null && events != null)
        {
            _context.UserEvents.Add(new UserEventsModel
            {
                Event = events,
                User = user
            });
            _context.SaveChanges();
            // return to detail
            return RedirectToAction("EventsItem", new { id = id });
        }
        else
        {
            return NotFound();
        }
    }
}