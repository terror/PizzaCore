using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Data;
using PizzaCore.Models;
using PizzaCore.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly ReCaptcha _captcha;
    private readonly PizzaCoreContext _context;

    public HomeController(ILogger<HomeController> logger, ReCaptcha captcha, PizzaCoreContext context) {
      _logger = logger;
      _captcha = captcha;
      _context = context;
    }

    public IActionResult Index() {
      return View();
    }

    public IActionResult About() {
      return View();
    }

    public IActionResult Privacy() {
      return View();
    }

    [HttpGet("contact")]
    public IActionResult Contact() {
      return View();
    }

    [HttpPost("contact")]
    public async Task<IActionResult> ContactAsync(ContactModel contact) {
      if (ModelState.IsValid) {
        var captcha = Request.Form["g-recaptcha-response"].ToString();

        if (await _captcha.IsValid(captcha)) {
          // Add the contact to the database.
          _context.Add(contact.setDate(DateTime.Now));
          await _context.SaveChangesAsync();

          // Call the view Success and send the contact model
          return View("Success", contact);
        }
      }

      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
