using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Data;
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
      repository.GetTodayOrders();
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
