using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Models;
using System.Diagnostics;

namespace PizzaCore.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger) {
      this.logger = logger;
    }

    // GET: /
    public IActionResult Index() {
      return View();
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
