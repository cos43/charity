using charity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace charity.Controllers;

[AuthorizationFilter]
[Route("admin")]
public class AdminController : Controller
{
    private ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // account list
    [HttpGet("account")]
    public IActionResult AccountList()
    {
        var users = _context.Users.ToList();
        return View(users);
    }

    //update account
    [HttpGet("account/update/{id}")]
    public IActionResult AccountUpdate(int id)
    {
        var user = _context.Users.Find(id);
        return View(user);
    }

    // handle update account
    [HttpPost("account/update/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult AccountUpdate(UserModel user)
    {
        var currentUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);
        ModelState.Remove("Password");
        ModelState.Remove("Email");
        if (ModelState.IsValid)
        {
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.PhoneNumber = user.PhoneNumber;
            currentUser.Address = user.Address;
            _context.SaveChanges();
            return RedirectToAction("AccountList", "Admin");
        }
        else
        {
            return View(user);
        }
    }

    //delete user
    [HttpGet("account/delete/{id}")]
    public IActionResult AccountDelete(int id)
    {
        var user = _context.Users.Find(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction("AccountList", "Admin");
    }


    //Application List
    [HttpGet("application")]
    public IActionResult ApplicationList()
    {
        // get applications include User
        var applications = _context.Applications.Include(x => x.User).ToList();
        return View(applications);
    }

    //approve application
    [HttpGet("application/approve/{id}")]
    public IActionResult ApplicationApprove(int id)
    {
        var application = _context.Applications.Find(id);
        if (application != null)
        {
            application.Status = ApplicationStatus.Approved;
            application.ApprovalDate = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("ApplicationList", "Admin");
        }
        else
        {
            return NotFound();
        }
    }

    //reject application
    [HttpGet("application/reject/{id}")]
    public IActionResult ApplicationReject(int id)
    {
        var application = _context.Applications.Find(id);
        if (application != null)
        {
            application.Status = ApplicationStatus.Rejected;
            _context.SaveChanges();
            return RedirectToAction("ApplicationList", "Admin");
        }
        else
        {
            return NotFound();
        }
    }

    //DonationList
    [HttpGet("donation")]
    public IActionResult DonationList()
    {
        var donations = _context.Donations.Include(x => x.Donor).Include(x => x.Receiver).ToList();
        return View(donations);
    }

    //delete donation
    [HttpGet("donation/delete/{id}")]
    public IActionResult DonationDelete(int id)
    {
        var donation = _context.Donations.Find(id);
        if (donation != null)
        {
            _context.Donations.Remove(donation);
            _context.SaveChanges();
            return RedirectToAction("DonationList", "Admin");
        }
        else
        {
            return NotFound();
        }
    }
}