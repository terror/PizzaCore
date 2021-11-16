using Assignment3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly ReCaptcha _captcha;

    public HomeController(ILogger<HomeController> logger, ReCaptcha captcha) {
      _logger = logger;
      _captcha = captcha;
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
    public IActionResult Contact()
    {
      return View();
    }

    [HttpPost("contact")]
    public async Task<IActionResult> ContactAsync(ContactModel contact)
    {
      if (ModelState.IsValid)
      {
        var captcha = Request.Form["g-recaptcha-response"].ToString();

        if (await _captcha.IsValid(captcha))
        {
          // Add the contact to the database.

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
