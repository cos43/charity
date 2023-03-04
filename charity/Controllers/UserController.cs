using charity.Models;
using Microsoft.AspNetCore.Mvc;

namespace charity.Controllers;

[Route("")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email, string password)
    {
        // get login user
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user != null && UserModel.ComputeMD5Hash(password)==user.Password)
        {
            HttpContext.Session.Set("UserId", System.Text.Encoding.UTF8.GetBytes(user.Id.ToString()));
            HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("Role", user.Role.ToString());
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }
    }

    //logout
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "User");
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public IActionResult Register(UserModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if email already exists
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "Email already exists.");
            }
            else
            {
                model.Password = UserModel.ComputeMD5Hash(model.Password);
                _context.Users.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }

        return View(model);
    }


    [HttpGet("profile")]
    public IActionResult Profile()
    {
        //get userid from session
        var userId = HttpContext.Session.Get("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "User");
        }

        //convert userid to int
        // get user
        var user = _context.Users.Find(int.Parse(System.Text.Encoding.UTF8.GetString(userId)));
        // If the user's profile does not exist in the database, return a 404 error
        if (user == null)
        {
            return NotFound();
        }

        // Pass the user's profile to the view
        return View(user);
    }

    [HttpPost("profile")]
    [ValidateAntiForgeryToken]
    public IActionResult Profile(UserModel user)
    {
        var currentUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);
        if (currentUser != null)
        {
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.PhoneNumber = user.PhoneNumber;
            currentUser.Address = user.Address;
            HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
            _context.SaveChanges();
        }

        // Redirect the user to their updated profile
        return RedirectToAction("Profile", "User");
    }

    //preview user's information by userId
    [HttpGet("preview/{id}")]
    public IActionResult Preview(int id)
    {
        //get user
        var user = _context.Users.Find(id);
        // If the user's profile does not exist in the database, return a 404 error
        if (user == null)
        {
            return NotFound();
        }

        // get received donation
        var receivedDonation = _context.Donations.Where(d => d.Receiver == user).ToList();
        // Pass the user's profile and received donation to the view
        ViewBag.ReceivedDonation = receivedDonation;
        return View(user);
    }


    // show needy people list
    [HttpGet("needy")]
    public IActionResult NeedyPeople()
    {
        //get all needy people
        var needyPeople = _context.Users.Where(u => u.Role == UserRole.NeedyPeople).ToList();
        return View(needyPeople);
    }
}