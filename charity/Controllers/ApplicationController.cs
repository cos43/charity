using charity.Models;
using Microsoft.AspNetCore.Mvc;


namespace charity.Controllers;
[AuthorizationFilter]
[Route("application")]
public class ApplicationController : Controller
{
    private readonly ApplicationDbContext _context;

    public ApplicationController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ApplicationModel model)
    {
        ModelState.Remove("user");
        if (ModelState.IsValid)
        {
            model.User = _context.Users.Find(1);
            model.SubmissionDate = DateTime.UtcNow;
            model.Status = ApplicationStatus.Pending;

            _context.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Status", "Application");
        }

        // print error

        return View(model);
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        var applications = _context.Applications.ToList();
        return View(applications);
    }
}