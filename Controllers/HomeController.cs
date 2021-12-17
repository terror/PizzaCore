using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Data;
using PizzaCore.Helpers;
using PizzaCore.Models;
using System.Diagnostics;

namespace PizzaCore.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> logger;
    private readonly IPizzaCoreRepository repository;

    public HomeController(ILogger<HomeController> logger, IPizzaCoreRepository repository) {
      this.repository = repository;
      this.logger = logger;
    }

    // GET: /
    public IActionResult Index() {

      
      if(SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, SessionHelper.EMPLOYEE_SIGN_IN_KEY) == true)
      {
        if (User.IsInRole("Manager") || User.IsInRole("Owner"))
        {
          return RedirectToAction("Index", "Dashboard");
        }
        else if (User.IsInRole("Cook"))
        {
          return RedirectToAction("Index", "Cook");
        }
        else if (User.IsInRole("Staff"))
        {
          return RedirectToAction("Index", "Employee");
        }
      }
      

      var products = repository.GetFeaturedProducts();
      return View(products);
    }

    // GET: /about
    [HttpGet("about")]
    public IActionResult About() {
      return View();
    }

    // GET: /covid
    [HttpGet("covid")]
    public IActionResult Covid() {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
