using charity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace charity.Controllers;
[AuthorizationFilter]
[Route("donation")]
public class DonationController : Controller
{
    private ApplicationDbContext _context;

    public DonationController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("freemarket")]
    public IActionResult FreeMarket()
    {
        var donations = _context.Donations.Include(d => d.Donor).Where(d => d.Status == DonationStatus.Available)
            .ToList();
        return View(donations);
    }

    // free market item details
    [HttpGet("freemarket/{id}")]
    public IActionResult FreeMarketItem(int id)
    {
        var donation = _context.Donations.Include(d => d.Donor).FirstOrDefault(d => d.Id == id);
        return View(donation);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(DonationModel model)
    {
        ModelState.Remove("Donor");
        if (ModelState.IsValid)
        {
            var currentUser = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == currentUser);
            if (user != null)
            {
                model.Donor = user;
                model.AddDate = DateTime.UtcNow;
                model.Status = DonationStatus.Available;
                _context.Add(model);
                _context.SaveChanges();

                return RedirectToAction("FreeMarket", "Donation");
            }
            else
            {
                return NotFound();
            }
        }

        // print error

        return View(model);
    }

    //confirm donation and bind it to user
    [HttpGet("confirm/{id}")]
    public IActionResult Confirm(int id)
    {
        var currentUser = HttpContext.Session.GetString("UserEmail");
        var user = _context.Users.FirstOrDefault(u => u.Email == currentUser);
        var donation = _context.Donations.Include(d => d.Donor).FirstOrDefault(d => d.Id == id);
        if (donation != null && user != null)
        {
            donation.Status = DonationStatus.Occupied;
            donation.Receiver = user;
            _context.SaveChanges();
            return RedirectToAction("FreeMarket", "Donation");
        }
        else
        {
            return NotFound();
        }
    }
}